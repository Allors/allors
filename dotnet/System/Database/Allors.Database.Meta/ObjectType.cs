// <copyright file="ObjectType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using Allors.Text;

public abstract class ObjectType : DataType, IObjectType
{
    private string pluralName;
    private string singularName;

    protected ObjectType(MetaPopulation metaPopulation, Guid id, string tag = null)
        : base(metaPopulation, id, tag)
    {
    }

    public string SingularName
    {
        get => this.singularName;

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.singularName = value;
            this.MetaPopulation.Stale();
        }
    }

    public string PluralName
    {
        get => !string.IsNullOrEmpty(this.pluralName) ? this.pluralName : Pluralizer.Pluralize(this.SingularName);

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.pluralName = value;
            this.MetaPopulation.Stale();
        }
    }

    public bool ExistAssignedPluralName =>
        !string.IsNullOrEmpty(this.PluralName) && !this.PluralName.Equals(Pluralizer.Pluralize(this.SingularName));

    public override string Name => this.SingularName;

    public bool IsUnit => this is IUnit;

    public bool IsComposite => this is IComposite;

    public bool IsInterface => this is IInterface;

    public bool IsClass => this is IClass;

    internal string ValidationName
    {
        get
        {
            if (!string.IsNullOrEmpty(this.SingularName))
            {
                return "object type " + this.SingularName;
            }

            return "object type " + this.Id;
        }
    }

    public override bool Equals(object other) => this.Id.Equals((other as IMetaIdentifiableObject)?.Id);

    public override int GetHashCode() => this.Id.GetHashCode();

    public int CompareTo(object other) => this.Id.CompareTo((other as IMetaIdentifiableObject)?.Id);

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(this.SingularName))
        {
            return this.SingularName;
        }

        return this.Tag;
    }

    internal void Validate(ValidationLog validationLog)
    {
        if (!string.IsNullOrEmpty(this.SingularName))
        {
            if (this.SingularName.Length < 2)
            {
                var message = this.ValidationName + " should have a singular name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "IObjectType.SingularName");
            }
            else
            {
                if (!char.IsLetter(this.SingularName[0]))
                {
                    var message = this.ValidationName + "'s singular name should start with an alfabetical character";
                    validationLog.AddError(message, this, ValidationKind.Format, "IObjectType.SingularName");
                }

                for (var i = 1; i < this.SingularName.Length; i++)
                {
                    if (!char.IsLetter(this.SingularName[i]) && !char.IsDigit(this.SingularName[i]))
                    {
                        var message = this.ValidationName + "'s singular name should only contain alfanumerical characters";
                        validationLog.AddError(message, this, ValidationKind.Format, "IObjectType.SingularName");
                        break;
                    }
                }
            }

            if (validationLog.ExistObjectTypeName(this.SingularName))
            {
                var message = "The singular name of " + this.ValidationName + " is already in use";
                validationLog.AddError(message, this, ValidationKind.Unique, "IObjectType.SingularName");
            }
            else
            {
                validationLog.AddObjectTypeName(this.SingularName);
            }
        }
        else
        {
            validationLog.AddError(this.ValidationName + " has no singular name", this, ValidationKind.Required, "IObjectType.SingularName");
        }

        if (!string.IsNullOrEmpty(this.PluralName))
        {
            if (this.PluralName.Length < 2)
            {
                var message = this.ValidationName + " should have a plural name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "IObjectType.PluralName");
            }
            else
            {
                if (!char.IsLetter(this.PluralName[0]))
                {
                    var message = this.ValidationName + "'s plural name should start with an alfabetical character";
                    validationLog.AddError(message, this, ValidationKind.Format, "IObjectType.PluralName");
                }

                for (var i = 1; i < this.PluralName.Length; i++)
                {
                    if (!char.IsLetter(this.PluralName[i]) && !char.IsDigit(this.PluralName[i]))
                    {
                        var message = this.ValidationName + "'s plural name should only contain alfanumerical characters";
                        validationLog.AddError(message, this, ValidationKind.Format, "IObjectType.PluralName");
                        break;
                    }
                }
            }

            if (validationLog.ExistObjectTypeName(this.PluralName))
            {
                var message = "The plural name of " + this.ValidationName + " is already in use";
                validationLog.AddError(message, this, ValidationKind.Unique, "IObjectType.PluralName");
            }
            else
            {
                validationLog.AddObjectTypeName(this.PluralName);
            }
        }
    }
}
