// <copyright file="Object.cs" company="Allors bvba">
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

        protected Strategy(Workspace workspace, IClass @class, long id)
        {
            this.Workspace = workspace;
            this.Id = id;
            this.Class = @class;
            this._Record = workspace.Connection.GetRecord(this.Id);
            this._IsPushed = false;
        }

        protected Strategy(Workspace workspace, Record record)
        {
            this.Workspace = workspace;
            this.Id = record.Id;
            this.Class = record.Class;
            this._Record = record;
            this._IsPushed = false;
        }

        public long Version => this._Version;

        public Workspace Workspace { get; }

        IWorkspace IStrategy.Workspace => this.Workspace;

        public IClass Class { get; }

        public long Id { get; private set; }

        public bool IsNew => Workspace.IsNewId(this.Id);

        public IObject Object => this.@object ??= this.Workspace.Connection.Configuration.ObjectFactory.Create(this);

        public bool HasChanges => this._HashChanges();

        public void Reset() => this._Reset();

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

        public bool HasChanged(IRoleType roleType) => this.CanRead(roleType) && this._HasChanged(roleType);

        public void RestoreRole(IRoleType roleType)
        {
            if (this.CanRead(roleType))
            {
                this._RestoreRole(roleType);
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
            return this.CanRead(roleType) ? this._GetUnitRole(roleType) : null;
        }

        public T GetCompositeRole<T>(IRoleType roleType) where T : class, IObject
        {
            return this.CanRead(roleType)
                    ? (T)this._GetCompositeRole(roleType)?.Object
                    : null;
        }

        public IEnumerable<T> GetCompositesRole<T>(IRoleType roleType) where T : class, IObject
        {
            return this.CanRead(roleType)
                    ? this._GetCompositesRole(roleType).Select(v => (T)v.Object)
                    : Array.Empty<T>();
        }

        public void SetRole(IRoleType roleType, object value)
        {
            if (roleType.ObjectType.IsUnit)
            {
                this.SetUnitRole(roleType, value);
            }
            else if (roleType.IsOne)
            {
                this.SetCompositeRole(roleType, (IObject)value);
            }
            else
            {
                this.SetCompositesRole(roleType, (IEnumerable<IObject>)value);
            }
        }

        public void SetUnitRole(IRoleType roleType, object value)
        {
            AssertUnit(roleType, value);

            if (this.CanWrite(roleType))
            {
                this._SetUnitRole(roleType, value);
            }
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
                this._SetCompositeRole(roleType, (Strategy)value?.Strategy);
            }
        }

        public void SetCompositesRole<T>(IRoleType roleType, in IEnumerable<T> role) where T : class, IObject
        {
            this.AssertComposites(role);

            var roleStrategies = RefRange<Strategy>.Load(role?.Select(v => (Strategy)v.Strategy));

            if (this.CanWrite(roleType))
            {
                this._SetCompositesRole(roleType, roleStrategies);
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
                this._AddCompositesRole(roleType, (Strategy)value.Strategy);
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
                this._RemoveCompositesRole(roleType, (Strategy)value.Strategy);
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
            return (T)this.Workspace.GetCompositeAssociation(this, associationType)?.Object;
        }

        public IEnumerable<T> GetCompositesAssociation<T>(IAssociationType associationType) where T : class, IObject
        {
            return this.Workspace.GetCompositesAssociation(this, associationType).Select(v => v.Object).Cast<T>();
        }

        public bool CanRead(IRoleType roleType) => this._CanRead(roleType);

        public bool CanWrite(IRoleType roleType) => this._CanWrite(roleType);

        public bool CanExecute(IMethodType methodType) => this._CanExecute(methodType);

        public bool IsCompositeAssociationForRole(IRoleType roleType, Strategy forRole) => this._IsAssociationForRole(roleType, forRole);

        public bool IsCompositesAssociationForRole(IRoleType roleType, Strategy forRoleId) => this._IsAssociationForRole(roleType, forRoleId);

        public void OnPushNewId(long newId) => this.Id = newId;

        public void OnPushed() => this._OnPushed();

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

        int IComparable<Strategy>.CompareTo(Strategy other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return other is null ? 1 : this.Id.CompareTo(other.Id);
        }







        public long _Version => this._Record?.Version ?? Allors.Version.WorkspaceInitial;

        public bool _CanRead(IRoleType roleType)
        {
            if (!this._ExistRecord)
            {
                return true;
            }

            if (this._IsVersionInitial)
            {
                // TODO: Security
                return true;
            }

            var permission = this.Workspace.Connection.GetPermission(this.Class, roleType, Operations.Read);
            return this._Record.IsPermitted(permission);
        }

        public bool _CanWrite(IRoleType roleType)
        {
            if (this._IsVersionInitial)
            {
                return !this._IsPushed;
            }

            if (this._IsPushed)
            {
                return false;
            }

            if (!this._ExistRecord)
            {
                return true;
            }

            var permission = this.Workspace.Connection.GetPermission(this.Class, roleType, Operations.Write);
            return this._Record.IsPermitted(permission);
        }

        public bool _CanExecute(IMethodType methodType)
        {
            if (!this._ExistRecord)
            {
                return true;
            }

            if (this._IsVersionInitial)
            {
                // TODO: Security
                return true;
            }

            var permission = this.Workspace.Connection.GetPermission(this.Class, methodType, Operations.Execute);
            return this._Record.IsPermitted(permission);
        }

        private bool _IsVersionInitial => this._Version == Allors.Version.WorkspaceInitial.Value;

        protected IEnumerable<IRoleType> _RoleTypes => this.Class.RoleTypes;

        protected bool _ExistRecord => this._Record != null;

        protected Record _Record { get; private set; }

        private bool _IsPushed { get; set; }

        public void _OnPushed() => this._IsPushed = true;

        public void _OnPulled(IPullResultInternals pull)
        {
            var newRecord = this.Workspace.Connection.GetRecord(this.Id);

            if (!this._IsPushed)
            {
                if (!this._CanMerge(newRecord))
                {
                    pull.AddMergeError(this.Object);
                    return;
                }
            }
            else
            {
                this._Reset();
                this._IsPushed = false;
            }

            this._Record = newRecord;
        }

        public Dictionary<IRelationType, object> _ChangedRoleByRelationType { get; private set; }

        public object _GetUnitRole(IRoleType roleType)
        {
            if (this._ChangedRoleByRelationType != null && this._ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var role))
            {
                return role;
            }

            return this._Record?.GetRole(roleType);
        }

        public void _SetUnitRole(IRoleType roleType, object role) => this._SetChangedRole(roleType, role);

        public Strategy _GetCompositeRole(IRoleType roleType)
        {
            if (this._ChangedRoleByRelationType != null && this._ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return (Strategy)changedRole;
            }

            var role = this._Record?.GetRole(roleType);

            if (role == null)
            {
                return null;
            }

            var strategy = this.Workspace.GetStrategy((long)role);
            this._AssertStrategy(strategy);
            return strategy;
        }

        public void _SetCompositeRole(IRoleType roleType, Strategy role)
        {
            if (this._SameCompositeRole(roleType, role))
            {
                return;
            }

            var associationType = roleType.AssociationType;
            if (associationType.IsOne && role != null)
            {
                var previousAssociation = this.Workspace.GetCompositeAssociation(role, associationType);
                this._SetChangedRole(roleType, role);
                if (associationType.IsOne && previousAssociation != null)
                {
                    // OneToOne
                    previousAssociation.SetRole(roleType, null);
                }
            }
            else
            {
                this._SetChangedRole(roleType, role);
            }
        }

        public RefRange<Strategy> _GetCompositesRole(IRoleType roleType)
        {
            if (this._ChangedRoleByRelationType != null && this._ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return RefRange<Strategy>.Ensure(changedRole);
            }

            var role = ValueRange<long>.Ensure(this._Record?.GetRole(roleType));

            if (role.IsEmpty)
            {
                return RefRange<Strategy>.Empty;
            }

            return RefRange<Strategy>.Load(role.Select(v =>
            {
                var strategy = this.Workspace.GetStrategy(v);
                this._AssertStrategy(strategy);
                return strategy;
            }));
        }

        public void _AddCompositesRole(IRoleType roleType, Strategy roleToAdd)
        {
            var associationType = roleType.AssociationType;
            var previousAssociation = this.Workspace.GetCompositeAssociation(roleToAdd, associationType);

            var role = this._GetCompositesRole(roleType);

            if (role.Contains(roleToAdd))
            {
                return;
            }

            role = role.Add(roleToAdd);
            this._SetChangedRole(roleType, role);

            if (associationType.IsMany)
            {
                return;
            }

            // OneToMany
            previousAssociation?.SetRole(roleType, null);
        }

        public void _RemoveCompositesRole(IRoleType roleType, Strategy roleToRemove)
        {
            var role = this._GetCompositesRole(roleType);

            if (!role.Contains(roleToRemove))
            {
                return;
            }

            role = role.Remove(roleToRemove);
            this._SetChangedRole(roleType, role);
        }

        public void _SetCompositesRole(IRoleType roleType, RefRange<Strategy> role)
        {
            if (this._SameCompositesRole(roleType, role))
            {
                return;
            }

            var previousRole = this._GetCompositesRoleIfInstantiated(roleType);

            this._SetChangedRole(roleType, role);

            var associationType = roleType.AssociationType;
            if (associationType.IsMany)
            {
                return;
            }

            // OneToMany
            foreach (var addedRole in role.Except(previousRole))
            {
                var previousAssociation = this.Workspace.GetCompositeAssociation(addedRole, associationType);
                previousAssociation?.SetRole(roleType, null);
            }
        }

        public bool _CanMerge(Record newRecord)
        {
            if (this._ChangedRoleByRelationType == null)
            {
                return true;
            }

            foreach (var kvp in this._ChangedRoleByRelationType)
            {
                var relationType = kvp.Key;
                var roleType = relationType.RoleType;

                var original = this._Record?.GetRole(roleType);
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

        public bool _HashChanges() => this._ChangedRoleByRelationType != null;

        public void _Reset() => this._ChangedRoleByRelationType = null;

        public bool _IsAssociationForRole(IRoleType roleType, Strategy forRole)
        {
            if (roleType.IsOne)
            {
                var compositeRole = this._GetCompositeRoleIfInstantiated(roleType);
                return compositeRole == forRole;
            }

            var compositesRole = this._GetCompositesRoleIfInstantiated(roleType);
            return compositesRole.Contains(forRole);
        }

        public bool _HasChanged(IRoleType roleType) => this._ChangedRoleByRelationType?.ContainsKey(roleType.RelationType) ?? false;

        public void _RestoreRole(IRoleType roleType) => this._ChangedRoleByRelationType?.Remove(roleType.RelationType);

        private void _SetChangedRole(IRoleType roleType, object role)
        {
            this._ChangedRoleByRelationType ??= new Dictionary<IRelationType, object>();
            this._ChangedRoleByRelationType[roleType.RelationType] = role;
            this._OnChange();
        }

        private Strategy _GetCompositeRoleIfInstantiated(IRoleType roleType)
        {
            if (this._ChangedRoleByRelationType != null && this._ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return (Strategy)changedRole;
            }

            var role = this._Record?.GetRole(roleType);
            return role == null ? null : this.Workspace.GetStrategy((long)role);
        }

        private RefRange<Strategy> _GetCompositesRoleIfInstantiated(IRoleType roleType)
        {
            if (this._ChangedRoleByRelationType != null && this._ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return RefRange<Strategy>.Ensure(changedRole);
            }

            var role = ValueRange<long>.Ensure(this._Record?.GetRole(roleType));
            return role.IsEmpty ? RefRange<Strategy>.Empty : RefRange<Strategy>.Load(role.Select(this.Workspace.GetStrategy).Where(v => v != null));
        }

        private bool _SameCompositeRole(IRoleType roleType, Strategy role)
        {
            if (this._ChangedRoleByRelationType != null && this._ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return role == changedRole;
            }

            var changedRoleId = this._Record?.GetRole(roleType);

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

        private bool _SameCompositesRole(IRoleType roleType, RefRange<Strategy> role)
        {
            if (this._ChangedRoleByRelationType != null && this._ChangedRoleByRelationType.TryGetValue(roleType.RelationType, out var changedRole))
            {
                return role.Equals(changedRole);
            }

            var roleIds = ValueRange<long>.Ensure(this._Record?.GetRole(roleType));
            return role.IsEmpty ? roleIds.IsEmpty : role.Select(v => v.Id).SequenceEqual(roleIds);
        }

        private void _AssertStrategy(Strategy strategy)
        {
            if (strategy == null)
            {
                throw new Exception("Strategy is not in Workspace.");
            }
        }

        protected void _OnChange()
        {
            this.Workspace.PushToDatabaseTracker.OnChanged(this);
        }
    }
}
