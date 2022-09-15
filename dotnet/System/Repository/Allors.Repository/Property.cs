// <copyright file="Property.cs" company="Allors bvba">
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
    using Text;

    public class Property : RepositoryObject
    {
        private readonly Inflector inflector;

        public Property(Inflector inflector, ISet<RepositoryObject> objects, Domain domain, SemanticModel semanticModel, Composite composite, PropertyDeclarationSyntax propertyDeclaration)
        {
            this.inflector = inflector;

            this.Domain = domain;
            this.DefiningType = composite;

            var propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclaration);
            this.RoleName = propertySymbol.Name;

            var xmlDocString = propertySymbol.GetDocumentationCommentXml(null, true);
            this.XmlDoc = !string.IsNullOrWhiteSpace(xmlDocString) ? new XmlDoc(xmlDocString) : null;

            composite.Properties.Add(this);
            domain.Properties.Add(this);

            objects.Add(this);
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

        public XmlDoc XmlDoc { get; set; }

        public Composite DefiningType { get; }

        public ObjectType ObjectType { get; set; }

        public Property DefiningProperty { get; set; }

        public Multiplicity Multiplicity
        {
            get
            {
                if (this.ObjectType is Unit)
                {
                    return Multiplicity.OneToOne;
                }

                dynamic attribute = this.AttributeByName.Get("Multiplicity");
                if (attribute == null)
                {
                    return Multiplicity.ManyToOne;
                }

                return (Multiplicity)(int)attribute.Value;
            }
        }

        public string RoleName { get; }

        public string RoleSingularName
        {
            get
            {
                if (this.Multiplicity is Multiplicity.OneToOne or Multiplicity.ManyToOne)
                {
                    return this.RoleName;
                }

                dynamic attribute = this.AttributeByName.Get("Singular");
                return attribute != null ? attribute.Value : this.inflector.Singularize(this.RoleName);
            }
        }

        public string AssignedRoleSingularName => !this.RoleSingularName.Equals(this.ObjectType.SingularName) ? this.RoleSingularName : null;

        public string RolePluralName
        {
            get
            {
                if (this.Multiplicity is Multiplicity.OneToMany or Multiplicity.ManyToMany)
                {
                    return this.RoleName;
                }

                dynamic attribute = this.AttributeByName.Get("Plural");
                return attribute != null ? attribute.Value : this.inflector.Pluralize(this.RoleName);
            }
        }

        public string AssignedRolePluralName => !Pluralizer.Pluralize(this.RoleSingularName).Equals(this.RolePluralName) ? this.RolePluralName : null;

        public string AssociationName
        {
            get
            {
                if (this.Multiplicity is Multiplicity.ManyToOne or Multiplicity.ManyToMany)
                {
                    return this.DefiningType.PluralName + "Where" + this.RoleSingularName;
                }

                return this.DefiningType.SingularName + "Where" + this.RoleSingularName;
            }
        }

        public override string ToString() => $"{this.DefiningType.SingularName}.{this.RoleSingularName}";
    }
}
