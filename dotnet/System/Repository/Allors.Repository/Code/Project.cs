// -------------------------------------------------------------------------------------------------
// <copyright file="RepositoryProject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Allors.Repository.Code
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Buildalyzer;
    using Buildalyzer.Workspaces;
    using Domain;
    using Inflector;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using NLog;

    public class Project
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Inflector inflector;

        private readonly string projectPath;

        public Project(string projectPath)
        {
            this.projectPath = projectPath;
            this.inflector = new Inflector(new CultureInfo("en"));
        }

        public async System.Threading.Tasks.Task InitializeAsync()
        {
            var analyzerManager = new AnalyzerManager();

            var projectAnalyzer = analyzerManager.GetProject(this.projectPath);
            var workspace = projectAnalyzer.GetWorkspace();
            var solution = workspace.CurrentSolution;
            var project = solution.Projects.First();

            this.Solution = project.Solution;
            this.Compilation = await project.GetCompilationAsync();

            if (this.Compilation == null)
            {
                throw new ArgumentException($"Can not get compilation for {this.projectPath}", nameof(this.projectPath));
            }

            this.SemanticModelBySyntaxTree = this.Compilation.SyntaxTrees.ToDictionary(v => v, v => this.Compilation.GetSemanticModel(v));
            this.DocumentBySyntaxTree = this.Compilation.SyntaxTrees.ToDictionary(v => v, v => this.Solution.GetDocument(v));

            this.DomainAttributeType = this.Compilation.GetTypeByMetadataName(Repository.AttributeNamespace + ".DomainAttribute");
            this.ExtendAttributeType = this.Compilation.GetTypeByMetadataName(Repository.AttributeNamespace + ".ExtendsAttribute");

            using var ms = new MemoryStream();
            var result = this.Compilation.Emit(ms);

            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                var text = string.Join("\n", failures.Select(v => v.Id + ": " + v.GetMessage()));
                throw new ArgumentException($"Can not emit assembly for {this.projectPath}\n{text}", nameof(this.projectPath));
            }

            ms.Seek(0, SeekOrigin.Begin);
            this.Assembly = Assembly.Load(ms.ToArray());

            this.Repository = new Repository();

            this.CreateDomains();

            this.CreateUnits();
            this.CreateTypes();
            this.CreateHierarchy();
            this.CreateRecords();
            this.CreateMembers();

            this.FromReflection();

            this.LinkImplementations();

            this.CreateInheritedProperties();
            this.CreateReverseProperties();

            var ids = new HashSet<Guid>();

            foreach (var composite in this.Repository.Composites)
            {
                this.CheckId(ids, composite.Id, $"{composite.SingularName}", "id");

                // TODO: Add id checks for properties
                foreach (var property in composite.DefinedProperties)
                {
                    this.CheckId(ids, property.Id, $"{composite.SingularName}.{property.RoleName}", "id");
                }

                foreach (var method in composite.DefinedMethods)
                {
                    if (!method.AttributeByName.ContainsKey(AttributeNames.Id))
                    {
                        this.HasErrors = true;
                        Logger.Error($"{method} has no {AttributeNames.Id} attribute.");
                    }
                }
            }
        }

        public Repository Repository { get; private set; }

        public Solution Solution { get; private set; }

        public Compilation Compilation { get; private set; }

        public Dictionary<SyntaxTree, SemanticModel> SemanticModelBySyntaxTree { get; private set; }

        public Dictionary<SyntaxTree, Document> DocumentBySyntaxTree { get; private set; }

        public INamedTypeSymbol DomainAttributeType { get; private set; }

        public INamedTypeSymbol ExtendAttributeType { get; private set; }

        public Assembly Assembly { get; private set; }

        public bool HasErrors { get; set; }

        protected void CreateUnits()
        {
            var domain = this.Repository.Domains.First();
            var unitBySingularName = this.Repository.UnitBySingularName;
            var typeBySingularName = this.Repository.TypeBySingularName;

            var binary = new Unit(UnitIds.Binary, UnitNames.Binary, domain);
            unitBySingularName.Add(binary.SingularName, binary);
            typeBySingularName.Add(binary.SingularName, binary);

            var boolean = new Unit(UnitIds.Boolean, UnitNames.Boolean, domain);
            unitBySingularName.Add(boolean.SingularName, boolean);
            typeBySingularName.Add(boolean.SingularName, boolean);

            var datetime = new Unit(UnitIds.DateTime, UnitNames.DateTime, domain);
            unitBySingularName.Add(datetime.SingularName, datetime);
            typeBySingularName.Add(datetime.SingularName, datetime);

            var @decimal = new Unit(UnitIds.Decimal, UnitNames.Decimal, domain);
            unitBySingularName.Add(@decimal.SingularName, @decimal);
            typeBySingularName.Add(@decimal.SingularName, @decimal);

            var @float = new Unit(UnitIds.Float, UnitNames.Float, domain);
            unitBySingularName.Add(@float.SingularName, @float);
            typeBySingularName.Add(@float.SingularName, @float);

            var integer = new Unit(UnitIds.Integer, UnitNames.Integer, domain);
            unitBySingularName.Add(integer.SingularName, integer);
            typeBySingularName.Add(integer.SingularName, integer);

            var @string = new Unit(UnitIds.String, UnitNames.String, domain);
            unitBySingularName.Add(@string.SingularName, @string);
            typeBySingularName.Add(@string.SingularName, @string);

            var unique = new Unit(UnitIds.Unique, UnitNames.Unique, domain);
            unitBySingularName.Add(unique.SingularName, unique);
            typeBySingularName.Add(unique.SingularName, unique);
        }

        private void CreateDomains()
        {
            try
            {
                var parentByChild = new Dictionary<string, string>();

                foreach (var syntaxTree in this.DocumentBySyntaxTree.Keys)
                {
                    var root = syntaxTree.GetRoot();
                    foreach (var structDeclaration in root.DescendantNodes().OfType<StructDeclarationSyntax>())
                    {
                        var semanticModel = this.Compilation.GetSemanticModel(syntaxTree);
                        var structureModel = (ITypeSymbol)semanticModel.GetDeclaredSymbol(structDeclaration);
                        var domainAttribute = structureModel.GetAttributes()
                            .FirstOrDefault(v => v.AttributeClass.Name.Equals("DomainAttribute"));

                        if (domainAttribute != null)
                        {
                            var id = Guid.Parse((string)domainAttribute.ConstructorArguments.First().Value);

                            var document = this.DocumentBySyntaxTree[syntaxTree];
                            var fileInfo = new FileInfo(document.FilePath);
                            var directoryInfo = new DirectoryInfo(fileInfo.DirectoryName);

                            var domain = new Domain(id, structureModel.Name, directoryInfo);
                            this.Repository.DomainByName.Add(domain.Name, domain);

                            var extendsAttribute = structureModel.GetAttributes()
                                .FirstOrDefault(v => v.AttributeClass.Name.Equals("ExtendsAttribute"));
                            var parent = (string)extendsAttribute?.ConstructorArguments.First().Value;

                            if (!string.IsNullOrEmpty(parent))
                            {
                                parentByChild.Add(domain.Name, parent);
                            }
                        }
                    }
                }

                foreach (var child in parentByChild.Keys)
                {
                    var parent = parentByChild[child];
                    this.Repository.DomainByName[child].Base = this.Repository.DomainByName[parent];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private void CreateTypes()
        {
            foreach (var syntaxTree in this.DocumentBySyntaxTree.Keys)
            {
                var root = syntaxTree.GetRoot();
                var semanticModel = this.SemanticModelBySyntaxTree[syntaxTree];
                var document = this.DocumentBySyntaxTree[syntaxTree];
                var fileInfo = new FileInfo(document.FilePath);

                foreach (var interfaceDeclaration in root.DescendantNodes().OfType<InterfaceDeclarationSyntax>())
                {
                    var typeModel = (ITypeSymbol)semanticModel.GetDeclaredSymbol(interfaceDeclaration);
                    var idAttribute = typeModel.GetAttributes().FirstOrDefault(v => v.AttributeClass.Name.Equals("IdAttribute"));

                    if (idAttribute != null)
                    {
                        var id = Guid.Parse((string)idAttribute.ConstructorArguments.First().Value);
                        var domain = this.Repository.Domains.First(v => v.DirectoryInfo.Contains(fileInfo));
                        var symbol = semanticModel.GetDeclaredSymbol(interfaceDeclaration);
                        var interfaceSingularName = symbol.Name;

                        var @interface = new Interface(this.inflector, id, interfaceSingularName, domain);
                        var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                        @interface.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;

                        this.Repository.InterfaceBySingularName.Add(interfaceSingularName, @interface);
                        this.Repository.CompositeByName.Add(interfaceSingularName, @interface);
                        this.Repository.TypeBySingularName.Add(interfaceSingularName, @interface);
                    }
                }

                foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    var typeModel = (ITypeSymbol)semanticModel.GetDeclaredSymbol(classDeclaration);
                    var idAttribute = typeModel.GetAttributes().FirstOrDefault(v => v.AttributeClass.Name.Equals("IdAttribute"));

                    if (idAttribute != null)
                    {
                        var id = Guid.Parse((string)idAttribute.ConstructorArguments.First().Value);
                        var domain = this.Repository.Domains.FirstOrDefault(v => v.DirectoryInfo.Contains(fileInfo));
                        var symbol = semanticModel.GetDeclaredSymbol(classDeclaration);
                        var classSingularName = symbol.Name;

                        var @class = new Class(this.inflector, id, classSingularName, domain);
                        var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                        @class.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;

                        this.Repository.ClassBySingularName.Add(classSingularName, @class);
                        this.Repository.CompositeByName.Add(classSingularName, @class);
                        this.Repository.TypeBySingularName.Add(classSingularName, @class);
                    }
                }
            }
        }

        private void CreateRecords()
        {
            foreach (var syntaxTree in this.DocumentBySyntaxTree.Keys)
            {
                var root = syntaxTree.GetRoot();
                var semanticModel = this.SemanticModelBySyntaxTree[syntaxTree];
                var document = this.DocumentBySyntaxTree[syntaxTree];
                var fileInfo = new FileInfo(document.FilePath);

                foreach (var recordDeclaration in root.DescendantNodes().OfType<RecordDeclarationSyntax>())
                {
                    var domain = this.Repository.Domains.First(v => v.DirectoryInfo.Contains(fileInfo));

                    var symbol = semanticModel.GetDeclaredSymbol(recordDeclaration);
                    if (Repository.RepositoryNamespaceName.Equals(symbol.ContainingNamespace.ToDisplayString()))
                    {
                        var recordName = symbol.Name;

                        if (!this.Repository.RecordByName.TryGetValue(recordName, out var record))
                        {
                            record = new Record(domain, recordName);
                            this.Repository.RecordByName.Add(recordName, record);
                        }

                        var typeModel = (ITypeSymbol)semanticModel.GetDeclaredSymbol(recordDeclaration);

                        var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                        record.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;
                    }
                }
            }
        }

        private void CreateHierarchy()
        {
            var definedTypeBySingularName = this.Assembly.DefinedTypes.Where(v => Repository.RepositoryNamespaceName.Equals(v.Namespace)).ToDictionary(v => v.Name);

            var composites = this.Repository.CompositeByName.Values.ToArray();

            foreach (var composite in composites)
            {
                var definedType = definedTypeBySingularName[composite.SingularName];
                var allInterfaces = definedType.GetInterfaces();
                foreach (var definedImplementedInterface in allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces())))
                {
                    if (this.Repository.InterfaceBySingularName.TryGetValue(definedImplementedInterface.Name, out var implementedInterface))
                    {
                        composite.ImplementedInterfaces.Add(implementedInterface);
                    }
                    else
                    {
                        throw new Exception("Can not find implemented interface " + definedImplementedInterface.Name + " on " + composite.SingularName);
                    }
                }
            }

            foreach (var composite in composites)
            {
                composite.Subtypes = composites.Where(v => v.Interfaces.Contains(composite)).ToArray();
            }
        }

        private void CreateMembers()
        {
            foreach (var syntaxTree in this.DocumentBySyntaxTree.Keys)
            {
                var root = syntaxTree.GetRoot();
                var semanticModel = this.SemanticModelBySyntaxTree[syntaxTree];
                var document = this.DocumentBySyntaxTree[syntaxTree];
                var fileInfo = new FileInfo(document.FilePath);
                var domain = this.Repository.Domains.FirstOrDefault(v => v.DirectoryInfo.Contains(fileInfo));

                if (domain != null)
                {
                    foreach (var typeDeclaration in root.DescendantNodes().Where(v => v is InterfaceDeclarationSyntax or ClassDeclarationSyntax))
                    {
                        var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                        var typeName = typeSymbol.Name;

                        if (this.Repository.CompositeByName.TryGetValue(typeName, out var composite))
                        {
                            foreach (var propertyDeclaration in typeDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>())
                            {
                                _ = new Property(this.inflector, semanticModel, composite, propertyDeclaration);
                            }

                            foreach (var methodDeclaration in typeDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>())
                            {
                                _ = new Method(this.inflector, semanticModel, composite, methodDeclaration);
                            }
                        }
                    }
                }
            }
        }

        private void FromReflection()
        {
            var declaredTypeBySingularName = this.Assembly.DefinedTypes.Where(v => Repository.RepositoryNamespaceName.Equals(v.Namespace)).ToDictionary(v => v.Name);

            foreach (var composite in this.Repository.CompositeByName.Values)
            {
                var reflectedType = declaredTypeBySingularName[composite.SingularName];

                // Type attributes
                {
                    foreach (var group in reflectedType.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType()))
                    {
                        var type = group.Key;
                        var typeName = type.Name;
                        if (typeName.ToLowerInvariant().EndsWith("attribute"))
                        {
                            typeName = typeName.Substring(0, typeName.Length - "attribute".Length);
                        }

                        var attributeUsage = type.GetCustomAttributes<AttributeUsageAttribute>().FirstOrDefault();
                        if (attributeUsage != null && attributeUsage.AllowMultiple)
                        {
                            composite.AttributesByName[typeName] = group.ToArray();
                        }
                        else
                        {
                            composite.AttributeByName[typeName] = group.First();
                        }
                    }
                }

                // Property attributes
                foreach (var property in composite.Properties)
                {
                    var reflectedProperty = reflectedType.GetProperty(property.RoleName);
                    if (reflectedProperty == null)
                    {
                        this.HasErrors = true;
                        Logger.Error($"{reflectedType.Name}.{property.RoleName} should be public");
                        continue;
                    }

                    var propertyAttributesByTypeName = reflectedProperty.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType());

                    var reflectedPropertyType = reflectedProperty.PropertyType;
                    var typeName = this.GetTypeName(reflectedPropertyType);
                    property.Type = this.Repository.TypeBySingularName[typeName];

                    foreach (var group in propertyAttributesByTypeName)
                    {
                        var attributeType = group.Key;
                        var attributeTypeName = attributeType.Name;
                        if (attributeTypeName.ToLowerInvariant().EndsWith("attribute"))
                        {
                            attributeTypeName = attributeTypeName.Substring(
                                0,
                                attributeTypeName.Length - "attribute".Length);
                        }

                        var attributeUsage =
                            attributeType.GetCustomAttributes<AttributeUsageAttribute>().FirstOrDefault();
                        if (attributeUsage != null && attributeUsage.AllowMultiple)
                        {
                            property.AttributesByName.Add(attributeTypeName, group.ToArray());
                        }
                        else
                        {
                            property.AttributeByName.Add(attributeTypeName, group.First());
                        }
                    }

                    if (property.AttributeByName.Keys.Contains("Multiplicity"))
                    {
                        if (reflectedPropertyType.Name.EndsWith("[]"))
                        {
                            if (property.IsRoleOne)
                            {
                                this.HasErrors = true;
                                Logger.Error($"{reflectedType.Name}.{property.RoleName} should be many");
                            }
                        }
                        else if (property.IsRoleMany)
                        {
                            this.HasErrors = true;
                            Logger.Error($"{reflectedType.Name}.{property.RoleName} should be one");
                        }
                    }
                }

                foreach (var method in composite.Methods)
                {
                    var reflectedMethod = reflectedType.GetMethod(method.Name);
                    foreach (var group in reflectedMethod.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType()))
                    {
                        var attributeType = group.Key;
                        var attributeTypeName = attributeType.Name;
                        if (attributeTypeName.ToLowerInvariant().EndsWith("attribute"))
                        {
                            attributeTypeName = attributeTypeName.Substring(0, attributeTypeName.Length - "attribute".Length);
                        }

                        var attributeUsage = attributeType.GetCustomAttributes<AttributeUsageAttribute>().FirstOrDefault();
                        if (attributeUsage != null && attributeUsage.AllowMultiple)
                        {
                            method.AttributesByName.Add(attributeTypeName, group.ToArray());
                        }
                        else
                        {
                            method.AttributeByName.Add(attributeTypeName, group.First());
                        }
                    }
                }
            }
        }

        private void LinkImplementations()
        {
            foreach (var @class in this.Repository.Classes)
            {
                foreach (var property in @class.Properties)
                {
                    property.DefiningProperty = @class.GetImplementedProperty(property);
                }

                foreach (var method in @class.Methods)
                {
                    method.DefiningMethod = @class.GetImplementedMethod(method);
                }
            }
        }

        private string GetTypeName(System.Type type)
        {
            var typeName = type.Name;
            if (typeName.EndsWith("[]"))
            {
                typeName = typeName.Substring(0, typeName.Length - "[]".Length);
            }

            return typeName switch
            {
                "Byte" => "Binary",
                "byte" => "Binary",
                "Boolean" => "Boolean",
                "bool" => "Boolean",
                "Decimal" => "Decimal",
                "decimal" => "Decimal",
                "DateTime" => "DateTime",
                "Double" => "Float",
                "double" => "Float",
                "Int32" => "Integer",
                "int" => "Integer",
                "String" => "String",
                "string" => "String",
                "Guid" => "Unique",
                _ => typeName
            };
        }

        private void CreateInheritedProperties()
        {
            foreach (var @interface in this.Repository.Interfaces)
            {
                foreach (var supertype in @interface.Interfaces)
                {
                    foreach (var property in supertype.DefinedProperties)
                    {
                        @interface.InheritedPropertyByRoleName.Add(property.RoleName, property);
                    }
                }
            }
        }

        private void CreateReverseProperties()
        {
            foreach (var composite in this.Repository.Composites)
            {
                foreach (var property in composite.DefinedProperties)
                {
                    var reverseType = property.Type;
                    var reverseComposite = reverseType as Composite;
                    reverseComposite?.DefinedReversePropertyByAssociationName.Add(property.AssociationName, property);
                }
            }

            foreach (var composite in this.Repository.Composites)
            {
                foreach (var supertype in composite.Interfaces)
                {
                    foreach (var property in supertype.DefinedReverseProperties)
                    {
                        composite.InheritedReversePropertyByAssociationName.Add(property.AssociationName, property);
                    }
                }
            }
        }

        private void CheckId(ISet<Guid> ids, string id, string name, string key)
        {
            if (!Guid.TryParse(id, out var idGuid))
            {
                this.HasErrors = true;
                Logger.Error($"{name} has a non GUID {key}: {id}");
            }

            this.CheckId(ids, idGuid, name, key);
        }

        private void CheckId(ISet<Guid> ids, Guid id, string name, string key)
        {
            if (ids.Contains(id))
            {
                this.HasErrors = true;
                Logger.Error($"{name} has a duplicate {key}: {id}");
            }

            ids.Add(id);
        }
    }
}
