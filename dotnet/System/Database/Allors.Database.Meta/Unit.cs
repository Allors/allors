// <copyright file="Unit.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using Embedded;
using Embedded.Meta;

public sealed class Unit : ObjectType
{
    public Unit(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
        this.MetaPopulation.OnCreated(this);
    }

    public override bool IsUnit => true;

    public override bool IsComposite => false;

    public override bool IsInterface => false;

    public override bool IsClass => false;

    public static implicit operator Unit(UnitIndex index) => index?.Unit;

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(this.SingularName))
        {
            return this.SingularName;
        }

        return this.Tag;
    }

    public override void Validate(ValidationLog validationLog)
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

    public override IEnumerable<string> WorkspaceNames => this.MetaPopulation.WorkspaceNames;
}
