// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Inflector;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class Method : RepositoryObject
    {
        public Method(Inflector inflector, ISet<RepositoryObject> objects, Domain domain, SemanticModel semanticModel, Composite composite, MethodDeclarationSyntax methodDeclaration)
        {
            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();

            this.Domain = domain;
            this.DefiningType = composite;

            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);
            this.Name = methodSymbol.Name;

            var xmlDocString = methodSymbol.GetDocumentationCommentXml(null, true);
            this.XmlDoc = !string.IsNullOrWhiteSpace(xmlDocString) ? new XmlDoc(xmlDocString) : null;

            composite.MethodByName.Add(this.Name, this);

            var parameters = methodDeclaration.ParameterList.Parameters;
            if (parameters.Any())
            {
                var parameter = parameters.First();
                var inputSymbol = (IParameterSymbol)semanticModel.GetDeclaredSymbol(parameter);
                this.Input = objects.OfType<Record>().First(v => v.Name == inputSymbol.Type.Name);
            }

            var outputType = methodDeclaration.ReturnType;
            if (outputType is not PredefinedTypeSyntax)
            {
                var outputTypeInfo = semanticModel.GetTypeInfo(outputType);
                this.Output = objects.OfType<Record>().First(v => v.Name == outputTypeInfo.Type.Name);
            }

            domain.Methods.Add(this);
        }

        public Domain Domain { get; }

        public string[] WorkspaceNames
        {
            get
            {
                dynamic attribute = this.AttributeByName.Get("Workspace");
                return attribute?.Names ?? Array.Empty<string>();
            }
        }

        public string Name { get; }

        public XmlDoc XmlDoc { get; set; }

        public Method DefiningMethod { get; set; }

        public StructuralType DefiningType { get; set; }

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }

        public Record Input { get; set; }

        public Record Output { get; set; }

        public override string ToString() => $"{this.DefiningType.SingularName}.{this.Name}()";
    }
}
