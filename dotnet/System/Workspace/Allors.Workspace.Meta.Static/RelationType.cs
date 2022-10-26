// <copyright file="IRelationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Workspace.Meta
{
    using System.Drawing;
    using Allors.Text;

    /// <summary>
    ///     A relation type defines the state and behavior for
    ///     a set of association types and role types.
    /// </summary>
    public sealed class RelationType : MetaIdentifiableObject
    {
        // Class
        public RelationType(MetaPopulation metaPopulation, string tag, AssociationType associationType, RoleType roleType, Multiplicity multiplicity = Multiplicity.ManyToOne)
        : base(metaPopulation, tag)
        {
            this.AssociationType = associationType;
            this.AssociationType.RelationType = this;
            this.RoleType = roleType;
            this.RoleType.RelationType = this;

            this.Multiplicity = this.RoleType.ObjectType.IsUnit ? Multiplicity.OneToOne : multiplicity;

            const string where = "Where";

            this.AssociationType.SingularName = this.AssociationType.ObjectType.SingularName + where + this.RoleType.SingularName;
            this.AssociationType.PluralName = this.AssociationType.ObjectType.PluralName + where + this.RoleType.SingularName;
            this.AssociationType.Name = this.AssociationType.IsMany ? this.AssociationType.PluralName : this.AssociationType.SingularName;

            this.RoleType.Name = this.RoleType.IsMany ? this.RoleType.PluralName : this.RoleType.SingularName;
        }

        public AssociationType AssociationType { get; }

        public RoleType RoleType { get; }

        public Multiplicity Multiplicity { get; }

        public bool IsDerived { get; set; }

        public override string ToString() => $"{this.AssociationType.ObjectType.SingularName}{this.RoleType.Name}";
    }
}
