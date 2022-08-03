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
    using Ranges;

    public abstract class Strategy : IStrategy, IComparable<Strategy>
    {
        private readonly long rangeId;

        private IObject @object;

        protected Strategy(Session session, Class @class, long id)
        {
            this.Session = session;
            this.Id = id;
            this.rangeId = this.Id;
            this.Class = @class;
            this.Ranges = this.Session.Workspace.StrategyRanges;
        }

        protected Strategy(Session session, DatabaseRecord databaseRecord)
        {
            this.Session = session;
            this.Id = databaseRecord.Id;
            this.rangeId = this.Id;
            this.Class = databaseRecord.Class;
            this.Ranges = this.Session.Workspace.StrategyRanges;
        }

        public long Version => this.DatabaseOriginState.Version;

        public Session Session { get; }

        public DatabaseOriginState DatabaseOriginState { get; protected set; }

        public IRanges<Strategy> Ranges { get; }

        ISession IStrategy.Session => this.Session;

        public Class Class { get; }

        public long Id { get; private set; }

        public bool IsNew => Session.IsNewId(this.Id);

        public IObject Object => this.@object ??= this.Session.Workspace.DatabaseConnection.Configuration.ObjectFactory.Create(this);
        public IReadOnlyList<IDiff> Diff()
        {
            var diffs = new List<IDiff>();
            this.DatabaseOriginState.Diff(diffs);
            return diffs.ToArray();
        }

        public bool HasChanges => this.DatabaseOriginState.HashChanges();

        public void Reset() => this.DatabaseOriginState.Reset();

        public bool ExistRole(RoleType roleType)
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

        public bool HasChanged(RoleType roleType) => this.CanRead(roleType) && this.DatabaseOriginState.HasChanged(roleType);

        public void RestoreRole(RoleType roleType)
        {
            if (this.CanRead(roleType))
            {
                this.DatabaseOriginState.RestoreRole(roleType);
            }
        }

        public object GetRole(RoleType roleType)
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

        public object GetUnitRole(RoleType roleType) => this.CanRead(roleType) ? this.DatabaseOriginState.GetUnitRole(roleType) : null;

        public T GetCompositeRole<T>(RoleType roleType) where T : class, IObject =>
            this.CanRead(roleType)
                ? (T)this.DatabaseOriginState.GetCompositeRole(roleType)?.Object
                : null;

        public IEnumerable<T> GetCompositesRole<T>(RoleType roleType) where T : class, IObject =>
            this.CanRead(roleType)
                ? this.DatabaseOriginState.GetCompositesRole(roleType).Select(v => (T)v.Object)
                : Array.Empty<T>();

        public void SetRole(RoleType roleType, object value)
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

        public void SetUnitRole(RoleType roleType, object value)
        {
            AssertUnit(roleType, value);

            if (this.CanWrite(roleType))
            {
                this.DatabaseOriginState.SetUnitRole(roleType, value);
            }
        }

        public void SetCompositeRole<T>(RoleType roleType, T value) where T : class, IObject
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
                this.DatabaseOriginState.SetCompositeRole(roleType, (Strategy)value?.Strategy);
            }
        }

        public void SetCompositesRole<T>(RoleType roleType, in IEnumerable<T> role) where T : class, IObject
        {
            this.AssertComposites(role);

            var roleStrategies = this.Ranges.Load(role?.Select(v => (Strategy)v.Strategy));

            if (this.CanWrite(roleType))
            {
                this.DatabaseOriginState.SetCompositesRole(roleType, roleStrategies);
            }
        }

        public void AddCompositesRole<T>(RoleType roleType, T value) where T : class, IObject
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
                this.DatabaseOriginState.AddCompositesRole(roleType, (Strategy)value.Strategy);
            }
        }

        public void RemoveCompositesRole<T>(RoleType roleType, T value) where T : class, IObject
        {
            if (value == null)
            {
                return;
            }

            this.AssertComposite(value);

            if (this.CanWrite(roleType))
            {
                this.DatabaseOriginState.RemoveCompositesRole(roleType, (Strategy)value.Strategy);
            }
        }

        public void RemoveRole(RoleType roleType)
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

        public T GetCompositeAssociation<T>(AssociationType associationType) where T : class, IObject => (T)this.Session.GetCompositeAssociation(this, associationType)?.Object;

        public IEnumerable<T> GetCompositesAssociation<T>(AssociationType associationType) where T : class, IObject => this.Session.GetCompositesAssociation(this, associationType).Select(v => v.Object).Cast<T>();

        public bool CanRead(RoleType roleType) => this.DatabaseOriginState.CanRead(roleType);

        public bool CanWrite(RoleType roleType) => this.DatabaseOriginState.CanWrite(roleType);

        public bool CanExecute(MethodType methodType) => this.DatabaseOriginState.CanExecute(methodType);

        public bool IsCompositeAssociationForRole(RoleType roleType, Strategy forRole) => this.DatabaseOriginState.IsAssociationForRole(roleType, forRole);

        public bool IsCompositesAssociationForRole(RoleType roleType, Strategy forRoleId) => this.DatabaseOriginState.IsAssociationForRole(roleType, forRoleId);

        public void OnDatabasePushNewId(long newId) => this.Id = newId;

        public void OnDatabasePushed() => this.DatabaseOriginState.OnPushed();

        private void AssertSameType<T>(RoleType roleType, T value) where T : class, IObject
        {
            if (!((IComposite)roleType.ObjectType).IsAssignableFrom(value.Strategy.Class))
            {
                throw new ArgumentException($"Types do not match: {nameof(roleType)}: {roleType.ObjectType.ClrType} and {nameof(value)}: {value.GetType()}");
            }
        }

        private void AssertSameSession(IObject value)
        {
            if (this.Session != value.Strategy.Session)
            {
                throw new ArgumentException($"Session do not match");
            }
        }

        private static void AssertUnit(RoleType roleType, object value)
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

            if (this.Session != value.Strategy.Session)
            {
                throw new ArgumentException("Strategy is from a different session");
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

            return other is null ? 1 : this.rangeId.CompareTo(other.rangeId);
        }
    }
}
