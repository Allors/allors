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
        private Dictionary<IRelationType, RefRange<Strategy>> targetsByRelationType;

        // Association
        private readonly Dictionary<IAssociationType, Strategy> databaseCompositeAssociationByAssociationType;
        private readonly Dictionary<IAssociationType, RefRange<Strategy>> databaseCompositesAssociationByAssociationType;

        private readonly Dictionary<IAssociationType, RefRange<Strategy>> changedAssociationByAssociationType;

        protected Strategy(Workspace workspace, IClass @class, long id)
        {
            this.Workspace = workspace;
            this.id = id;
            this.Class = @class;
            this.isPushed = false;

            this.databaseCompositeAssociationByAssociationType = new Dictionary<IAssociationType, Strategy>();
            this.databaseCompositesAssociationByAssociationType = new Dictionary<IAssociationType, RefRange<Strategy>>();

            this.changedAssociationByAssociationType = new Dictionary<IAssociationType, RefRange<Strategy>>();
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
                return this.UnitRole(roleType);
            }

            if (roleType.IsOne)
            {
                return this.CompositeRole(roleType);
            }

            return this.CompositesRole(roleType);
        }

        public IUnitRole UnitRole(IRoleType roleType) =>
            roleType.ObjectType.Tag switch
            {
                UnitTags.Binary => this.BinaryRole(roleType),
                UnitTags.Boolean => this.BooleanRole(roleType),
                UnitTags.DateTime => this.DateTimeRole(roleType),
                UnitTags.Decimal => this.DecimalRole(roleType),
                UnitTags.Float => this.FloatRole(roleType),
                UnitTags.Integer => this.IntegerRole(roleType),
                UnitTags.String => this.StringRole(roleType),
                UnitTags.Unique => this.UniqueRole(roleType),
                _ => throw new Exception("Unknown unit role")
            };

        public IBinaryRole BinaryRole(IRoleType roleType) => this.Workspace.BinaryRole(this, roleType);

        public IBooleanRole BooleanRole(IRoleType roleType) => this.Workspace.BooleanRole(this, roleType);

        public IDateTimeRole DateTimeRole(IRoleType roleType) => this.Workspace.DateTimeRole(this, roleType);

        public IDecimalRole DecimalRole(IRoleType roleType) => this.Workspace.DecimalRole(this, roleType);

        public IFloatRole FloatRole(IRoleType roleType) => this.Workspace.FloatRole(this, roleType);

        public IIntegerRole IntegerRole(IRoleType roleType) => this.Workspace.IntegerRole(this, roleType);

        public IStringRole StringRole(IRoleType roleType) => this.Workspace.StringRole(this, roleType);

        public IUniqueRole UniqueRole(IRoleType roleType) => this.Workspace.UniqueRole(this, roleType);

        public ICompositeRole CompositeRole(IRoleType roleType) => this.Workspace.CompositeRole(this, roleType);

        public ICompositesRole CompositesRole(IRoleType roleType) => this.Workspace.CompositesRole(this, roleType);

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

        public ICompositesAssociation CompositesAssociation(IAssociationType associationType) => this.Workspace.CompositesAssociation(this, associationType);

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

            changes = changes.Where(v => v.Source != null).ToArray();

            if (changes.Length == 0)
            {
                this.changesByRelationType.Remove(roleType.RelationType);
            }
            else
            {
                this.changesByRelationType[roleType.RelationType] = changes;
            }

            if (this.targetsByRelationType?.TryGetValue(roleType.RelationType, out var targets) == true)
            {
                foreach (var target in targets)
                {
                    target.RestoreTargetRole(roleType, this);
                }

                this.targetsByRelationType.Remove(roleType.RelationType);
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

            this.Workspace.Reaction(this, roleType);
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
                this.SetCompositeRoleStrategy(roleType, (Strategy)value, null);
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

                foreach (var addRoleStrategy in addRoleStrategies)
                {
                    this.AddCompositesRoleStrategy(roleType, addRoleStrategy);
                }

                foreach (var removeRoleStrategy in removeRoleStrategies)
                {
                    this.RemoveCompositesRoleStrategy(roleType, removeRoleStrategy, null);
                }
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
                this.AddCompositesRoleStrategy(roleType, (Strategy)value);
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
                this.RemoveCompositesRoleStrategy(roleType, (Strategy)value, null);
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
                this.SetCompositeRole(roleType, (IStrategy)null);
            }
            else
            {
                this.SetCompositesRole(roleType, null);
            }
        }

        public IStrategy GetCompositeAssociation(IAssociationType associationType)
        {
            return this.GetCompositeAssociationStrategy(associationType);
        }

        public IEnumerable<IStrategy> GetCompositesAssociation(IAssociationType associationType)
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
            this.targetsByRelationType = null;

            this.changedAssociationByAssociationType.Clear();

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

        private void SetCompositeRoleStrategy(IRoleType roleType, Strategy role, Strategy source)
        {
            if (this.SameRoleStrategy(roleType, role))
            {
                return;
            }

            this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
            this.changesByRelationType[roleType.RelationType] = new Change[]
            {
                new SetCompositeChange(role, source)
            };

            source?.AddTarget(roleType, this);

            // OneToOne
            var associationType = roleType.AssociationType;
            if (associationType.IsOne && role != null)
            {
                Strategy previousAssociation = associationType.IsOne ? role.GetCompositeAssociationStrategy(associationType) : null;
                if (associationType.IsOne && previousAssociation != null)
                {
                    var previousRole = previousAssociation.GetCompositeRoleStrategy(roleType);
                    previousRole?.AddChangedAssociation(roleType.AssociationType, this);

                    previousAssociation.SetCompositeRoleStrategy(roleType, null, this);
                }
            }

            role?.AddChangedAssociation(roleType.AssociationType, this);

            this.Workspace.PushToDatabaseTracker.OnChanged(this);

            this.Workspace.Reaction(this, roleType);
        }

        private void AddTarget(IRoleType roleType, Strategy target)
        {
            this.targetsByRelationType ??= new Dictionary<IRelationType, RefRange<Strategy>>();
            if (this.targetsByRelationType.TryGetValue(roleType.RelationType, out var targets))
            {
                targets = targets.Add(target);
            }
            else
            {
                targets = RefRange<Strategy>.Load(target);
            }

            this.targetsByRelationType[roleType.RelationType] = targets;

        }

        private void RestoreTargetRole(IRoleType roleType, Strategy source)
        {
            if (!this.changesByRelationType.TryGetValue(roleType.RelationType, out var changes))
            {
                return;
            }

            changes = changes.Where(v => v.Source != source).ToArray();

            if (changes.Length == 0)
            {
                this.changesByRelationType.Remove(roleType.RelationType);
            }
            else
            {
                this.changesByRelationType[roleType.RelationType] = changes;
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
                    changes = changes.Append(new AddCompositeChange(roleToAdd, null)).ToArray();
                }

                this.changesByRelationType[roleType.RelationType] = changes;
            }
            else
            {
                this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
                this.changesByRelationType[roleType.RelationType] = new Change[] { new AddCompositeChange(roleToAdd, null) };
            }

            roleToAdd.AddChangedAssociation(roleType.AssociationType, this);

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

                previousAssociation?.RemoveCompositesRoleStrategy(roleType, roleToAdd, this);
            }

            this.Workspace.PushToDatabaseTracker.OnChanged(this);
        }

        private void RemoveCompositesRoleStrategy(IRoleType roleType, Strategy roleToRemove, Strategy source)
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
                    changes = changes.Where(v => v != add).ToArray();
                }
                else if (remove == null)
                {
                    changes = changes.Append(new RemoveCompositeChange(roleToRemove, source)).ToArray();
                }

                this.changesByRelationType[roleType.RelationType] = changes;
            }
            else
            {
                this.changesByRelationType ??= new Dictionary<IRelationType, Change[]>();
                this.changesByRelationType[roleType.RelationType] = new Change[] { new RemoveCompositeChange(roleToRemove, source) };
            }

            source?.AddTarget(roleType, this);

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

            var deduplicate = new HashSet<Strategy>();

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

        private bool SameRoleStrategy(IRoleType roleType, Strategy role)
        {
            if (this.changesByRelationType != null && this.changesByRelationType.TryGetValue(roleType.RelationType, out var changes))
            {
                var change = (SetCompositeChange)changes[0];
                return role == change.Role;
            }

            var changedRoleId = this.record?.GetRole(roleType);

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
