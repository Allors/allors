// <copyright file="IRoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
    public abstract class RoleType : IPropertyType
    {
        /// <summary>
        ///     The maximum size value.
        /// </summary>
        public const int MaximumSize = -1;

        public MetaPopulation MetaPopulation { get; set; }
        public RelationType RelationType { get; set; }
        public AssociationType AssociationType => this.RelationType.AssociationType;
        public int? Size { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool IsRequired { get; set; }
        public bool IsUnique { get; set; }
        public string MediaType { get; set; }

        public IObjectType ObjectType { get; set; }

        public string SingularName { get; set; }
        public string PluralName { get; set; }
        public string Name { get; set; }
        public bool IsMany { get; set; }
        public bool IsOne { get; set; }

        int IComparable<IPropertyType>.CompareTo(IPropertyType other) =>
            string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);

        public string OperandTag => this.RelationType.Tag;

        public override string ToString() => $"{this.Name}";

        public void Init(string singularName = null, string pluralName = null, int? size = null, int? precision = null, int? scale = null,
            bool isRequired = false, bool isUnique = false, string mediaType = null)
        {
            this.SingularName = singularName ?? this.ObjectType.SingularName;
            this.PluralName = pluralName ?? Pluralizer.Pluralize(this.SingularName);

            this.IsMany = this.RelationType.Multiplicity == Multiplicity.OneToMany ||
                          this.RelationType.Multiplicity == Multiplicity.ManyToMany;
            this.IsOne = !this.IsMany;
            this.Name = this.IsMany ? this.PluralName : this.SingularName;

            if (this.ObjectType is Unit unitType)
            {
                switch (unitType.Tag)
                {
                    case UnitTags.String:
                        this.Size = size ?? 256;
                        break;

                    case UnitTags.Binary:
                        this.Size = size ?? MaximumSize;
                        break;

                    case UnitTags.Decimal:
                        this.Precision = precision ?? 19;
                        this.Scale = scale ?? 2;
                        break;
                }
            }

            this.IsRequired = isRequired;
            this.IsUnique = isUnique;
            this.MediaType = mediaType;
        }
    }
}
