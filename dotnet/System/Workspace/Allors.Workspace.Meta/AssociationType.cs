// <copyright file="IAssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;

    /// <summary>
    ///     An association type defines the association side of a relation.
    ///     This is also called the 'active', 'controlling' or 'owning' side.
    ///     AssociationTypes can only have composite <see cref="ObjectType" />s.
    /// </summary>
    public abstract class AssociationType : IPropertyType
    {
        public MetaPopulation MetaPopulation { get; set; }
        public IComposite ObjectType { get; set; }

        public RelationType RelationType { get; set; }

        public RoleType RoleType => this.RelationType.RoleType;

        IObjectType IPropertyType.ObjectType => this.ObjectType;

        public string SingularName { get; set; }
        public string PluralName { get; set; }
        public string Name { get; set; }
        public bool IsMany { get; set; }
        public bool IsOne { get; set; }

        public string OperandTag => this.RelationType.Tag;

        int IComparable<IPropertyType>.CompareTo(IPropertyType other) =>
            string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);

        public override string ToString() => $"{this.RoleType.ObjectType.SingularName}.{this.Name}";

        public void Init()
        {
            const string where = "Where";

            this.IsMany = this.RelationType.Multiplicity == Multiplicity.ManyToOne ||
                          this.RelationType.Multiplicity == Multiplicity.ManyToMany;
            this.IsOne = !this.IsMany;
            this.SingularName = this.ObjectType.SingularName + where + this.RoleType.SingularName;
            this.PluralName = this.ObjectType.PluralName + where + this.RoleType.SingularName;
            this.Name = this.IsMany ? this.PluralName : this.SingularName;
        }
    }
}
