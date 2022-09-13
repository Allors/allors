// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using Inflector;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class Method
    {
        public Method(Inflector inflector, SemanticModel semanticModel, Composite composite, MethodDeclarationSyntax methodDeclaration)
        {
            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();

            this.DefiningType = composite;

            var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration);
            this.Name = methodSymbol.Name;

            var xmlDocString = methodSymbol.GetDocumentationCommentXml(null, true);
            this.XmlDoc = !string.IsNullOrWhiteSpace(xmlDocString) ? new XmlDoc(xmlDocString) : null;

            composite.MethodByName.Add(this.Name, this);

            //var input = methodDeclaration.ParameterList;
            //var input2 = methodDeclaration.TypeParameterList;

            //var outputType = methodDeclaration.ReturnType;
            //if (outputType is not PredefinedTypeSyntax)
            //{
            //    var outputSymbol = semanticModel.GetDeclaredSymbol(outputType);
                
            //}
        }

        public string Name { get; }

        public string[] WorkspaceNames
        {
            get
            {
                dynamic attribute = this.AttributeByName.Get("Workspace");
                return attribute?.Names ?? Array.Empty<string>();
            }
        }

        public XmlDoc XmlDoc { get; set; }

        public Method DefiningMethod { get; set; }

        public Type DefiningType { get; set; }

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }

        public Record Input { get; set; }

        public Record Output { get; set; }

        public override string ToString() => $"{this.DefiningType.SingularName}.{this.Name}()";
    }
}
