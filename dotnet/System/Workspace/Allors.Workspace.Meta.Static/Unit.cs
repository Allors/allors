// <copyright file="IUnit.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using Allors.Text;

    public abstract class Unit : ObjectType
    {
        protected Unit(MetaPopulation metaPopulation, string tag, string singularName)
            : base(metaPopulation, tag, singularName, Pluralizer.Pluralize(singularName))
        {
        }

        public bool IsBinary => this.Tag == UnitTags.Binary;

        public bool IsBoolean => this.Tag == UnitTags.Boolean;

        public bool IsDateTime => this.Tag == UnitTags.DateTime;

        public bool IsDecimal => this.Tag == UnitTags.Decimal;

        public bool IsFloat => this.Tag == UnitTags.Float;

        public bool IsInteger => this.Tag == UnitTags.Integer;

        public bool IsString => this.Tag == UnitTags.String;

        public bool IsUnique => this.Tag == UnitTags.Unique;

        public override Type ClrType { get; set; }

        public void Bind() =>
            this.ClrType = this.Tag switch
            {
                UnitTags.Binary => typeof(byte[]),
                UnitTags.Boolean => typeof(bool),
                UnitTags.DateTime => typeof(DateTime),
                UnitTags.Decimal => typeof(decimal),
                UnitTags.Float => typeof(double),
                UnitTags.Integer => typeof(int),
                UnitTags.String => typeof(string),
                UnitTags.Unique => typeof(Guid),
                _ => this.ClrType,
            };
    }
}
