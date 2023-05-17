﻿// <copyright file="Object.cs" company="Allors bvba">
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
        private IObject @object;

        private readonly Dictionary<IAssociationType, Strategy> databaseCompositeAssociationByAssociationType;
        private readonly Dictionary<IAssociationType, RefRange<Strategy>> databaseCompositesAssociationByAssociationType;

        private Dictionary<IRelationType, Change[]> changesByRelationType;
        private readonly Dictionary<IAssociationType, RefRange<Strategy>> changedAssociationByAssociationType;

        protected Strategy(Workspace workspace, IClass @class, long id)
        {
            this.Workspace = workspace;
            this.Id = id;
            this.Class = @class;
            this.IsPushed = false;

            this.databaseCompositeAssociationByAssociationType = new Dictionary<IAssociationType, Strategy>();
            this.databaseCompositesAssociationByAssociationType = new Dictionary<IAssociationType, RefRange<Strategy>>();

            this.changedAssociationByAssociationType = new Dictionary<IAssociationType, RefRange<Strategy>>();
        }

        public long Version => this.Record?.Version ?? Allors.Version.WorkspaceInitial;

        IWorkspace IStrategy.Workspace => this.Workspace;

        public IClass Class { get; }

        public long Id { get; private set; }

        public bool IsNew => Workspace.IsNewId(this.Id);

        public IObject Object => this.@object ??= this.Workspace.Connection.Configuration.ObjectFactory.Create(this);

        public bool HasChanges => this.changesByRelationType != null;

        private bool IsVersionInitial => this.Version == Allors.Version.WorkspaceInitial.Value;

        protected Workspace Workspace { get; }

        protected bool ExistRecord => this.Record != null;

        protected Record Record { get; private set; }

        private bool IsPushed { get; set; }

        int IComparable<Strategy>.CompareTo(Strategy other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return other is null ? 1 : this.Id.CompareTo(other.Id);
        }

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

            var permission = this.Workspace.Connection.GetPermission(this.Class, roleType, Operations.Write);
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

            var permission = this.Workspace.Connection.GetPermission(this.Class, methodType, Operations.Execute);
            return this.Record.IsPermitted(permission);
        }

        public bool ExistRole(IRoleType roleType)
        {
            if (roleType.ObjectType.IsUnit)
            {
                return this.GetUnitRole(roleType) != null;
            }

            if (roleType.IsOne)
            {
                return this.GetCompositeRole<IObject>(roleType) != null;
            }

            return this.GetCompositesRole<IObject>(roleType).Any();
        }

        public bool HasChanged(IRoleType roleType) =>
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

            changes = changes.Where(v => !v.IsDirect).ToArray();

            if (changes.Length == 0)
            {
                this.changesByRelationType.Remove(roleType.RelationType);
            }
            else
            {
                this.changesByRelationType[roleType.RelationType] = changes;
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
                return this.GetCompositeRole<IObject>(roleType);
            }

            return this.GetCompositesRole<IObject>(roleType);
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

            return this.Record?.GetRole(roleType);
        }

        public T GetCompositeRole<T>(IRoleType roleType) where T : class, IObject
        {
            return this.CanRead(roleType)
                    ? (T)this.GetCompositeRoleStrategy(roleType)?.Object
                    : null;
        }

        public IEnumerable<T> GetCompositesRole<T>(IRoleType roleType) where T : class, IObject
        {
            return this.CanRead(roleType)
                    ? this.GetCompositesRoleStrategies(roleType).Select(v => (T)v.Object)
                    : Array.Empty<T>();
        }

        public void SetRole(IRoleType roleType, object role)
        {
            if (roleType.ObjectType.IsUnit)
            {
                this.SetUnitRole(roleType, role);
            }
            else if (roleType.IsOne)
            {
                this.SetCompositeRole(roleType, (IObject)role);
            }
            else
            {
                this.SetCompositesRole(roleType, (IEnumerable<IObject>)role);
            }
        }

        public void SetUnitRole(IRoleType roleType, object role)
        {
            AssertUnit(roleType, role);

            if (!this.CanWrite(roleType))
            {
                return;
            }

            var recordRole = this.Record?.GetRole(roleType);

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
        }

        public void SetCompositeRole<T>(IRoleType roleType, T value) where T : class, IObject
        {
            this.AssertComposite(value);

            if (value != null)
            {
                this.AssertSameType(roleType, value);
                this.AssertSameSession(value);
            }

            if (roleType.IsMany)
            {
                throw new ArgumentException($"Given {nameof(roleType)} is the wrong multiplicity");
            }

            if (this.CanWrite(roleType))
            {
                this.SetCompositeRoleStrategy(roleType, (Strategy)value?.Strategy);
            }
        }

        public void SetCompositesRole<T>(IRoleType roleType, in IEnumerable<T> role) where T : class, IObject
        {
            this.AssertComposites(role);

            if (this.CanWrite(roleType))
            {
                var newRoleStrategies = RefRange<Strategy>.Load(role?.Select(v => (Strategy)v.Strategy));
                var existingRoleStrategies = this.GetCompositesRoleStrategies(roleType);

                var addRoleStrategies = newRoleStrategies.Except(existingRoleStrategies);
                var removeRoleStrategies = existingRoleStrategies.Except(newRoleStrategies);

                foreach (var addRoleStrategy in addRoleStrategies)
                {
                    this.AddCompositesRoleStrategy(roleType, addRoleStrategy);
                }

                foreach (var removeRoleStrategy in removeRoleStrategies)
                {
                    this.RemoveCompositesRoleStrategy(roleType, removeRoleStrategy);
                }
            }
        }

        public void AddCompositesRole<T>(IRoleType roleType, T value) where T : class, IObject
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
                this.AddCompositesRoleStrategy(roleType, (Strategy)value.Strategy);
            }
        }

        public void RemoveCompositesRole<T>(IRoleType roleType, T value) where T : class, IObject
        {
            if (value == null)
            {
                return;
            }

            this.AssertComposite(value);

            if (this.CanWrite(roleType))
            {
                this.RemoveCompositesRoleStrategy(roleType, (Strategy)value.Strategy);
            }
        }

        public void RemoveRole(IRoleType roleType)
        {
            if (roleType.ObjectType.IsUnit)
            {
                this.SetUnitRole(roleType, null);
            }
            else if (roleType.IsOne)
            {
                this.SetCompositeRole(roleType, (IObject)null);
            }
            else
            {
                this.SetCompositesRole(roleType, (IEnumerable<IObject>)null);
            }
        }

        public T GetCompositeAssociation<T>(IAssociationType associationType) where T : class, IObject
        {
            return (T)this.GetCompositeAssociationStrategy(associationType)?.Object;
        }

        public IEnumerable<T> GetCompositesAssociation<T>(IAssociationType associationType) where T : class, IObject
        {
            return this.GetCompositesAssociationStrategies(this, associationType).Select(v => v.Object).Cast<T>();
        }

        public void OnPushNewId(long newId) => this.Id = newId;

        public void OnPushed() => this.IsPushed = true;

        public void OnPulled(IPullResultInternals pull)
        {
            var newRecord = this.Workspace.Connection.GetRecord(this.Id);

            // Remove old associations
            if (this.Record != null && this.Record != newRecord)
            {
                foreach (var roleType in this.Class.RoleTypes.Where(v => v.ObjectType.IsComposite))
                {
                    if (roleType.IsOne)
                    {
                        var roleRef = this.Record?.GetRole(roleType);

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
                        var roleRefs = ValueRange<long>.Ensure(this.Record?.GetRole(roleType));

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

            if (!this.IsPushed)
            {
                if (!this.CanMerge(newRecord))
                {
                    pull.AddMergeError(this.Object);
                    return;
                }
            }
            else
            {
                this.Reset();
                this.IsPushed = false;
            }

            // Update Associations
            if (this.Record != newRecord)
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

            this.Record = newRecord;
        }

        internal void Reset()
        {
            this.changesByRelationType = null;

            this.changedAssociationByAssociationType.Clear();
        }

        private Strategy GetCompositeRoleStrategy(IRoleType roleType, bool assertStrategy = true)
        {
            if (this.changesByRelationType?.TryGetValue(roleType.RelationType, out var changes) == true)
            {
                var change = (SetCompositeChange)changes[0];
                return change.Role;
            }

            var role = this.Record?.GetRole(roleType);

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

        private void SetCompositeRoleStrategy(IRoleType roleType, Strategy role)
        {
            if (this.SameRoleStrategy(roleType, role))
            {
                this.changesByRelationType?.Remove(roleType.RelationType);
                if (this.changesByRelationType?.Count == 0)
                {
                    this.changesByRelationType = null;
                }
            }
            else
            {
                this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
                this.changesByRelationType[roleType.RelationType] = new Change[]
                {
                    new SetCompositeChange(role)
                };
            }

            // OneToOne
            var associationType = roleType.AssociationType;
            if (associationType.IsOne && role != null)
            {
                Strategy previousAssociation = associationType.IsOne ? role.GetCompositeAssociationStrategy(associationType) : null;
                if (associationType.IsOne && previousAssociation != null)
                {
                    var previousRole = previousAssociation.GetCompositeRoleStrategy(roleType);
                    previousRole?.AddChangedAssociation(roleType.AssociationType, this);

                    previousAssociation.SetRole(roleType, null);
                }
            }

            role?.AddChangedAssociation(roleType.AssociationType, this);

            this.Workspace.PushToDatabaseTracker.OnChanged(this);
        }

        private RefRange<Strategy> GetCompositesRoleStrategies(IRoleType roleType, bool assertStrategy = true)
        {
            var roleIds = ValueRange<long>.Ensure(this.Record?.GetRole(roleType));

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
                        RemoveCompositeChange remove => role.Add(remove.Role),
                        _ => role
                    };
                }
            }

            return role;
        }

        private void AddCompositesRoleStrategy(IRoleType roleType, Strategy roleToAdd)
        {
            var role = this.GetCompositesRoleStrategies(roleType);
            if (role.Contains(roleToAdd))
            {
                return;
            }

            var associationType = roleType.AssociationType;

            var previousAssociation = associationType.IsOne ? roleToAdd.GetCompositeAssociationStrategy(associationType) : null;

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
                    changes = changes.Append(new AddCompositeChange(roleToAdd)).ToArray();
                }

                this.changesByRelationType[roleType.RelationType] = changes;
            }
            else
            {
                this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
                this.changesByRelationType[roleType.RelationType] = new Change[] { new AddCompositeChange(roleToAdd) };
            }

            roleToAdd?.AddChangedAssociation(roleType.AssociationType, this);

            // OneToMany
            if (associationType.IsOne)
            {
                var previousRoleStrategies = previousAssociation?.GetCompositesRoleStrategies(roleType);
                if (previousRoleStrategies != null)
                {
                    foreach (var previousRoleStrategy in previousRoleStrategies)
                    {
                        previousRoleStrategy?.AddChangedAssociation(roleType.AssociationType, this);

                    }
                }

                previousAssociation?.SetRole(roleType, null);
            }

            this.Workspace.PushToDatabaseTracker.OnChanged(this);
        }

        private void RemoveCompositesRoleStrategy(IRoleType roleType, Strategy roleToRemove)
        {
            var role = this.GetCompositesRoleStrategies(roleType);

            if (!role.Contains(roleToRemove))
            {
                return;
            }

            if (this.changesByRelationType?.TryGetValue(roleType.RelationType, out var changes) == true)
            {
                AddCompositeChange add = null;
                RemoveCompositeChange remove = null;

                foreach (var change in changes)
                {
                    switch (change)
                    {
                    case AddCompositeChange addCompositeChange when addCompositeChange.Role == roleToRemove:
                        add = addCompositeChange;
                        break;
                    case RemoveCompositeChange removeCompositeChange when removeCompositeChange.Role == roleToRemove:
                        remove = removeCompositeChange;
                        break;
                    }
                }

                if (add != null)
                {
                    changes = changes.Where(v => v == add).ToArray();
                }
                else if (remove == null)
                {
                    changes = changes.Append(new RemoveCompositeChange(roleToRemove)).ToArray();
                }

                this.changesByRelationType[roleType.RelationType] = changes;
            }
            else
            {
                this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
                this.changesByRelationType[roleType.RelationType] = new Change[] { new RemoveCompositeChange(roleToRemove) };
            }

            this.Workspace.PushToDatabaseTracker.OnChanged(this);
        }

        private void AddChangedAssociation(IAssociationType associationType, Strategy association)
        {
            this.changedAssociationByAssociationType.TryGetValue(associationType, out var associations);

            var associationRefs = RefRange<Strategy>.Ensure(associations);
            associationRefs = associationRefs.Add(association);

            this.changedAssociationByAssociationType[associationType] = associationRefs;
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

            if (this.changedAssociationByAssociationType.TryGetValue(associationType, out var changedAssociationRefs))
            {
                foreach (var changedAssociation in changedAssociationRefs)
                {
                    if (!changedAssociation.CanRead(roleType))
                    {
                        continue;
                    }

                    if (changedAssociation.IsAssociationForRole(roleType, this))
                    {
                        return changedAssociation;
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

            var dedupe = new HashSet<Strategy>();

            if (this.changedAssociationByAssociationType.TryGetValue(associationType, out var changedAssociationRefs))
            {
                foreach (var association in changedAssociationRefs)
                {
                    if (!association.CanRead(roleType))
                    {
                        continue;
                    }

                    if (association.IsAssociationForRole(roleType, role))
                    {
                        dedupe.Add(association);
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
                            if (!dedupe.Contains(association))
                            {
                                if (associationType.IsOne && yielded)
                                {
                                    yield break;
                                }

                                dedupe.Add(association);
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

        private bool SameRoleStrategy(IRoleType roleType, Strategy role)
        {
            if (this.changesByRelationType != null && this.changesByRelationType.TryGetValue(roleType.RelationType, out var changes))
            {
                var change = (SetCompositeChange)changes[0];
                return role == change.Role;
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

        private bool CanMerge(Record newRecord)
        {
            if (this.changesByRelationType == null)
            {
                return true;
            }

            foreach (var kvp in this.changesByRelationType)
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

        private void AssertSameType<T>(IRoleType roleType, T value) where T : class, IObject
        {
            if (!((IComposite)roleType.ObjectType).IsAssignableFrom(value.Strategy.Class))
            {
                throw new ArgumentException($"Types do not match: {nameof(roleType)}: {roleType.ObjectType.ClrType} and {nameof(value)}: {value.GetType()}");
            }
        }

        private void AssertSameSession(IObject value)
        {
            if (this.Workspace != value.Strategy.Workspace)
            {
                throw new ArgumentException($"Workspace do not match");
            }
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

        private void AssertComposite(IObject value)
        {
            if (value == null)
            {
                return;
            }

            if (this.Workspace != value.Strategy.Workspace)
            {
                throw new ArgumentException("Strategy is from a different workspace");
            }
        }

        private void AssertComposites(IEnumerable<IObject> inputs)
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

        private void AssertStrategy(Strategy strategy)
        {
            if (strategy == null)
            {
                throw new Exception("Strategy is not in Workspace.");
            }
        }
    }
}
