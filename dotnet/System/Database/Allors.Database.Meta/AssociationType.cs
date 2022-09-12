// <copyright file="AssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An <see cref="AssociationType"/> defines the association side of a relation.
    /// This is also called the 'active', 'controlling' or 'owning' side.
    /// AssociationTypes can only have composite <see cref="ObjectType"/>s.
    /// </summary>
    public abstract class AssociationType : IAssociationType, IComparable
    {
        /// <summary>
        /// Used to create property names.
        /// </summary>
        private const string Where = "Where";

        private Composite objectType;

        protected AssociationType(RelationType relationType)
        {
            this.RelationType = relationType;
            this.MetaPopulation = this.RelationType.MetaPopulation;
            this.RelationType.MetaPopulation.OnAssociationTypeCreated(this);
        }

        IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;
        public MetaPopulation MetaPopulation { get; }

        public IEnumerable<string> WorkspaceNames => this.RelationType.WorkspaceNames;

        public string[] AssignedWorkspaceNames => this.RelationType.AssignedWorkspaceNames;

        public string Name => this.IsMany ? this.PluralName : this.SingularName;

        public string SingularName => this.objectType.SingularName + Where + this.RoleType.SingularName;

        public string SingularFullName => this.SingularName;

        public string PluralName => this.objectType.PluralName + Where + this.RoleType.SingularName;

        public string PluralFullName => this.PluralName;

        IObjectType IPropertyType.ObjectType => this.ObjectType;
        IComposite IAssociationType.ObjectType => this.ObjectType;
        public Composite ObjectType
        {
            get => this.objectType;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.objectType = value;
                this.MetaPopulation.Stale();
            }
        }

        public bool IsOne => !this.IsMany;

        public bool IsMany =>
            this.RelationType.Multiplicity switch
            {
                Multiplicity.ManyToOne => true,
                Multiplicity.ManyToMany => true,
                _ => false
            };

        IRoleType IAssociationType.RoleType => this.RoleType;
        public RoleType RoleType => this.RelationType.RoleType;

        IRelationType IAssociationType.RelationType => this.RelationType;
        public RelationType RelationType { get; }

        internal string ValidationName => "association type " + this.Name;

        public override bool Equals(object other) => this.RelationType.Id.Equals((other as AssociationType)?.RelationType.Id);

        public override int GetHashCode() => this.RelationType.Id.GetHashCode();

        public int CompareTo(object other) => this.RelationType.Id.CompareTo((other as AssociationType)?.RelationType.Id);

        public override string ToString() => $"{this.RoleType.ObjectType.Name}.{this.Name}";

        // TODO: move to extension method
        public object Get(IStrategy strategy, IComposite ofType)
        {
            var association = strategy.GetAssociation(this);

            if (ofType == null || association == null)
            {
                return association;
            }

            if (this.IsMany)
            {
                var extent = (IEnumerable<IObject>)association;
                return extent.Where(v => ofType.IsAssignableFrom(v.Strategy.Class));
            }

            return !ofType.IsAssignableFrom(((IObject)association).Strategy.Class) ? null : association;
        }

        internal void Validate(ValidationLog validationLog)
        {
            if (this.objectType == null)
            {
                var message = this.ValidationName + " has no object type";
                validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.IObjectType");
            }

            if (this.RelationType == null)
            {
                var message = this.ValidationName + " has no relation type";
                validationLog.AddError(message, this, ValidationKind.Required, "AssociationType.RelationType");
            }
        }
    }
}
