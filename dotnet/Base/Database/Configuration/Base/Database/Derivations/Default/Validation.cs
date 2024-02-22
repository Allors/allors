
// <copyright file="ValidationBase.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration.Derivations.Default
{
    using System.Collections.Generic;
    using System.Linq;
    using Database.Derivations;
    using Meta;

    public partial class Validation : IValidation
    {
        private readonly List<IDerivationError> errors;

        internal Validation() => this.errors = new List<IDerivationError>();

        public bool HasErrors => this.errors.Count > 0;

        public IDerivationError[] Errors => [.. this.errors];

        public void AddError(string errorCode) => this.AddError(new DerivationErrorGeneric(this, relation: null, errorCode));

        public void AddError(IDerivationError derivationError) => this.errors.Add(derivationError);

        public void AddError(IObject association, RoleType roleType, string errorCode)
        {
            var error = new DerivationErrorGeneric(this, new DerivationRelation(association, roleType), errorCode);
            this.AddError(error);
        }

        public void AddError(IObject role, AssociationType associationType, string errorCode)
        {
            var error = new DerivationErrorGeneric(this, new DerivationRelation(role, associationType), errorCode);
            this.AddError(error);
        }

        public void AssertExists(IObject association, RoleType roleType)
        {
            if (!association.Strategy.ExistRole(roleType))
            {
                this.AddError(new DerivationErrorRequired(this, association, roleType));
            }
        }

        public void AssertNotExists(IObject association, RoleType roleType)
        {
            if (association.Strategy.ExistRole(roleType))
            {
                this.AddError(new DerivationErrorNotAllowed(this, association, roleType));
            }
        }

        public void AssertNonEmptyString(IObject association, RoleType roleType)
        {
            if (association.Strategy.ExistRole(roleType) && association.Strategy.GetUnitRole(roleType).Equals(string.Empty))
            {
                this.AddError(new DerivationErrorRequired(this, association, roleType));
            }
        }

        public void AssertNonWhiteSpaceString(IObject association, RoleType roleType)
        {
            if (string.IsNullOrWhiteSpace(association.Strategy.GetUnitRole(roleType) as string))
            {
                this.AddError(new DerivationErrorRequired(this, association, roleType));
            }
        }

        public void AssertExistsNonEmptyString(IObject association, RoleType roleType)
        {
            this.AssertExists(association, roleType);
            this.AssertNonEmptyString(association, roleType);
        }

        public void AssertIsUnique(IChangeSet changeSet, IObject association, RoleType roleType)
        {
            if (changeSet.RoleTypesByAssociation.TryGetValue(association, out var changedRoleTypes) && changedRoleTypes.Contains(roleType))
            {
                var objectType = roleType.AssociationType.Composite;
                var role = association.Strategy.GetRole(roleType);

                if (role != null)
                {
                    var transaction = association.Strategy.Transaction;
                    var extent = transaction.Extent(objectType);
                    extent.Filter.AddEquals(roleType, role);
                    if (extent.Count != 1)
                    {
                        this.AddError(new DerivationErrorUnique(this, association, roleType));
                    }
                }
            }
        }

        public void AssertIsUnique(IChangeSet changeSet, IObject association, Composite objectType, params RoleType[] roleTypes)
        {
            if (changeSet.RoleTypesByAssociation.TryGetValue(association, out var changedRoleTypes) && changedRoleTypes.Intersect(roleTypes).Any())
            {
                Extent extent = null;

                foreach (var roleType in roleTypes)
                {
                    var role = association.Strategy.GetRole(roleType);
                    if (role == null)
                    {
                        continue;
                    }

                    if (extent == null)
                    {
                        var transaction = association.Strategy.Transaction;
                        extent = transaction.Extent(objectType);
                    }

                    extent.Filter.AddEquals(roleType, role);
                }

                if (extent != null && extent.Count != 1)
                {
                    this.AddError(new DerivationErrorUnique(this, association, roleTypes));
                }

            }
        }

        public void AssertAtLeastOne(IObject association, params RoleType[] roleTypes)
        {
            if (roleTypes.Any(association.Strategy.ExistRole))
            {
                return;
            }

            this.AddError(new DerivationErrorAtLeastOne(this, DerivationRelation.Create(association, roleTypes)));
        }

        public void AssertExistsAtMostOne(IObject association, params RoleType[] roleTypes)
        {
            var count = 0;
            foreach (var roleType in roleTypes)
            {
                if (association.Strategy.ExistRole(roleType))
                {
                    ++count;
                }
            }

            if (count > 1)
            {
                this.AddError(new DerivationErrorAtMostOne(this, DerivationRelation.Create(association, roleTypes)));
            }
        }

        public void AssertAreEqual(IObject association, RoleType roleType, RoleType otherRoleType)
        {
            var value = association.Strategy.GetRole(roleType);
            var otherValue = association.Strategy.GetRole(otherRoleType);

            bool equal;
            if (value == null)
            {
                equal = otherValue == null;
            }
            else
            {
                equal = value.Equals(otherValue);
            }

            if (!equal)
            {
                this.AddError(new DerivationErrorEquals(this, DerivationRelation.Create(association, roleType, otherRoleType)));
            }
        }

        public void AssertExists(IObject role, AssociationType associationType)
        {
            if (!role.Strategy.ExistAssociation(associationType))
            {
                this.AddError(new DerivationErrorRequired(this, role, associationType));
            }
        }
    }
}
