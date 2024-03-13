// <copyright file="IRoleType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using Allors.Text;

    /// <summary>
    ///     A <see cref="RoleType" /> defines the role side of a relation.
    ///     This is also called the 'passive' side.
    ///     RoleTypes can have composite and unit <see cref="ObjectType" />s.
    /// </summary>
    public sealed class RoleType : MetaIdentifiableObject, IRoleType
    {
        /// <summary>
        ///     The maximum size value.
        /// </summary>
        public const int MaximumSize = -1;

        private const string Where = "Where";

        public RoleType(MetaPopulation metaPopulation, string tag, string singularName, string pluralName, IObjectType objectType, IComposite associationObjectType, Multiplicity multiplicity = Multiplicity.ManyToOne)
            : base(metaPopulation, tag)
        {

            this.ObjectType = objectType;
            this.SingularName = singularName ?? this.ObjectType.SingularName;
            this.PluralName = pluralName ?? Pluralizer.Pluralize(this.SingularName);
            this.Multiplicity = this.ObjectType.IsUnit ? Multiplicity.OneToOne : multiplicity;

            this.AssociationType = new AssociationType(associationObjectType) { RoleType = this };
            this.AssociationType.SingularName = this.AssociationType.ObjectType.SingularName + Where + this.SingularName;
            this.AssociationType.PluralName = this.AssociationType.ObjectType.PluralName + Where + this.SingularName;
            this.AssociationType.Name = this.AssociationType.IsMany ? this.AssociationType.PluralName : this.AssociationType.SingularName;

            this.Name = this.IsMany ? this.PluralName : this.SingularName;
        }

        IAssociationType IRoleType.AssociationType => this.AssociationType;
        public AssociationType AssociationType { get; internal set; }

        public IObjectType ObjectType { get; }

        public string SingularName { get; }

        public string PluralName { get; }

        public string Name { get; internal set; }

        public Multiplicity Multiplicity { get; }

        public bool IsMany => this.Multiplicity == Multiplicity.OneToMany ||
                              this.Multiplicity == Multiplicity.ManyToMany;

        public bool IsOne => !this.IsMany;

        public string OperandTag => this.Tag;

        public int? Size { get; set; }

        public int? Precision { get; set; }

        public int? Scale { get; set; }

        public bool IsDerived { get; set; }

        public bool IsRequired { get; set; }

        public bool IsUnique { get; set; }

        public string MediaType { get; set; }

        int IComparable<IRelationEndType>.CompareTo(IRelationEndType other) => string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);

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
