// <copyright file="State.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using Allors.Shared.Ranges;
    using Meta;

    public abstract class State
    {
        protected State(Record record)
        {
            this.Record = record;
            this.IsPushed = false;
        }

        public long Version => this.Record?.Version ?? Allors.Version.WorkspaceInitial;

        public bool CanRead(IRoleType roleType)
        {
            if (!this.ExistRecord)
            {
                return true;
            }

            if (this.IsVersionInitial)
            {
                // TODO: Security
                return true;
            }

            var permission = this.Session.Connection.GetPermission(this.Class, roleType, Operations.Read);
            return this.Record.IsPermitted(permission);
        }

        public bool CanWrite(IRoleType roleType)
        {
            if (this.IsVersionInitial)
            {
                return !this.IsPushed;
            }

            if (this.IsPushed)
            {
                return false;
            }

            if (!this.ExistRecord)
            {
                return true;
            }

            var permission = this.Session.Connection.GetPermission(this.Class, roleType, Operations.Write);
            return this.Record.IsPermitted(permission);
        }

        public bool CanExecute(IMethodType methodType)
        {
            if (!this.ExistRecord)
            {
                return true;
            }

            if (this.IsVersionInitial)
            {
                // TODO: Security
                return true;
            }

            var permission = this.Session.Connection.GetPermission(this.Class, methodType, Operations.Execute);
            return this.Record.IsPermitted(permission);
        }

        public abstract Strategy Strategy { get; }

        private bool IsVersionInitial => this.Version == Allors.Version.WorkspaceInitial.Value;

        protected IEnumerable<IRoleType> RoleTypes => this.Class.RoleTypes;

        protected bool ExistRecord => this.Record != null;

        protected Record Record { get; private set; }

        private bool IsPushed { get; set; }

        protected bool HasChanges => this.Record == null || this.ChangedRoleByRelationType?.Count > 0;

        public void OnPushed() => this.IsPushed = true;

        public void OnPulled(IPullResultInternals pull)
        {
            var newRecord = this.Session.Connection.GetRecord(this.Id);

            if (!this.IsPushed)
            {
                if (!this.CanMerge(newRecord))
                {
                    pull.AddMergeError(this.Strategy.Object);
                    return;
                }
            }
            else
            {
                this.Reset();
                this.IsPushed = false;
            }

            this.Record = newRecord;
        }

        public Dictionary<IRelationType, object> ChangedRoleByRelationType { get; private set; }

        public object GetUnitRole(IRoleType roleType)
        {
            if (this.ChangedRoleByRelationType != null && this.ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var role))
            {
                return role;
            }

            return this.Record?.GetRole(roleType);
        }

        public void SetUnitRole(IRoleType roleType, object role) => this.SetChangedRole(roleType, role);

        public Strategy GetCompositeRole(IRoleType roleType)
        {
            if (this.ChangedRoleByRelationType != null && this.ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return (Strategy)changedRole;
            }

            var role = this.Record?.GetRole(roleType);

            if (role == null)
            {
                return null;
            }

            var strategy = this.Session.GetStrategy((long)role);
            this.AssertStrategy(strategy);
            return strategy;
        }

        public void SetCompositeRole(IRoleType roleType, Strategy role)
        {
            if (this.SameCompositeRole(roleType, role))
            {
                return;
            }

            var associationType = roleType.AssociationType;
            if (associationType.IsOne && role != null)
            {
                var previousAssociation = this.Session.GetCompositeAssociation(role, associationType);
                this.SetChangedRole(roleType, role);
                if (associationType.IsOne && previousAssociation != null)
                {
                    // OneToOne
                    previousAssociation.SetRole(roleType, null);
                }
            }
            else
            {
                this.SetChangedRole(roleType, role);
            }
        }

        public RefRange<Strategy> GetCompositesRole(IRoleType roleType)
        {
            if (this.ChangedRoleByRelationType != null && this.ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return RefRange<Strategy>.Ensure(changedRole);
            }

            var role = ValueRange<long>.Ensure(this.Record?.GetRole(roleType));

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

        public void AddCompositesRole(IRoleType roleType, Strategy roleToAdd)
        {
            var associationType = roleType.AssociationType;
            var previousAssociation = this.Session.GetCompositeAssociation(roleToAdd, associationType);

            var role = this.GetCompositesRole(roleType);

            if (role.Contains(roleToAdd))
            {
                return;
            }

            role = role.Add(roleToAdd);
            this.SetChangedRole(roleType, role);

            if (associationType.IsMany)
            {
                return;
            }

            // OneToMany
            previousAssociation?.SetRole(roleType, null);
        }

        public void RemoveCompositesRole(IRoleType roleType, Strategy roleToRemove)
        {
            var role = this.GetCompositesRole(roleType);

            if (!role.Contains(roleToRemove))
            {
                return;
            }

            role = role.Remove(roleToRemove);
            this.SetChangedRole(roleType, role);
        }

        public void SetCompositesRole(IRoleType roleType, RefRange<Strategy> role)
        {
            if (this.SameCompositesRole(roleType, role))
            {
                return;
            }

            var previousRole = this.GetCompositesRoleIfInstantiated(roleType);

            this.SetChangedRole(roleType, role);

            var associationType = roleType.AssociationType;
            if (associationType.IsMany)
            {
                return;
            }

            // OneToMany
            foreach (var addedRole in role.Except(previousRole))
            {
                var previousAssociation = this.Session.GetCompositeAssociation(addedRole, associationType);
                previousAssociation?.SetRole(roleType, null);
            }
        }

        public bool CanMerge(Record newRecord)
        {
            if (this.ChangedRoleByRelationType == null)
            {
                return true;
            }

            foreach (var kvp in this.ChangedRoleByRelationType)
            {
                var relationType = kvp.Key;
                var roleType = relationType.RoleType;

                var original = this.Record?.GetRole(roleType);
                var newOriginal = newRecord?.GetRole(roleType);

                if (roleType.ObjectType.IsUnit)
                {
                    if (!Equals(original, newOriginal))
                    {
                        return false;
                    }
                }
                else if (roleType.IsOne)
                {
                    if (!Equals(original, newOriginal))
                    {
                        return false;
                    }
                }
                else if (!ValueRange<long>.Ensure(original).Equals(ValueRange<long>.Ensure(newOriginal)))
                {
                    return false;
                }
            }

            return true;
        }

        public bool HashChanges() => this.ChangedRoleByRelationType != null;

        public void Reset() => this.ChangedRoleByRelationType = null;

        public bool IsAssociationForRole(IRoleType roleType, Strategy forRole)
        {
            if (roleType.IsOne)
            {
                var compositeRole = this.GetCompositeRoleIfInstantiated(roleType);
                return compositeRole == forRole;
            }

            var compositesRole = this.GetCompositesRoleIfInstantiated(roleType);
            return compositesRole.Contains(forRole);
        }

        public bool HasChanged(IRoleType roleType) => this.ChangedRoleByRelationType?.ContainsKey(roleType.RelationType) ?? false;

        public void RestoreRole(IRoleType roleType) => this.ChangedRoleByRelationType?.Remove(roleType.RelationType);

        private void SetChangedRole(IRoleType roleType, object role)
        {
            this.ChangedRoleByRelationType ??= new Dictionary<IRelationType, object>();
            this.ChangedRoleByRelationType[roleType.RelationType] = role;
            this.OnChange();
        }

        private Strategy GetCompositeRoleIfInstantiated(IRoleType roleType)
        {
            if (this.ChangedRoleByRelationType != null && this.ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return (Strategy)changedRole;
            }

            var role = this.Record?.GetRole(roleType);
            return role == null ? null : this.Session.GetStrategy((long)role);
        }

        private RefRange<Strategy> GetCompositesRoleIfInstantiated(IRoleType roleType)
        {
            if (this.ChangedRoleByRelationType != null && this.ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return RefRange<Strategy>.Ensure(changedRole);
            }

            var role = ValueRange<long>.Ensure(this.Record?.GetRole(roleType));
            return role.IsEmpty ? RefRange<Strategy>.Empty : RefRange<Strategy>.Load(role.Select(this.Session.GetStrategy).Where(v => v != null));
        }

        private bool SameCompositeRole(IRoleType roleType, Strategy role)
        {
            if (this.ChangedRoleByRelationType != null && this.ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return role == changedRole;
            }

            var changedRoleId = this.Record?.GetRole(roleType);

            if (role == null)
            {
                return changedRoleId == null;
            }

            if (changedRoleId == null)
            {
                return false;
            }

            return role.Id == (long)changedRoleId;
        }

        private bool SameCompositesRole(IRoleType roleType, RefRange<Strategy> role)
        {
            if (this.ChangedRoleByRelationType != null && this.ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return role.Equals(changedRole);
            }

            var roleIds = ValueRange<long>.Ensure(this.Record?.GetRole(roleType));
            return role.IsEmpty ? roleIds.IsEmpty : role.Select(v => v.Id).SequenceEqual(roleIds);
        }

        private void AssertStrategy(Strategy strategy)
        {
            if (strategy == null)
            {
                throw new Exception("Strategy is not in Workspace.");
            }
        }

        protected void OnChange()
        {
            this.Session.PushToDatabaseTracker.OnChanged(this);
        }

        #region Proxy Properties

        protected long Id => this.Strategy.Id;

        protected IClass Class => this.Strategy.Class;

        protected Workspace Session => this.Strategy.Workspace;

        #endregion
    }
}
