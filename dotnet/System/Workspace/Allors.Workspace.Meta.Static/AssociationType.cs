// <copyright file="IAssociationType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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
    public abstract class AssociationType : MetaIdentifiableObject, IAssociationType
    {
        protected AssociationType(MetaPopulation metaPopulation, string tag, IComposite objectType)
            : base(metaPopulation, tag)
        {
            this.ObjectType = objectType;
        }

        IObjectType IRelationEndType.ObjectType => this.ObjectType;

        public IComposite ObjectType { get; }

        public RoleType RoleType { get; internal set; }

        IRoleType IAssociationType.RoleType => this.RoleType;

        public string SingularName { get; internal set; }

        public string PluralName { get; internal set; }

        public string Name { get; internal set; }

        public bool IsMany => this.RoleType.Multiplicity == Multiplicity.ManyToOne ||
                              this.RoleType.Multiplicity == Multiplicity.ManyToMany;

        public bool IsOne => !this.IsMany;

        public string OperandTag => this.RoleType.Tag;

        int IComparable<IRelationEndType>.CompareTo(IRelationEndType other) =>
            string.Compare(this.Name, other.Name, StringComparison.InvariantCulture);

        public override string ToString() => $"{this.RoleType.ObjectType.SingularName}.{this.Name}";
    }
}
