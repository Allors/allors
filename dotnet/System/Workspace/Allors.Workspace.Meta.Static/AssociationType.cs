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
    public abstract class AssociationType : IAssociationType
    {
        protected AssociationType(IComposite objectType)
        {
            this.ObjectType = objectType;

            const string where = "Where";

            this.SingularName = this.ObjectType.SingularName + where + this.RoleType.SingularName;
            this.PluralName = this.ObjectType.PluralName + where + this.RoleType.SingularName;
            this.Name = this.IsMany ? this.PluralName : this.SingularName;
        }

        IObjectType IPropertyType.ObjectType => this.ObjectType;

        public IComposite ObjectType { get; }

        public MetaPopulation MetaPopulation => this.RelationType.MetaPopulation;

        public RelationType RelationType { get; internal set; }

        public RoleType RoleType => this.RelationType.RoleType;

        public string SingularName { get; }

        public string PluralName { get; }

        public string Name { get; }

        public bool IsMany => this.RelationType.Multiplicity == Multiplicity.ManyToOne ||
                              this.RelationType.Multiplicity == Multiplicity.ManyToMany;

        public bool IsOne => !this.IsMany;

        public string OperandTag => this.RelationType.Tag;

        int IComparable<IPropertyType>.CompareTo(IPropertyType other) =>
            string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);

        public override string ToString() => $"{this.RoleType.ObjectType.SingularName}.{this.Name}";
    }
}
