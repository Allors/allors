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
        }

        IObjectType IRelationEndType.ObjectType => this.ObjectType;

        public IComposite ObjectType { get; }

        public MetaPopulation MetaPopulation => this.RelationType.MetaPopulation;

        IRelationType IAssociationType.RelationType => this.RelationType;

        public RelationType RelationType { get; internal set; }

        public RoleType RoleType => this.RelationType.RoleType;

        public string SingularName { get; internal set; }

        public string PluralName { get; internal set; }

        public string Name { get; internal set; }

        public bool IsMany => this.RelationType.Multiplicity == Multiplicity.ManyToOne ||
                              this.RelationType.Multiplicity == Multiplicity.ManyToMany;

        public bool IsOne => !this.IsMany;

        public string OperandTag => this.RelationType.Tag;

        int IComparable<IRelationEndType>.CompareTo(IRelationEndType other) =>
            string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);

        public override string ToString() => $"{this.RoleType.ObjectType.SingularName}.{this.Name}";
    }
}
