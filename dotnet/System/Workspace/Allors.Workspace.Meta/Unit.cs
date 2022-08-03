// <copyright file="IUnit.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using Text;

    public abstract class Unit : IObjectType
    {
        public MetaPopulation MetaPopulation { get; set; }

        public string Tag { get; set; }

        public string SingularName { get; set; }

        public string PluralName { get; set; }

        public Type ClrType { get; set; }

        int IComparable<IObjectType>.CompareTo(IObjectType other) => string.Compare(this.SingularName, other.SingularName, StringComparison.InvariantCulture);

        public bool IsUnit => true;

        public bool IsComposite => false;

        public bool IsInterface => false;

        public bool IsClass => false;

        public bool IsBinary => this.Tag == UnitTags.Binary;

        public bool IsBoolean => this.Tag == UnitTags.Boolean;

        public bool IsDateTime => this.Tag == UnitTags.DateTime;

        public bool IsDecimal => this.Tag == UnitTags.Decimal;

        public bool IsFloat => this.Tag == UnitTags.Float;

        public bool IsInteger => this.Tag == UnitTags.Integer;

        public bool IsString => this.Tag == UnitTags.String;

        public bool IsUnique => this.Tag == UnitTags.Unique;

        public Unit Init(string tag, string singularName)
        {
            this.Tag = tag;
            this.SingularName = singularName;
            this.PluralName = Pluralizer.Pluralize(singularName);

            return this;
        }

        public void Bind()
        {
            switch (this.Tag)
            {
                case UnitTags.Binary:
                    this.ClrType = typeof(byte[]);
                    break;

                case UnitTags.Boolean:
                    this.ClrType = typeof(bool);
                    break;

                case UnitTags.DateTime:
                    this.ClrType = typeof(DateTime);
                    break;

                case UnitTags.Decimal:
                    this.ClrType = typeof(decimal);
                    break;

                case UnitTags.Float:
                    this.ClrType = typeof(double);
                    break;

                case UnitTags.Integer:
                    this.ClrType = typeof(int);
                    break;

                case UnitTags.String:
                    this.ClrType = typeof(string);
                    break;

                case UnitTags.Unique:
                    this.ClrType = typeof(Guid);
                    break;
            }
        }
    }
}
