// <copyright file="IObjectType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using Allors.Embedded;
using Embedded.Meta;

public abstract class ObjectType : EmbeddedObject, IMetaIdentifiableObject, IComparable<ObjectType>
{
    private readonly IEmbeddedUnitRole<string> singularName;
    private readonly IEmbeddedUnitRole<string> assignedPluralName;
    private readonly IEmbeddedUnitRole<string> pluralName;

    protected ObjectType(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
        this.MetaPopulation = metaPopulation;

        this.singularName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.MetaMeta.ObjectTypeSingularName);
        this.assignedPluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.MetaMeta.ObjectTypeAssignedPluralName);
        this.pluralName = this.EmbeddedPopulation.EmbeddedGetUnitRole<string>(this, metaPopulation.MetaMeta.ObjectTypePluralName);
    }

    public MetaPopulation MetaPopulation { get; }

    public Guid Id { get; set; }

    public string Tag { get; set; }

    public Type BoundType { get; internal set; }

    public string SingularName { get => this.singularName.Value; set => this.singularName.Value = value; }

    public string AssignedPluralName { get => this.assignedPluralName.Value; set => this.assignedPluralName.Value = value; }

    public string PluralName { get => this.pluralName.Value; set => this.pluralName.Value = value; }

    public abstract IEnumerable<string> WorkspaceNames { get; }

    public abstract bool IsUnit { get; }

    public abstract bool IsComposite { get; }

    public abstract bool IsInterface { get; }

    public abstract bool IsClass { get; }

    public override bool Equals(object obj)
    {
        if (obj is not ObjectType other)
        {
            return false;
        }

        if (this.EmbeddedPopulation != other.EmbeddedPopulation)
        {
            throw new ArgumentException("Object is from another meta population");
        }

        return this == other;
    }

    public int CompareTo(ObjectType other)
    {
        return this.Id.CompareTo(other?.Id);
    }

    public abstract void Validate(ValidationLog validationLog);

    internal string ValidationName()
    {
        if (!string.IsNullOrEmpty(this.SingularName))
        {
            return "object type " + this.SingularName;
        }

        return "object type " + this.Id;
    }

    internal void ValidateObjectType(ValidationLog validationLog)
    {
        if (!string.IsNullOrEmpty(this.SingularName))
        {
            if (this.SingularName.Length < 2)
            {
                var message = this.ValidationName() + " should have a singular name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "IObjectType.SingularName");
            }
            else
            {
                if (!char.IsLetter(this.SingularName[0]))
                {
                    var message = this.ValidationName() + "'s singular name should start with an alphabetical character";
                    validationLog.AddError(message, this, ValidationKind.Format, "IObjectType.SingularName");
                }

                for (var i = 1; i < this.SingularName.Length; i++)
                {
                    if (!char.IsLetter(this.SingularName[i]) && !char.IsDigit(this.SingularName[i]))
                    {
                        var message = this.ValidationName() + "'s singular name should only contain alphanumerical characters";
                        validationLog.AddError(message, this, ValidationKind.Format, "IObjectType.SingularName");
                        break;
                    }
                }
            }

            if (validationLog.ExistObjectTypeName(this.SingularName))
            {
                var message = "The singular name of " + this.ValidationName() + " is already in use";
                validationLog.AddError(message, this, ValidationKind.Unique, "IObjectType.SingularName");
            }
            else
            {
                validationLog.AddObjectTypeName(this.SingularName);
            }
        }
        else
        {
            validationLog.AddError(this.ValidationName() + " has no singular name", this, ValidationKind.Required, "IObjectType.SingularName");
        }

        if (!string.IsNullOrEmpty(this.PluralName))
        {
            if (this.PluralName.Length < 2)
            {
                var message = this.ValidationName() + " should have a plural name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "IObjectType.PluralName");
            }
            else
            {
                if (!char.IsLetter(this.PluralName[0]))
                {
                    var message = this.ValidationName() + "'s plural name should start with an alphabetical character";
                    validationLog.AddError(message, this, ValidationKind.Format, "IObjectType.PluralName");
                }

                for (var i = 1; i < this.PluralName.Length; i++)
                {
                    if (!char.IsLetter(this.PluralName[i]) && !char.IsDigit(this.PluralName[i]))
                    {
                        var message = this.ValidationName() + "'s plural name should only contain alphanumerical characters";
                        validationLog.AddError(message, this, ValidationKind.Format, "IObjectType.PluralName");
                        break;
                    }
                }
            }

            if (validationLog.ExistObjectTypeName(this.PluralName))
            {
                var message = "The plural name of " + this.ValidationName() + " is already in use";
                validationLog.AddError(message, this, ValidationKind.Unique, "IObjectType.PluralName");
            }
            else
            {
                validationLog.AddObjectTypeName(this.PluralName);
            }
        }
    }
}
