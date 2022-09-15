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
    using System.Threading.Tasks;
    using Buildalyzer;
    using Buildalyzer.Workspaces;
    using Domain;
    using Inflector;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using NLog;
    using TypeInfo = System.Reflection.TypeInfo;

    public class Project
    {
        public const string RepositoryNamespaceName = "Allors.Repository";

        public const string AttributeNamespace = RepositoryNamespaceName + ".Attributes";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Inflector inflector;

        private readonly string projectPath;

        private Dictionary<string, TypeInfo> typeInfoByName;

        public Project(string projectPath)
        {
            this.projectPath = projectPath;
            this.inflector = new Inflector(new CultureInfo("en"));
        }

        public async Task InitializeAsync()
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

            this.DomainAttributeType = this.Compilation.GetTypeByMetadataName(AttributeNamespace + ".DomainAttribute");
            this.ExtendAttributeType = this.Compilation.GetTypeByMetadataName(AttributeNamespace + ".ExtendsAttribute");

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

            this.typeInfoByName = this.Assembly.DefinedTypes.Where(v => RepositoryNamespaceName.Equals(v.Namespace)).ToDictionary(v => v.Name);

            this.Repository = new Repository();

            this.CreateDomains();

            this.CreateUnits();
            this.CreateTypes();
            this.CreateHierarchy();
            this.CreateRecords();
            this.CreateMembers();
            this.CreateFields();

            this.FromReflection();

            this.LinkImplementations();

            this.CreateInheritedProperties();
            this.CreateReverseProperties();
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
            var domain = this.Repository.Objects.OfType<Domain>().First();
            var objects = this.Repository.Objects;

            _ = new Unit(objects, UnitIds.Binary, UnitNames.Binary, domain);
            _ = new Unit(objects, UnitIds.Boolean, UnitNames.Boolean, domain);
            _ = new Unit(objects, UnitIds.DateTime, UnitNames.DateTime, domain);
            _ = new Unit(objects, UnitIds.Decimal, UnitNames.Decimal, domain);
            _ = new Unit(objects, UnitIds.Float, UnitNames.Float, domain);
            _ = new Unit(objects, UnitIds.Integer, UnitNames.Integer, domain);
            _ = new Unit(objects, UnitIds.String, UnitNames.String, domain);
            _ = new Unit(objects, UnitIds.Unique, UnitNames.Unique, domain);
        }
        private void CreateDomains()
        {
            try
            {
                var parentNameByChildName = new Dictionary<string, string>();

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

                            var domain = new Domain(this.Repository.Objects, id, structureModel.Name, directoryInfo);

                            var extendsAttribute = structureModel.GetAttributes()
                                .FirstOrDefault(v => v.AttributeClass.Name.Equals("ExtendsAttribute"));
                            var parent = (string)extendsAttribute?.ConstructorArguments.First().Value;

                            if (!string.IsNullOrEmpty(parent))
                            {
                                parentNameByChildName.Add(domain.Name, parent);
                            }
                        }
                    }
                }

                foreach (var childName in parentNameByChildName.Keys)
                {
                    var parentName = parentNameByChildName[childName];
                    var child = this.Repository.Objects.OfType<Domain>().First(v => v.Name == childName);
                    var parent = this.Repository.Objects.OfType<Domain>().First(v => v.Name == parentName);
                    child.Base = parent;
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

                    if (idAttribute != null && idAttribute.ApplicationSyntaxReference?.SyntaxTree == syntaxTree)
                    {
                        var id = Guid.Parse((string)idAttribute.ConstructorArguments.First().Value);
                        var domain = this.Repository.Objects.OfType<Domain>().First(v => v.DirectoryInfo.Contains(fileInfo));
                        var symbol = semanticModel.GetDeclaredSymbol(interfaceDeclaration);
                        var interfaceSingularName = symbol.Name;

                        var @interface = new Interface(this.inflector, this.Repository.Objects, id, interfaceSingularName, domain);
                        var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                        @interface.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;
                    }
                }

                foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    var typeModel = (ITypeSymbol)semanticModel.GetDeclaredSymbol(classDeclaration);
                    var idAttribute = typeModel.GetAttributes().FirstOrDefault(v => v.AttributeClass.Name.Equals("IdAttribute"));

                    if (idAttribute != null && idAttribute.ApplicationSyntaxReference?.SyntaxTree == syntaxTree)
                    {
                        var id = Guid.Parse((string)idAttribute.ConstructorArguments.First().Value);
                        var domain = this.Repository.Objects.OfType<Domain>().FirstOrDefault(v => v.DirectoryInfo.Contains(fileInfo));
                        var symbol = semanticModel.GetDeclaredSymbol(classDeclaration);
                        var classSingularName = symbol.Name;

                        var @class = new Class(this.inflector, this.Repository.Objects, id, classSingularName, domain);
                        var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                        @class.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;
                    }
                }
            }
        }

        private void CreateHierarchy()
        {
            var definedTypeBySingularName = this.Assembly.DefinedTypes.Where(v => RepositoryNamespaceName.Equals(v.Namespace)).ToDictionary(v => v.Name);

            var composites = this.Repository.Objects.OfType<Composite>();

            foreach (var composite in composites)
            {
                var definedType = definedTypeBySingularName[composite.SingularName];
                var allInterfaces = definedType.GetInterfaces();
                foreach (var definedImplementedInterface in allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces())))
                {
                    var implementedInterface = this.Repository.Objects.OfType<Interface>().FirstOrDefault(v => v.SingularName == definedImplementedInterface.Name);

                    if (implementedInterface == null)
                    {
                        throw new Exception("Can not find implemented interface " + definedImplementedInterface.Name + " on " + composite.SingularName);
                    }

                    composite.ImplementedInterfaces.Add(implementedInterface);
                }
            }

            foreach (var composite in composites)
            {
                composite.Subtypes = composites.Where(v => v.Interfaces.Contains(composite)).ToArray();
            }
        }

        private void CreateRecords()
        {
            foreach (var syntaxTree in this.DocumentBySyntaxTree.Keys)
            {
                var root = syntaxTree.GetRoot();
                var semanticModel = this.SemanticModelBySyntaxTree[syntaxTree];

                foreach (var recordDeclaration in root.DescendantNodes().OfType<RecordDeclarationSyntax>())
                {
                    var symbol = semanticModel.GetDeclaredSymbol(recordDeclaration);
                    if (RepositoryNamespaceName.Equals(symbol.ContainingNamespace.ToDisplayString()))
                    {
                        var recordName = symbol.Name;
                        var record = this.Repository.Objects.OfType<Record>().FirstOrDefault(v => v.Name == recordName) ?? new Record(this.Repository.Objects, recordName);
                        var xmlDoc = symbol.GetDocumentationCommentXml(null, true);
                        record.XmlDoc = !string.IsNullOrWhiteSpace(xmlDoc) ? new XmlDoc(xmlDoc) : null;
                    }
                }
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
                var domain = this.Repository.Objects.OfType<Domain>().FirstOrDefault(v => v.DirectoryInfo.Contains(fileInfo));

                if (domain != null)
                {
                    foreach (var typeDeclaration in root.DescendantNodes().Where(v => v is InterfaceDeclarationSyntax or ClassDeclarationSyntax))
                    {
                        var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration);
                        var typeName = typeSymbol.Name;

                        var type = this.Repository.Objects.OfType<ObjectType>().FirstOrDefault(v => v.SingularName == typeName);

                        if (type != null)
                        {
                            var composite = (Composite)type;
                            foreach (var propertyDeclaration in typeDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>())
                            {
                                _ = new Property(this.inflector, this.Repository.Objects, domain, semanticModel, composite, propertyDeclaration);
                            }

                            foreach (var methodDeclaration in typeDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>())
                            {
                                _ = new Method(this.inflector, this.Repository.Objects, domain, semanticModel, composite, methodDeclaration);
                            }
                        }
                    }
                }
            }
        }

        private void CreateFields()
        {
            foreach (var syntaxTree in this.DocumentBySyntaxTree.Keys)
            {
                var root = syntaxTree.GetRoot();
                var semanticModel = this.SemanticModelBySyntaxTree[syntaxTree];

                foreach (var recordDeclaration in root.DescendantNodes().OfType<RecordDeclarationSyntax>())
                {
                    var symbol = semanticModel.GetDeclaredSymbol(recordDeclaration);
                    var recordName = symbol.Name;

                    var record = this.Repository.Objects.OfType<Record>().FirstOrDefault(v => v.Name == recordName);

                    if (record != null)
                    {
                        foreach (var propertyDeclaration in recordDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>())
                        {
                            _ = new Field(this.Repository.Objects, semanticModel, record, propertyDeclaration);
                        }
                    }
                }
            }
        }

        private void FromReflection()
        {
            foreach (var composite in this.Repository.Objects.OfType<Composite>())
            {
                var typeInfo = this.typeInfoByName[composite.SingularName];

                // Type attributes
                {
                    foreach (var group in typeInfo.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType()))
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
                    var reflectedProperty = typeInfo.GetProperty(property.RoleName);
                    if (reflectedProperty == null)
                    {
                        this.HasErrors = true;
                        Logger.Error($"{typeInfo.Name}.{property.RoleName} should be public");
                        continue;
                    }

                    var propertyAttributesByTypeName = reflectedProperty.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType());

                    var reflectedPropertyType = reflectedProperty.PropertyType;
                    var typeName = this.GetTypeName(reflectedPropertyType);
                    property.ObjectType = this.Repository.Objects.OfType<ObjectType>().First(v => v.SingularName == typeName);

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
                            if (!(property.Multiplicity is Multiplicity.OneToMany or Multiplicity.ManyToMany))
                            {
                                this.HasErrors = true;
                                Logger.Error($"{typeInfo.Name}.{property.RoleName} should be many");
                            }
                        }
                        else if (property.Multiplicity is Multiplicity.OneToMany or Multiplicity.ManyToMany)
                        {
                            this.HasErrors = true;
                            Logger.Error($"{typeInfo.Name}.{property.RoleName} should be one");
                        }
                    }
                }

                foreach (var method in composite.Methods)
                {
                    var reflectedMethod = typeInfo.GetMethod(method.Name);
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

            foreach (var record in this.Repository.Objects.OfType<Record>())
            {
                var typeInfo = this.typeInfoByName[record.Name];

                // Type attributes
                {
                    foreach (var group in typeInfo.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType()))
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
                            record.AttributesByName[typeName] = group.ToArray();
                        }
                        else
                        {
                            record.AttributeByName[typeName] = group.First();
                        }
                    }
                }

                // Field attributes
                foreach (var property in record.Fields)
                {
                    var reflectedProperty = typeInfo.GetProperty(property.Name);
                    if (reflectedProperty == null)
                    {
                        this.HasErrors = true;
                        Logger.Error($"{typeInfo.Name}.{property.Name} should be public");
                        continue;
                    }

                    var propertyAttributesByTypeName = reflectedProperty.GetCustomAttributes(false).Cast<Attribute>().GroupBy(v => v.GetType());

                    var reflectedPropertyType = reflectedProperty.PropertyType;
                    var typeName = this.GetTypeName(reflectedPropertyType);

                    property.Type = (FieldObjectType)this.Repository.Objects.OfType<Record>().FirstOrDefault(v => v.Name == typeName) ?? this.Repository.Objects.OfType<ObjectType>().First(v => v.SingularName == typeName);
                    property.IsMany = typeName.EndsWith("[]");

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
                }
            }
        }

        private void LinkImplementations()
        {
            foreach (var @class in this.Repository.Objects.OfType<Class>())
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

        private string GetTypeName(Type type)
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
            foreach (var @interface in this.Repository.Objects.OfType<Interface>())
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
            foreach (var composite in this.Repository.Objects.OfType<Composite>())
            {
                foreach (var property in composite.DefinedProperties)
                {
                    var reverseType = property.ObjectType;
                    var reverseComposite = reverseType as Composite;
                    reverseComposite?.DefinedReverseProperties.Add(property);
                }
            }

            foreach (var composite in this.Repository.Objects.OfType<Composite>())
            {
                foreach (var supertype in composite.Interfaces)
                {
                    foreach (var property in supertype.DefinedReverseProperties)
                    {
                        composite.InheritedReverseProperties.Add(property);
                    }
                }
            }
        }
    }
}
