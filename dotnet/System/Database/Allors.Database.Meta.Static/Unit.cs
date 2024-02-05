// <copyright file="Unit.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using Text;

public abstract class Unit : IStaticUnit, IObjectType, IMetaIdentifiableObject
{
    protected Unit(IStaticMetaPopulation metaPopulation, Guid id, string tag, string singularName, string assignedPluralName)
    {
        this.Attributes = new MetaExtension();
        this.MetaPopulation = metaPopulation;
        this.Id = id;
        this.Tag = id.Tag();
        this.SingularName = singularName;
        this.AssignedPluralName = !string.IsNullOrEmpty(assignedPluralName) ? assignedPluralName : null;
        this.PluralName = this.AssignedPluralName != null ? this.AssignedPluralName : Pluralizer.Pluralize(this.SingularName);
        this.Tag = tag;

        metaPopulation.OnCreated(this);
    }

    public dynamic Attributes { get; }

    IMetaPopulation IMetaIdentifiableObject.MetaPopulation => this.MetaPopulation;

    public IStaticMetaPopulation MetaPopulation { get; }

    public Guid Id { get; }

    public string Tag { get; set; }

    public Type BoundType { get; set; }

    public string Name => this.SingularName;

    public string SingularName { get; }

    public string AssignedPluralName { get; }

    public string PluralName { get; }

    public bool IsUnit => this is IUnit;

    public bool IsComposite => this is IComposite;

    public bool IsInterface => this is IInterface;

    public bool IsClass => this is IClass;

    public override bool Equals(object other) => this.Id.Equals((other as IMetaIdentifiableObject)?.Id);

    public override int GetHashCode() => this.Id.GetHashCode();

    public int CompareTo(IObjectType other)
    {
        return this.Id.CompareTo(other?.Id);
    }

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(this.SingularName))
        {
            return this.SingularName;
        }

        return this.Tag;
    }

    void IStaticMetaIdentifiableObject.Validate(ValidationLog validationLog)
    {
        this.ValidateObjectType(validationLog);
    }
    
    public bool IsBinary => this.Tag == UnitTags.Binary;

    public bool IsBoolean => this.Tag == UnitTags.Boolean;

    public bool IsDateTime => this.Tag == UnitTags.DateTime;

    public bool IsDecimal => this.Tag == UnitTags.Decimal;

    public bool IsFloat => this.Tag == UnitTags.Float;

    public bool IsInteger => this.Tag == UnitTags.Integer;

    public bool IsString => this.Tag == UnitTags.String;

    public bool IsUnique => this.Tag == UnitTags.Unique;

    public IEnumerable<string> WorkspaceNames => this.MetaPopulation.WorkspaceNames;
}
