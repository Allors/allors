// <copyright file="Unit.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;

    public abstract class Unit : ObjectType, IUnit
    {
        private Type clrType;

        protected Unit(MetaPopulation metaPopulation, Guid id, string tag) : base(metaPopulation, id, tag) => metaPopulation.OnUnitCreated(this);

        public bool IsBinary => this.Tag == UnitTags.Binary;

        public bool IsBoolean => this.Tag == UnitTags.Boolean;

        public bool IsDateTime => this.Tag == UnitTags.DateTime;

        public bool IsDecimal => this.Tag == UnitTags.Decimal;

        public bool IsFloat => this.Tag == UnitTags.Float;

        public bool IsInteger => this.Tag == UnitTags.Integer;

        public bool IsString => this.Tag == UnitTags.String;

        public bool IsUnique => this.Id.Equals(UnitIds.Unique);

        public override Type ClrType => this.clrType;

        public override IEnumerable<string> WorkspaceNames => this.MetaPopulation.WorkspaceNames;

        public void Bind() =>
            this.clrType = this.Tag switch
            {
                UnitTags.Binary => typeof(byte[]),
                UnitTags.Boolean => typeof(bool),
                UnitTags.DateTime => typeof(DateTime),
                UnitTags.Decimal => typeof(decimal),
                UnitTags.Float => typeof(double),
                UnitTags.Integer => typeof(int),
                UnitTags.String => typeof(string),
                UnitTags.Unique => typeof(Guid),
                _ => this.clrType
            };
    }
}
