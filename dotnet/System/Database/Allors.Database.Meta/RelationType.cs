// <copyright file="RelationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RelationType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A <see cref="RelationType"/> defines the state and behavior for
    /// a set of <see cref="AssociationType"/>s and <see cref="RoleType"/>s.
    /// </summary>
    public sealed class RelationType : IRelationType
    {
        private Multiplicity assignedMultiplicity;
        private Multiplicity multiplicity;

        private bool isDerived;
        private bool isIndexed;

        private string[] assignedWorkspaceNames;
        private string[] derivedWorkspaceNames;

        public RelationType(Composite associationTypeComposite, Guid id, Func<RelationType, AssociationType> associationTypeFactory, Func<RelationType, RoleType> roleTypeFactory, string tag = null)
        {
            this.MetaPopulation = associationTypeComposite.MetaPopulation;
            this.Id = id;
            this.Tag = tag ?? id.Tag();

            this.AssociationType = associationTypeFactory(this);
            this.AssociationType.ObjectType = associationTypeComposite;

            this.RoleType = roleTypeFactory(this);

            this.MetaPopulation.OnRelationTypeCreated(this);
        }

        public Guid Id { get; }

        public string Tag { get; }

        public string[] AssignedWorkspaceNames
        {
            get => this.assignedWorkspaceNames ?? Array.Empty<string>();

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.assignedWorkspaceNames = value;
                this.MetaPopulation.Stale();
            }
        }

        public IEnumerable<string> WorkspaceNames
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedWorkspaceNames;
            }
        }

        public MetaPopulation MetaPopulation { get; }
        IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;

        public bool IsDerived
        {
            get => this.isDerived;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.isDerived = value;
                this.MetaPopulation.Stale();
            }
        }

        public Multiplicity AssignedMultiplicity
        {
            get => this.assignedMultiplicity;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.assignedMultiplicity = value;
                this.MetaPopulation.Stale();
            }
        }

        public Multiplicity Multiplicity
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.multiplicity;
            }
        }

        public bool ExistExclusiveClasses
        {
            get
            {
                if (this.AssociationType?.ObjectType != null && this.RoleType?.ObjectType != null)
                {
                    return this.AssociationType.ObjectType.ExistExclusiveClass && this.RoleType.ObjectType is Composite roleCompositeType && roleCompositeType.ExistExclusiveClass;
                }

                return false;
            }
        }

        public bool IsIndexed
        {
            get => this.isIndexed;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.isIndexed = value;
                this.MetaPopulation.Stale();
            }
        }

        IAssociationType IRelationType.AssociationType => this.AssociationType;
        public AssociationType AssociationType { get; }

        IRoleType IRelationType.RoleType => this.RoleType;
        public RoleType RoleType { get; }

        public bool IsManyToMany => this.AssociationType.IsMany && this.RoleType.IsMany;

        public bool IsManyToOne => this.AssociationType.IsMany && !this.RoleType.IsMany;

        public bool IsOneToMany => this.AssociationType.IsOne && this.RoleType.IsMany;

        public bool IsOneToOne => this.AssociationType.IsOne && !this.RoleType.IsMany;

        public string Name => this.AssociationType.ObjectType + this.RoleType.SingularName;

        public string ReverseName => this.RoleType.SingularName + this.AssociationType.ObjectType;

        internal string ValidationName => "relation type" + this.Name;

        public override bool Equals(object other) => this.Id.Equals((other as RelationType)?.Id);

        public override int GetHashCode() => this.Id.GetHashCode();

        public int CompareTo(object other) => this.Id.CompareTo((other as RelationType)?.Id);

        public override string ToString()
        {
            try
            {
                return this.Name;
            }
            catch
            {
                return this.Tag;
            }
        }

        internal void DeriveMultiplicity()
        {
            if (this.RoleType?.ObjectType != null && this.RoleType.ObjectType.IsUnit)
            {
                this.multiplicity = Multiplicity.OneToOne;
            }
            else
            {
                this.multiplicity = this.AssignedMultiplicity;
            }
        }

        internal void DeriveWorkspaceNames() =>
            this.derivedWorkspaceNames = this.assignedWorkspaceNames != null ?
                this.assignedWorkspaceNames
                    .Intersect(this.AssociationType.ObjectType.Classes.SelectMany(v => v.WorkspaceNames))
                    .Intersect(this.RoleType.ObjectType switch
                    {
                        Unit unit => unit.WorkspaceNames,
                        Interface @interface => @interface.Classes.SelectMany(v => v.WorkspaceNames),
                        Class @class => @class.WorkspaceNames,
                        _ => Array.Empty<string>()
                    })
                    .ToArray() :
                Array.Empty<string>();

        internal void Validate(ValidationLog validationLog)
        {
            this.ValidateIdentity(validationLog);

            if (this.AssociationType != null && this.RoleType != null)
            {
                if (validationLog.ExistRelationName(this.Name))
                {
                    var message = "name of " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
                }
                else
                {
                    validationLog.AddRelationTypeName(this.Name);
                }

                if (validationLog.ExistRelationName(this.ReverseName))
                {
                    var message = "reversed name of " + this.ValidationName + " is already in use";
                    validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
                }
                else
                {
                    validationLog.AddRelationTypeName(this.ReverseName);
                }

                if (validationLog.ExistObjectTypeName(this.Name))
                {
                    var message = "name of " + this.ValidationName + " is in conflict with object type " + this.Name;
                    validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
                }

                if (validationLog.ExistObjectTypeName(this.ReverseName))
                {
                    var message = "reversed name of " + this.ValidationName + " is in conflict with object type " + this.Name;
                    validationLog.AddError(message, this, ValidationKind.Unique, "RelationType.Name");
                }
            }
            else if (this.AssociationType == null)
            {
                var message = this.ValidationName + " has no association type";
                validationLog.AddError(message, this, ValidationKind.Required, "RelationType.AssociationType");
            }
            else
            {
                var message = this.ValidationName + " has no role type";
                validationLog.AddError(message, this, ValidationKind.Required, "RelationType.RoleType");
            }

            this.AssociationType?.Validate(validationLog);

            this.RoleType?.Validate(validationLog);
        }
    }
}
