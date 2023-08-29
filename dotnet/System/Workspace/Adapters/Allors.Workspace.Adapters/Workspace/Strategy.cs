// <copyright file="Strategy.cs" company="Allors bvba">
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

    public abstract class Strategy : IStrategy, IComparable<Strategy>
    {
        public readonly Workspace Workspace;

        private long id;
        private bool isPushed;

        // Role
        private Record record;

        private Dictionary<IRelationType, Change[]> changesByRelationType;
        private Dictionary<IRelationType, RefRange<Strategy>> dependentsByRelationType;

        // Association
        private readonly Dictionary<IAssociationType, Strategy> databaseCompositeAssociationByAssociationType;
        private readonly Dictionary<IAssociationType, RefRange<Strategy>> databaseCompositesAssociationByAssociationType;

        private readonly Dictionary<IAssociationType, RefRange<Strategy>> involvedAssociationsByAssociationType;

        protected Strategy(Workspace workspace, IClass @class, long id)
        {
            this.Workspace = workspace;
            this.id = id;
            this.Class = @class;
            this.isPushed = false;

            this.databaseCompositeAssociationByAssociationType = new Dictionary<IAssociationType, Strategy>();
            this.databaseCompositesAssociationByAssociationType = new Dictionary<IAssociationType, RefRange<Strategy>>();

            this.involvedAssociationsByAssociationType = new Dictionary<IAssociationType, RefRange<Strategy>>();
        }

        public long Version => this.record?.Version ?? Allors.Version.WorkspaceInitial;

        public IClass Class { get; }

        public long Id => this.id;

        public bool IsNew => Adapters.Workspace.IsNewId(this.Id);

        public bool IsDeleted => this.Id == 0;

        public bool HasChanges => this.changesByRelationType != null;

        private bool IsVersionInitial => this.Version == Allors.Version.WorkspaceInitial.Value;

        IWorkspace IStrategy.Workspace => this.Workspace;

        protected bool ExistRecord => this.record != null;

        public Dictionary<IRelationType, Change[]> ChangesByRelationType => this.changesByRelationType;

        int IComparable<Strategy>.CompareTo(Strategy other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return other is null ? 1 : this.Id.CompareTo(other.Id);
        }

        public void Delete()
        {
            if (!this.IsNew)
            {
                throw new Exception("Existing database objects can not be deleted");
            }

            foreach (var roleType in this.Class.RoleTypes)
            {
                this.SetRole(roleType, null);
            }

            this.id = 0;
        }

        public IRole Role(IRoleType roleType)
        {
            if (roleType == null)
            {
                throw new ArgumentNullException(nameof(roleType));
            }

            if (roleType.ObjectType.IsUnit)
            {
                return roleType.ObjectType.Tag switch
                {
                    UnitTags.Binary => this.Workspace.BinaryRole(this, roleType),
                    UnitTags.Boolean => this.Workspace.BooleanRole(this, roleType),
                    UnitTags.DateTime => this.Workspace.DateTimeRole(this, roleType),
                    UnitTags.Decimal => this.Workspace.DecimalRole(this, roleType),
                    UnitTags.Float => this.Workspace.FloatRole(this, roleType),
                    UnitTags.Integer => this.Workspace.IntegerRole(this, roleType),
                    UnitTags.String => this.Workspace.StringRole(this, roleType),
                    UnitTags.Unique => this.Workspace.UniqueRole(this, roleType),
                    _ => throw new Exception("Unknown unit role")
                };
            }

            if (roleType.IsOne)
            {
                return this.CompositeRole(roleType);
            }

            return this.CompositesRole(roleType);
        }

        public IUnitRole<T> UnitRole<T>(IRoleType roleType) =>
            roleType.ObjectType.Tag switch
            {
                UnitTags.Binary => (IUnitRole<T>)this.Workspace.BinaryRole(this, roleType),
                UnitTags.Boolean => (IUnitRole<T>)this.Workspace.BooleanRole(this, roleType),
                UnitTags.DateTime => (IUnitRole<T>)this.Workspace.DateTimeRole(this, roleType),
                UnitTags.Decimal => (IUnitRole<T>)this.Workspace.DecimalRole(this, roleType),
                UnitTags.Float => (IUnitRole<T>)this.Workspace.FloatRole(this, roleType),
                UnitTags.Integer => (IUnitRole<T>)this.Workspace.IntegerRole(this, roleType),
                UnitTags.String => (IUnitRole<T>)this.Workspace.StringRole(this, roleType),
                UnitTags.Unique => (IUnitRole<T>)this.Workspace.UniqueRole(this, roleType),
                _ => throw new Exception("Unknown unit role")
            };

        public ICompositeRole CompositeRole(IRoleType roleType) => this.Workspace.CompositeRole(this, roleType);

        public ICompositeRole<T> CompositeRole<T>(IRoleType roleType) where T : class, IObject => this.Workspace.CompositeRole<T>(this, roleType);

        public ICompositesRole CompositesRole(IRoleType roleType) => this.Workspace.CompositesRole(this, roleType);

        public ICompositesRole<T> CompositesRole<T>(IRoleType roleType) where T : class, IObject => this.Workspace.CompositesRole<T>(this, roleType);

        public IAssociation Association(IAssociationType associationType)
        {
            if (associationType == null)
            {
                throw new ArgumentNullException(nameof(associationType));
            }

            if (associationType.IsOne)
            {
                return this.CompositeAssociation(associationType);
            }

            return this.CompositesAssociation(associationType);
        }

        public ICompositeAssociation CompositeAssociation(IAssociationType associationType) => this.Workspace.CompositeAssociation(this, associationType);

        public ICompositeAssociation<T> CompositeAssociation<T>(IAssociationType associationType) where T : class, IObject => this.Workspace.CompositeAssociation<T>(this, associationType);

        public ICompositesAssociation CompositesAssociation(IAssociationType associationType) => this.Workspace.CompositesAssociation(this, associationType);

        public ICompositesAssociation<T> CompositesAssociation<T>(IAssociationType associationType) where T : class, IObject => this.Workspace.CompositesAssociation<T>(this, associationType);

        public IMethod Method(IMethodType methodType) => this.Workspace.Method(this, methodType);

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

            var permission = this.Workspace.Connection.GetPermission(this.Class, roleType, Operations.Read);
            return this.record.IsPermitted(permission);
        }

        public bool CanWrite(IRoleType roleType)
        {
            if (this.IsVersionInitial)
            {
                return !this.isPushed;
            }

            if (this.isPushed)
            {
                return false;
            }

            if (!this.ExistRecord)
            {
                return true;
            }

            var permission = this.Workspace.Connection.GetPermission(this.Class, roleType, Operations.Write);
            return this.record.IsPermitted(permission);
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

            var permission = this.Workspace.Connection.GetPermission(this.Class, methodType, Operations.Execute);
            return this.record.IsPermitted(permission);
        }

        public bool ExistRole(IRoleType roleType)
        {
            if (roleType.ObjectType.IsUnit)
            {
                return this.GetUnitRole(roleType) != null;
            }

            if (roleType.IsOne)
            {
                return this.GetCompositeRole(roleType) != null;
            }

            return this.GetCompositesRole(roleType).Any();
        }

        public bool IsModified(IRoleType roleType) =>
            this.CanRead(roleType) &&
            (this.changesByRelationType?.ContainsKey(roleType.RelationType) ?? false);

        public void RestoreRole(IRoleType roleType)
        {
            if (!this.CanRead(roleType) || this.changesByRelationType == null)
            {
                return;
            }

            if (!this.changesByRelationType.TryGetValue(roleType.RelationType, out var changes))
            {
                return;
            }

            changes = changes.Where(v => v.Dependee != null).ToArray();

            if (changes.Length == 0)
            {
                this.changesByRelationType.Remove(roleType.RelationType);
            }
            else
            {
                this.changesByRelationType[roleType.RelationType] = changes;
            }

            if (this.dependentsByRelationType?.TryGetValue(roleType.RelationType, out var dependents) == true)
            {
                foreach (var dependent in dependents)
                {
                    dependent.RestoreDependentRole(roleType, this);
                }

                this.dependentsByRelationType.Remove(roleType.RelationType);
            }
        }

        public object GetRole(IRoleType roleType)
        {
            if (roleType == null)
            {
                throw new ArgumentNullException(nameof(roleType));
            }

            if (roleType.ObjectType.IsUnit)
            {
                return this.GetUnitRole(roleType);
            }

            if (roleType.IsOne)
            {
                return this.GetCompositeRole(roleType);
            }

            return this.GetCompositesRole(roleType);
        }

        public object GetUnitRole(IRoleType roleType)
        {
            if (!this.CanRead(roleType))
            {
                return null;
            }

            if (this.changesByRelationType?.TryGetValue(roleType.RelationType, out var changes) == true)
            {
                var change = (SetUnitChange)changes[0];
                return change.Role;
            }

            return this.record?.GetRole(roleType);
        }

        public IStrategy GetCompositeRole(IRoleType roleType)
        {
            return this.CanRead(roleType)
                    ? this.GetCompositeRoleStrategy(roleType)
                    : null;
        }

        public IEnumerable<IStrategy> GetCompositesRole(IRoleType roleType)
        {
            return this.CanRead(roleType)
                    ? this.GetCompositesRoleStrategies(roleType).Select(v => v)
                    : Array.Empty<IStrategy>();
        }

        public void SetRole(IRoleType roleType, object role)
        {
            if (roleType.ObjectType.IsUnit)
            {
                this.SetUnitRole(roleType, role);
            }
            else if (roleType.IsOne)
            {
                this.SetCompositeRole(roleType, (IStrategy)role);
            }
            else
            {
                this.SetCompositesRole(roleType, (IEnumerable<IStrategy>)role);
            }
        }

        public void SetUnitRole(IRoleType roleType, object role)
        {
            AssertUnit(roleType, role);

            if (!this.CanWrite(roleType))
            {
                return;
            }

            var recordRole = this.record?.GetRole(roleType);

            if (Equals(recordRole, role))
            {
                this.changesByRelationType?.Remove(roleType.RelationType);
                if (this.changesByRelationType?.Count == 0)
                {
                    this.changesByRelationType = null;
                }

                return;
            }

            this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
            this.changesByRelationType[roleType.RelationType] = new Change[]
            {
                new SetUnitChange(role)
            };

            this.Workspace.PushToDatabaseTracker.OnChanged(this);

            this.Workspace.RegisterWorkspaceReaction(this, roleType);

            this.Workspace.HandleWorkspaceReactions();
        }

        public void SetCompositeRole(IRoleType roleType, IStrategy value)
        {
            if (value != null)
            {
                this.AssertComposite(value);
                this.AssertSameType(roleType, value);
                this.AssertSameSession(value);
            }

            if (roleType.IsMany)
            {
                throw new ArgumentException($"Given {nameof(roleType)} is the wrong multiplicity");
            }

            if (this.CanWrite(roleType))
            {
                if (roleType.AssociationType.IsOne)
                {
                    this.SetCompositeRoleOneToOne(roleType, (Strategy)value);
                }
                else
                {
                    this.SetCompositeRoleManyToOne(roleType, (Strategy)value);
                }

                this.Workspace.HandleWorkspaceReactions();
            }
        }

        public void SetCompositesRole(IRoleType roleType, in IEnumerable<IStrategy> role)
        {
            this.AssertComposites(role);

            if (this.CanWrite(roleType))
            {
                var newRoleStrategies = RefRange<Strategy>.Load(role?.Select(v => (Strategy)v));
                var existingRoleStrategies = this.GetCompositesRoleStrategies(roleType);

                var addRoleStrategies = newRoleStrategies.Except(existingRoleStrategies);
                var removeRoleStrategies = existingRoleStrategies.Except(newRoleStrategies);

                var associationType = roleType.AssociationType;

                foreach (var addRoleStrategy in addRoleStrategies)
                {
                    if (associationType.IsOne)
                    {
                        this.AddCompositesRoleStrategyOneToMany(roleType, addRoleStrategy);
                    }
                    else
                    {
                        this.AddCompositesRoleStrategyManyToMany(roleType, addRoleStrategy);
                    }
                }

                foreach (var removeRoleStrategy in removeRoleStrategies)
                {
                    if (associationType.IsOne)
                    {
                        this.RemoveCompositesRoleStrategyOneToMany(roleType, removeRoleStrategy, null);
                    }
                    else
                    {
                        this.RemoveCompositesRoleStrategyManyToMany(roleType, removeRoleStrategy, null);
                    }
                }

                this.Workspace.HandleWorkspaceReactions();
            }
        }

        public void AddCompositesRole(IRoleType roleType, IStrategy value)
        {
            if (value == null)
            {
                return;
            }

            this.AssertComposite(value);
            this.AssertSameType(roleType, value);

            if (roleType.IsOne)
            {
                throw new ArgumentException($"Given {nameof(roleType)} is the wrong multiplicity");
            }

            if (this.CanWrite(roleType))
            {
                var associationType = roleType.AssociationType;

                if (associationType.IsOne)
                {
                    this.AddCompositesRoleStrategyOneToMany(roleType, (Strategy)value);
                }
                else
                {
                    this.AddCompositesRoleStrategyManyToMany(roleType, (Strategy)value);
                }

                this.Workspace.HandleWorkspaceReactions();
            }
        }

        public void RemoveCompositesRole(IRoleType roleType, IStrategy value)
        {
            if (value == null)
            {
                return;
            }

            this.AssertComposite(value);

            if (this.CanWrite(roleType))
            {
                var associationType = roleType.AssociationType;

                if (associationType.IsOne)
                {
                    this.RemoveCompositesRoleStrategyOneToMany(roleType, (Strategy)value, null);
                }
                else
                {
                    this.RemoveCompositesRoleStrategyManyToMany(roleType, (Strategy)value, null);
                }

                this.Workspace.HandleWorkspaceReactions();
            }
        }

        public IStrategy GetCompositeAssociation(IAssociationType associationType)
        {
            return this.GetCompositeAssociationStrategy(associationType);
        }

        public IEnumerable<Strategy> GetCompositesAssociation(IAssociationType associationType)
        {
            return this.GetCompositesAssociationStrategies(this, associationType);
        }

        public void OnPushNewId(long newId) => this.id = newId;

        public void OnPushed() => this.isPushed = true;

        public void OnPulled(IPullResultInternals pull)
        {
            var newRecord = this.Workspace.Connection.GetRecord(this.Id);

            // Remove old associations
            if (this.record != null && this.record != newRecord)
            {
                foreach (var roleType in this.Class.RoleTypes.Where(v => v.ObjectType.IsComposite))
                {
                    if (roleType.IsOne)
                    {
                        var roleRef = this.record?.GetRole(roleType);

                        if (roleRef == null)
                        {
                            continue;
                        }

                        var roleStrategy = this.Workspace.GetStrategy((long)roleRef);

                        if (roleStrategy != null)
                        {
                            roleStrategy.OnPulledRemoveOldAssociation(roleType.AssociationType, this);
                        }
                    }
                    else
                    {
                        var roleRefs = ValueRange<long>.Ensure(this.record?.GetRole(roleType));

                        if (roleRefs.IsEmpty)
                        {
                            continue;
                        }

                        var roleStrategies = RefRange<Strategy>.Load(roleRefs.Select(this.Workspace.GetStrategy).Where(v => v != null));

                        foreach (var roleStrategy in roleStrategies)
                        {
                            roleStrategy.OnPulledRemoveOldAssociation(roleType.AssociationType, roleStrategy);
                        }
                    }
                }
            }

            if (!this.isPushed)
            {
                if (this.changesByRelationType != null)
                {
                    foreach (var kvp in this.changesByRelationType)
                    {
                        var relationType = kvp.Key;
                        var roleType = relationType.RoleType;

                        var databaseRole = this.record?.GetRole(roleType);
                        var newDatabaseRole = newRecord?.GetRole(roleType);

                        if (roleType.ObjectType.IsUnit)
                        {
                            if (!Equals(databaseRole, newDatabaseRole))
                            {
                                var conflict = new Conflict(this, roleType, this.GetRole(roleType));
                                pull.AddMergeError(conflict);
                                this.RestoreRole(roleType);
                            }
                        }
                        else if (roleType.IsOne)
                        {
                            if (!Equals(databaseRole, newDatabaseRole))
                            {
                                var conflict = new Conflict(this, roleType, this.GetRole(roleType));
                                pull.AddMergeError(conflict);
                                this.RestoreRole(roleType);
                            }
                        }
                        else if (!ValueRange<long>.Ensure(databaseRole).Equals(ValueRange<long>.Ensure(newDatabaseRole)))
                        {
                            var conflict = new Conflict(this, roleType, this.GetRole(roleType));
                            pull.AddMergeError(conflict);
                            this.RestoreRole(roleType);
                        }
                    }
                }
            }
            else
            {
                this.Reset();
                this.isPushed = false;
            }

            // Update Associations
            if (this.record != newRecord)
            {
                foreach (var roleType in this.Class.RoleTypes.Where(v => v.ObjectType.IsComposite))
                {
                    if (roleType.IsOne)
                    {
                        var roleRef = newRecord.GetRole(roleType);

                        if (roleRef == null)
                        {
                            continue;
                        }

                        var roleStrategy = this.Workspace.GetStrategy((long)roleRef);

                        if (roleStrategy != null)
                        {
                            roleStrategy.OnPulledAddAssociation(roleType.AssociationType, this);
                        }
                    }
                    else
                    {
                        var roleRefs = ValueRange<long>.Ensure(newRecord.GetRole(roleType));

                        if (roleRefs.IsEmpty)
                        {
                            continue;
                        }

                        var roleStrategies = RefRange<Strategy>.Load(roleRefs.Select(this.Workspace.GetStrategy).Where(v => v != null));

                        foreach (var roleStrategy in roleStrategies)
                        {
                            roleStrategy.OnPulledAddAssociation(roleType.AssociationType, this);
                        }
                    }
                }
            }

            this.record = newRecord;
        }

        internal void Reset()
        {
            this.changesByRelationType = null;
            this.dependentsByRelationType = null;

            this.involvedAssociationsByAssociationType.Clear();

            if (this.IsNew)
            {
                this.id = 0;
            }
        }

        private Strategy GetCompositeRoleStrategy(IRoleType roleType, bool assertStrategy = true)
        {
            if (this.changesByRelationType?.TryGetValue(roleType.RelationType, out var changes) == true)
            {
                var change = (SetCompositeChange)changes[0];
                return change.Role;
            }

            var role = this.record?.GetRole(roleType);

            if (role == null)
            {
                return null;
            }

            var strategy = this.Workspace.GetStrategy((long)role);

            if (assertStrategy)
            {
                this.AssertStrategy(strategy);
            }

            return strategy;
        }

        private void SetCompositeRoleOneToOne(IRoleType roleType, Strategy role)
        {
            /*  [if exist]        [then remove]        set
            *
            *  RA ----- R         RA --x-- R       RA    -- R       RA    -- R
            *                ->                +        -        =       -
            *   A ----- PR         A --x-- PR       A --    PR       A --    PR
            */

            if (this.SameRoleStrategy(roleType, role))
            {
                return;
            }

            Strategy previousRole = this.GetCompositeRoleStrategy(roleType, false);
            Strategy roleAssociation = role?.GetCompositeAssociationStrategy(roleType.AssociationType);

            // RA --x-- R
            if (roleAssociation != null)
            {
                roleAssociation.Workspace.RegisterWorkspaceReaction(roleAssociation, roleType);
                this.AddDependent(roleType, roleAssociation);
                roleAssociation.SetCompositeChange(roleType, new SetCompositeChange(null, this));
                roleAssociation.Workspace.PushToDatabaseTracker.OnChanged(roleAssociation);
            }

            // A --x-- PR
            previousRole?.AddInvolvedAssociation(roleType.AssociationType, this);
            Strategy tempQualifier = previousRole;
            if (tempQualifier != null)
            {
                tempQualifier.Workspace.RegisterWorkspaceReaction(tempQualifier, roleType.AssociationType);
            }

            // A ----> R
            this.SetCompositeChange(roleType, new SetCompositeChange(role, null));
            this.Workspace.RegisterWorkspaceReaction(this, roleType);
            this.Workspace.PushToDatabaseTracker.OnChanged(this);

            // A <---- R
            role?.AddInvolvedAssociation(roleType.AssociationType, this);
            Strategy tempQualifier1 = role;
            if (tempQualifier1 != null)
            {
                tempQualifier1.Workspace.RegisterWorkspaceReaction(tempQualifier1, roleType.AssociationType);
            }
        }

        private void SetCompositeRoleManyToOne(IRoleType roleType, Strategy role)
        {
            /*  [if exist]        [then remove]        set
            *
            *  RA ----- R         RA       R       RA    -- R       RA ----- R
            *                ->                +        -        =       -
            *   A ----- PR         A --x-- PR       A --    PR       A --    PR
            */

            if (this.SameRoleStrategy(roleType, role))
            {
                return;
            }

            Strategy previousRole = this.GetCompositeRoleStrategy(roleType, false);

            // A --x-- PR
            previousRole?.AddInvolvedAssociation(roleType.AssociationType, this);
            Strategy tempQualifier = previousRole;
            if (tempQualifier != null)
            {
                tempQualifier.Workspace.RegisterWorkspaceReaction(tempQualifier, roleType.AssociationType);
            }

            // A ----> R
            this.SetCompositeChange(roleType, new SetCompositeChange(role, null));
            this.Workspace.RegisterWorkspaceReaction(this, roleType);
            this.Workspace.PushToDatabaseTracker.OnChanged(this);

            // A <---- R
            role?.AddInvolvedAssociation(roleType.AssociationType, this);
            Strategy tempQualifier1 = role;
            if (tempQualifier1 != null)
            {
                tempQualifier1.Workspace.RegisterWorkspaceReaction(tempQualifier1, roleType.AssociationType);
            }
        }

        private RefRange<Strategy> GetCompositesRoleStrategies(IRoleType roleType, bool assertStrategy = true)
        {
            var roleIds = ValueRange<long>.Ensure(this.record?.GetRole(roleType));

            var role = roleIds.IsEmpty
                ? RefRange<Strategy>.Empty
                : RefRange<Strategy>.Load(roleIds.Select(v =>
                {
                    var strategy = this.Workspace.GetStrategy(v);
                    if (assertStrategy)
                    {
                        this.AssertStrategy(strategy);
                    }

                    return strategy;
                }));

            if (this.changesByRelationType?.TryGetValue(roleType.RelationType, out var changes) == true)
            {
                foreach (var change in changes)
                {
                    role = change switch
                    {
                        AddCompositeChange add => role.Add(add.Role),
                        RemoveCompositeChange remove => role.Remove(remove.Role),
                        _ => role
                    };
                }
            }

            return role;
        }

        private void AddCompositesRoleStrategyOneToMany(IRoleType roleType, Strategy role)
        {
            /*  [if exist]        [then remove]        set
            *
            *  RA ----- R         RA --x-- R       RA    -- R       RA    -- R
            *                ->                +        -        =       -
            *   A ----- PR         A       PR       A --    PR       A ----- PR
            */

            var previousRole = this.GetCompositesRoleStrategies(roleType);

            // R in PR
            if (previousRole.Contains(role))
            {
                return;
            }

            var roleAssociation = role.GetCompositeAssociationStrategy(roleType.AssociationType);

            // RA --x-- R
            roleAssociation?.RemoveCompositesRoleStrategyOneToMany(roleType, role, this);

            // A ----> R
            this.AddCompositesRoleStrategyChange(roleType, role);
            this.Workspace.RegisterWorkspaceReaction(this, roleType);
            this.Workspace.PushToDatabaseTracker.OnChanged(this);

            // A <---- R
            role.AddInvolvedAssociation(roleType.AssociationType, this);
            role.Workspace.RegisterWorkspaceReaction(role, roleType.AssociationType);
        }

        private void AddCompositesRoleStrategyManyToMany(IRoleType roleType, Strategy role)
        {
            /*  [if exist]        [no remove]         set
             *
             *  RA ----- R         RA       R       RA    -- R       RA ----- R
             *                ->                +        -        =       -
             *   A ----- PR         A       PR       A --    PR       A ----- PR
             */

            var previousRole = this.GetCompositesRoleStrategies(roleType);
            if (previousRole.Contains(role))
            {
                return;
            }

            // A ----> R
            this.AddCompositesRoleStrategyChange(roleType, role);
            this.Workspace.RegisterWorkspaceReaction(this, roleType);
            this.Workspace.PushToDatabaseTracker.OnChanged(this);

            // A <---- R
            role.AddInvolvedAssociation(roleType.AssociationType, this);
            role.Workspace.RegisterWorkspaceReaction(role, roleType.AssociationType);
        }

        private void AddCompositesRoleStrategyChange(IRoleType roleType, Strategy roleToAdd)
        {
            if (this.changesByRelationType?.TryGetValue(roleType.RelationType, out var changes) == true)
            {
                AddCompositeChange add = null;
                RemoveCompositeChange remove = null;

                foreach (var change in changes)
                {
                    switch (change)
                    {
                    case AddCompositeChange addCompositeChange when addCompositeChange.Role == roleToAdd:
                        add = addCompositeChange;
                        break;
                    case RemoveCompositeChange removeCompositeChange when removeCompositeChange.Role == roleToAdd:
                        remove = removeCompositeChange;
                        break;
                    }
                }

                if (remove != null)
                {
                    changes = changes.Where(v => v == remove).ToArray();
                }
                else if (add == null)
                {
                    changes = changes.Append(new AddCompositeChange(roleToAdd, null)).ToArray();
                }

                this.changesByRelationType[roleType.RelationType] = changes;
            }
            else
            {
                this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
                this.changesByRelationType[roleType.RelationType] = new Change[] { new AddCompositeChange(roleToAdd, null) };
            }
        }

        private void RemoveCompositesRoleStrategyOneToMany(IRoleType roleType, Strategy role, Strategy dependee)
        {
            var previousRole = this.GetCompositesRoleStrategies(roleType);

            // R not in PR
            if (!previousRole.Contains(role))
            {
                return;
            }

            // A ----> R
            if (this.changesByRelationType?.TryGetValue(roleType.RelationType, out var changes) == true)
            {
                AddCompositeChange add = null;
                RemoveCompositeChange remove = null;

                foreach (var change in changes)
                {
                    switch (change)
                    {
                    case AddCompositeChange addCompositeChange when addCompositeChange.Role == role:
                        add = addCompositeChange;
                        break;
                    case RemoveCompositeChange removeCompositeChange when removeCompositeChange.Role == role:
                        remove = removeCompositeChange;
                        break;
                    }
                }

                if (add != null)
                {
                    changes = changes.Where(v => v != add).ToArray();
                }
                else if (remove == null)
                {
                    changes = changes.Append(new RemoveCompositeChange(role, dependee)).ToArray();
                }

                this.changesByRelationType[roleType.RelationType] = changes;
            }
            else
            {
                this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
                this.changesByRelationType[roleType.RelationType] = new Change[] { new RemoveCompositeChange(role, dependee) };
            }

            dependee?.AddDependent(roleType, this);
            this.Workspace.RegisterWorkspaceReaction(this, roleType);
            this.Workspace.PushToDatabaseTracker.OnChanged(this);

            // A <---- R
            role.Workspace.RegisterWorkspaceReaction(role, roleType.AssociationType);
        }

        private void RemoveCompositesRoleStrategyManyToMany(IRoleType roleType, Strategy role, Strategy dependee)
        {
            var previousRole = this.GetCompositesRoleStrategies(roleType);

            // R not in PR
            if (!previousRole.Contains(role))
            {
                return;
            }

            // A ----> R
            if (this.changesByRelationType?.TryGetValue(roleType.RelationType, out var changes) == true)
            {
                AddCompositeChange add = null;
                RemoveCompositeChange remove = null;

                foreach (var change in changes)
                {
                    switch (change)
                    {
                    case AddCompositeChange addCompositeChange when addCompositeChange.Role == role:
                        add = addCompositeChange;
                        break;
                    case RemoveCompositeChange removeCompositeChange when removeCompositeChange.Role == role:
                        remove = removeCompositeChange;
                        break;
                    }
                }

                if (add != null)
                {
                    changes = changes.Where(v => v != add).ToArray();
                }
                else if (remove == null)
                {
                    changes = changes.Append(new RemoveCompositeChange(role, dependee)).ToArray();
                }

                this.changesByRelationType[roleType.RelationType] = changes;
            }
            else
            {
                this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
                this.changesByRelationType[roleType.RelationType] = new Change[] { new RemoveCompositeChange(role, dependee) };
            }

            dependee?.AddDependent(roleType, this);
            this.Workspace.RegisterWorkspaceReaction(this, roleType);
            this.Workspace.PushToDatabaseTracker.OnChanged(this);

            // A <---- R
            role.Workspace.RegisterWorkspaceReaction(role, roleType.AssociationType);
        }

        private void AddInvolvedAssociation(IAssociationType associationType, Strategy association)
        {
            this.involvedAssociationsByAssociationType.TryGetValue(associationType, out var associations);

            var associationRefs = RefRange<Strategy>.Ensure(associations);
            associationRefs = associationRefs.Add(association);

            this.involvedAssociationsByAssociationType[associationType] = associationRefs;
        }

        private bool IsAssociationForRole(IRoleType roleType, Strategy forRole)
        {
            if (roleType.IsOne)
            {
                var compositeRole = this.GetCompositeRoleStrategy(roleType, false);
                return compositeRole == forRole;
            }

            var compositesRole = RefRange<Strategy>.Load(this.GetCompositesRoleStrategies(roleType, false).Where(v => v != null));
            return compositesRole.Contains(forRole);
        }

        private Strategy GetCompositeAssociationStrategy(IAssociationType associationType)
        {
            var roleType = associationType.RelationType.RoleType;

            if (this.involvedAssociationsByAssociationType.TryGetValue(associationType, out var involvedAssociations))
            {
                foreach (var involvedAssociation in involvedAssociations)
                {
                    if (!involvedAssociation.CanRead(roleType))
                    {
                        continue;
                    }

                    if (involvedAssociation.IsAssociationForRole(roleType, this))
                    {
                        return involvedAssociation;
                    }
                }
            }

            if (this.databaseCompositeAssociationByAssociationType.TryGetValue(associationType, out var association))
            {
                if (association?.CanRead(roleType) == true)
                {
                    if (association.IsAssociationForRole(roleType, this))
                    {
                        return association;
                    }
                }
            }

            return null;
        }

        private IEnumerable<Strategy> GetCompositesAssociationStrategies(Strategy role, IAssociationType associationType)
        {
            var roleType = associationType.RelationType.RoleType;

            var yielded = false;

            var deduplicate = new HashSet<Strategy>();

            if (this.involvedAssociationsByAssociationType.TryGetValue(associationType, out var involvedAssociation))
            {
                foreach (var association in involvedAssociation)
                {
                    if (!association.CanRead(roleType))
                    {
                        continue;
                    }

                    if (association.IsAssociationForRole(roleType, role))
                    {
                        deduplicate.Add(association);
                        yielded = true;
                        yield return association;
                    }
                }
            }

            if (associationType.IsOne && yielded)
            {
                yield break;
            }

            if (this.databaseCompositesAssociationByAssociationType.TryGetValue(associationType, out var associationStrategies))
            {
                foreach (var association in associationStrategies)
                {
                    if (association?.CanRead(roleType) == true)
                    {
                        if (association.IsAssociationForRole(roleType, this))
                        {
                            if (!deduplicate.Contains(association))
                            {
                                if (associationType.IsOne && yielded)
                                {
                                    yield break;
                                }

                                deduplicate.Add(association);
                                yielded = true;
                                yield return association;
                            }
                        }
                    }
                }
            }
        }

        private void OnPulledRemoveOldAssociation(IAssociationType associationType, Strategy associationToRemove)
        {
            if (associationType.IsOne)
            {
                if (this.databaseCompositeAssociationByAssociationType.TryGetValue(associationType, out var association))
                {
                    if (association == associationToRemove)
                    {
                        this.databaseCompositeAssociationByAssociationType[associationType] = null;
                    }
                }
            }
            else
            {
                if (this.databaseCompositesAssociationByAssociationType.TryGetValue(associationType, out var association))
                {
                    association = association.Remove(associationToRemove);
                    this.databaseCompositesAssociationByAssociationType[associationType] = association;
                }
            }
        }

        private void OnPulledAddAssociation(IAssociationType associationType, Strategy associationToAdd)
        {
            if (associationType.IsOne)
            {
                this.databaseCompositeAssociationByAssociationType[associationType] = associationToAdd;
            }
            else
            {
                if (this.databaseCompositesAssociationByAssociationType.TryGetValue(associationType, out var association))
                {
                    association = association.Add(associationToAdd);
                    this.databaseCompositesAssociationByAssociationType[associationType] = association;
                }
                else
                {
                    this.databaseCompositesAssociationByAssociationType[associationType] = RefRange<Strategy>.Load(associationToAdd);
                }
            }
        }

        private void AddDependent(IRoleType roleType, Strategy dependent)
        {
            this.dependentsByRelationType ??= new Dictionary<IRelationType, RefRange<Strategy>>();
            if (this.dependentsByRelationType.TryGetValue(roleType.RelationType, out var dependents))
            {
                dependents = dependents.Add(dependent);
            }
            else
            {
                dependents = RefRange<Strategy>.Load(dependent);
            }

            this.dependentsByRelationType[roleType.RelationType] = dependents;

        }

        private void RestoreDependentRole(IRoleType roleType, Strategy dependee)
        {
            if (!this.changesByRelationType.TryGetValue(roleType.RelationType, out var changes))
            {
                return;
            }

            changes = changes.Where(v => v.Dependee != dependee).ToArray();

            if (changes.Length == 0)
            {
                this.changesByRelationType.Remove(roleType.RelationType);
            }
            else
            {
                this.changesByRelationType[roleType.RelationType] = changes;
            }
        }

        private bool SameRoleStrategy(IRoleType roleType, Strategy role)
        {
            if (this.changesByRelationType != null && this.changesByRelationType.TryGetValue(roleType.RelationType, out var changes))
            {
                var change = (SetCompositeChange)changes[0];
                return role == change.Role;
            }

            var recordRoleId = this.record?.GetRole(roleType);

            if (role == null)
            {
                return recordRoleId == null;
            }

            if (recordRoleId == null)
            {
                return false;
            }

            return role.Id == (long)recordRoleId;
        }

        private void SetCompositeChange(IRoleType roleType, SetCompositeChange setCompositeChange)
        {
            this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
            this.changesByRelationType[roleType.RelationType] = new Change[]
            {
                setCompositeChange
            };
        }

        private static void AssertUnit(IRoleType roleType, object value)
        {
            if (value == null)
            {
                return;
            }

            switch (roleType.ObjectType.Tag)
            {
            case UnitTags.Binary:
                if (!(value is byte[]))
                {
                    throw new ArgumentException($"{nameof(value)} is not a Binary");
                }
                break;
            case UnitTags.Boolean:
                if (!(value is bool))
                {
                    throw new ArgumentException($"{nameof(value)} is not an Bool");
                }
                break;
            case UnitTags.DateTime:
                if (!(value is DateTime))
                {
                    throw new ArgumentException($"{nameof(value)} is not an DateTime");
                }
                break;
            case UnitTags.Decimal:
                if (!(value is decimal))
                {
                    throw new ArgumentException($"{nameof(value)} is not an Decimal");
                }
                break;
            case UnitTags.Float:
                if (!(value is double))
                {
                    throw new ArgumentException($"{nameof(value)} is not an Float");
                }
                break;
            case UnitTags.Integer:
                if (!(value is int))
                {
                    throw new ArgumentException($"{nameof(value)} is not an Integer");
                }
                break;
            case UnitTags.String:
                if (!(value is string))
                {
                    throw new ArgumentException($"{nameof(value)} is not an String");
                }
                break;
            case UnitTags.Unique:
                if (!(value is Guid))
                {
                    throw new ArgumentException($"{nameof(value)} is not an Unique");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(roleType));
            }
        }

        private void AssertSameType(IRoleType roleType, IStrategy value)
        {
            if (!((IComposite)roleType.ObjectType).IsAssignableFrom(value.Class))
            {
                throw new ArgumentException($"Types do not match: {nameof(roleType)}: {roleType.ObjectType.ClrType} and {nameof(value)}: {value.GetType()}");
            }
        }

        private void AssertSameSession(IStrategy value)
        {
            if (this.Workspace != value.Workspace)
            {
                throw new ArgumentException("Workspace do not match");
            }
        }

        private void AssertComposites(IEnumerable<IStrategy> inputs)
        {
            if (inputs == null)
            {
                return;
            }

            foreach (var input in inputs)
            {
                this.AssertComposite(input);
            }
        }

        private void AssertComposite(IStrategy value)
        {
            if (this.Workspace != value.Workspace)
            {
                throw new ArgumentException("Strategy is from a different workspace");
            }
        }

        private void AssertStrategy(Strategy strategy)
        {
            if (strategy == null)
            {
                throw new Exception("Strategy is not in Workspace.");
            }
        }
    }
}
