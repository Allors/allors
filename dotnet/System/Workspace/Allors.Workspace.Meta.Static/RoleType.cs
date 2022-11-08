// <copyright file="IRoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Allors.Text;

    /// <summary>
    ///     A <see cref="RoleType" /> defines the role side of a relation.
    ///     This is also called the 'passive' side.
    ///     RoleTypes can have composite and unit <see cref="ObjectType" />s.
    /// </summary>
    public abstract class RoleType : IRoleType
    {
        /// <summary>
        ///     The maximum size value.
        /// </summary>
        public const int MaximumSize = -1;

        protected RoleType(IObjectType objectType, string singularName = null, string pluralName = null)
        {
            this.ObjectType = objectType;
            this.SingularName = singularName ?? this.ObjectType.SingularName;
            this.PluralName = pluralName ?? Pluralizer.Pluralize(this.SingularName);
        }

        public MetaPopulation MetaPopulation => this.RelationType.MetaPopulation;

        IRelationType IRoleType.RelationType => this.RelationType;
        public RelationType RelationType { get; internal set; }

        public IAssociationType AssociationType => this.RelationType.AssociationType;

        public IObjectType ObjectType { get; }

        public string SingularName { get; }

        public string PluralName { get; }

        public string Name { get; internal set; }

        public bool IsMany => this.RelationType.Multiplicity == Multiplicity.OneToMany ||
                              this.RelationType.Multiplicity == Multiplicity.ManyToMany;

        public bool IsOne => !this.IsMany;

        public string OperandTag => this.RelationType.Tag;

        public int? Size { get; protected set; }

        public int? Precision { get; protected set; }

        public int? Scale { get; protected set; }

        public bool IsRequired { get; protected set; }

        public bool IsUnique { get; protected set; }

        public string MediaType { get; protected set; }

        int IComparable<IRelationEndType>.CompareTo(IRelationEndType other) =>
            string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);

        public override string ToString() => $"{this.Name}";

        internal void InitializeSizeScaleAndPrecision()
        {
            if (this.ObjectType is Unit unitType)
            {
                switch (unitType.Tag)
                {
                    case UnitTags.String:
                        this.Size ??= 256;
                        break;

                    case UnitTags.Binary:
                        this.Size ??= MaximumSize;
                        break;

                    case UnitTags.Decimal:
                        this.Precision ??= 19;
                        this.Scale ??= 2;
                        break;
                }
            }
        }
    }
}
