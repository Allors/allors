// <copyright file="RecordBasedOriginState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using Shared.Ranges;

    public abstract class RecordBasedOriginState
    {
        public abstract Strategy Strategy { get; }

        protected abstract IEnumerable<RoleType> RoleTypes { get; }

        protected abstract IRecord Record { get; }

        protected IRecord PreviousRecord { get; set; }


        public object GetUnitRole(RoleType roleType) => this.Record?.GetRole(roleType);

        public Strategy GetCompositeRole(RoleType roleType)
        {
            var role = this.Record?.GetRole(roleType);

            if (role == null)
            {
                return null;
            }

            var strategy = this.Session.GetStrategy((long)role);
            this.AssertStrategy(strategy);
            return strategy;
        }

        public RefRange<Strategy> GetCompositesRole(RoleType roleType)
        {
            var role = (ValueRange<long>)(this.Record?.GetRole(roleType) ?? ValueRange<long>.Empty);

            if (role.IsEmpty)
            {
                return RefRange<Strategy>.Empty;
            }

            return RefRange<Strategy>.Load(role.Select(v =>
            {
                var strategy = this.Session.GetStrategy(v);
                this.AssertStrategy(strategy);
                return strategy;
            }));
        }

        public bool IsAssociationForRole(RoleType roleType, Strategy forRole)
        {
            if (roleType.IsOne)
            {
                var compositeRole = this.GetCompositeRoleIfInstantiated(roleType);
                return compositeRole == forRole;
            }

            var compositesRole = this.GetCompositesRoleIfInstantiated(roleType);
            return compositesRole.Contains(forRole);
        }


        private Strategy GetCompositeRoleIfInstantiated(RoleType roleType)
        {
            var role = this.Record?.GetRole(roleType);
            return role == null ? null : this.Session.GetStrategy((long)role);
        }

        private RefRange<Strategy> GetCompositesRoleIfInstantiated(RoleType roleType)
        {
            var role = (ValueRange<long>)(this.Record?.GetRole(roleType) ?? ValueRange<long>.Empty);
            return role.IsEmpty ? RefRange<Strategy>.Empty : RefRange<Strategy>.Load(role.Select(v => this.Session.GetStrategy(v)).Where(v => v != null));
        }

        private void AssertStrategy(Strategy strategy)
        {
            if (strategy == null)
            {
                throw new Exception("Strategy is not in Session.");
            }
        }

        #region Proxy Properties

        protected long Id => this.Strategy.Id;

        protected Class Class => this.Strategy.Class;

        protected Workspace Session => this.Strategy.Session;

        protected WorkspaceConnection Workspace => this.Session.WorkspaceConnection;

        #endregion
    }
}
