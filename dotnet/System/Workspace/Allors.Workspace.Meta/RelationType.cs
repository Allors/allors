// <copyright file="IRelationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Workspace.Meta
{
    /// <summary>
    /// A relation type defines the state and behavior for
    /// a set of association types and role types.
    /// </summary>
    public sealed class RelationType : IMetaObject
    {
        // Class
        public RelationType(string tag, AssociationType associationType, IComposite associationObjectType, RoleType roleType, IObjectType roleObjectType, Multiplicity multiplicity = Multiplicity.ManyToOne)
        {
            this.Tag = tag;
            this.AssociationType = associationType;
            this.AssociationType.RelationType = this;
            this.AssociationType.ObjectType = associationObjectType;
            this.RoleType = roleType;
            this.RoleType.RelationType = this;
            this.RoleType.ObjectType = roleObjectType;
            this.Multiplicity = this.RoleType.ObjectType.IsUnit ? Multiplicity.OneToOne : multiplicity;
        }

        public AssociationType AssociationType { get; }
        public RoleType RoleType { get; }

        public string Tag { get; }
        public Multiplicity Multiplicity { get; }
        public bool IsDerived { get; set; }

        public MetaPopulation MetaPopulation { get; }

        public override string ToString() => $"{this.AssociationType.ObjectType.SingularName}{this.RoleType.Name}";

        public void Init(bool isDerived = false)
        {
            this.IsDerived = isDerived;

            ((AssociationType)this.AssociationType).Init();
        }
    }
}
