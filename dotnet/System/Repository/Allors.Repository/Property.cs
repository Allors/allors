// <copyright file="Property.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain;

using System.Collections.Generic;
using Inflector;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Allors.Text;

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

        composite.Properties.Add(this);
        domain.Properties.Add(this);

        objects.Add(this);
    }

    public Domain Domain { get; }

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
                if (this.IsArray)
                {
                    return Multiplicity.ManyToMany;
                }
                else
                {
                    return Multiplicity.ManyToOne;
                }
            }

            return (Multiplicity)(int)attribute.Value;
        }
    }

    public bool IsRoleOne => !this.IsRoleMany;

    public bool IsRoleMany
    {
        get
        {
            switch (this.Multiplicity)
            {
                case Multiplicity.OneToMany:
                case Multiplicity.ManyToMany:
                    return true;

                default:
                    return false;
            }
        }
    }

    public bool IsAssociationOne => !this.IsAssociationMany;

    public bool IsAssociationMany
    {
        get
        {
            switch (this.Multiplicity)
            {
                case Multiplicity.ManyToOne:
                case Multiplicity.ManyToMany:
                    return true;

                default:
                    return false;
            }
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

    public string AssignedRolePluralName =>
        !Pluralizer.Pluralize(this.RoleSingularName).Equals(this.RolePluralName) ? this.RolePluralName : null;

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

    public bool IsArray { get; set; }

    public override string ToString() => $"{this.DefiningType.SingularName}.{this.RoleSingularName}";
}
