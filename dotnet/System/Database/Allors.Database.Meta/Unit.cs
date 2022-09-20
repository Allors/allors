// <copyright file="Unit.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;

public abstract class Unit : ObjectType, IUnit
{
    protected Unit(MetaPopulation metaPopulation, Guid id)
        : base(metaPopulation, id)
        => metaPopulation.OnUnitCreated(this);

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
