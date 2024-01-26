// <copyright file="Workspace.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allors.Workspace.Data;

namespace Allors.Workspace.Adapters
{
    public abstract class Workspace : IWorkspace
    {
        private readonly Dictionary<IClass, ISet<Strategy>> strategiesByClass;

        private readonly IDictionary<IRoleType, IDictionary<IStrategy, IRole>> roleByStrategyByRoleType;
        private readonly IDictionary<IAssociationType, IDictionary<IStrategy, IAssociation>> associationByStrategyByAssociationType;
        private readonly IDictionary<IMethodType, IDictionary<IStrategy, IMethod>> methodByStrategyByMethodType;

        private readonly Dictionary<IOperand, IChangeDetector> changeDetectorByOperand;
        private readonly HashSet<IChangeDetector> changeDetectors;
        private readonly ChangedEventArgs changedEventArgs;
        private bool isHandlingChanges;

        protected Workspace(Connection connection, IWorkspaceServices services)
        {
            this.Connection = connection;
            this.Services = services;

            this.MetaPopulation = this.Connection.Configuration.MetaPopulation;

            this.StrategyById = new Dictionary<long, Strategy>();
            this.strategiesByClass = new Dictionary<IClass, ISet<Strategy>>();

            this.roleByStrategyByRoleType = new Dictionary<IRoleType, IDictionary<IStrategy, IRole>>();
            this.associationByStrategyByAssociationType = new Dictionary<IAssociationType, IDictionary<IStrategy, IAssociation>>();
            this.methodByStrategyByMethodType = new Dictionary<IMethodType, IDictionary<IStrategy, IMethod>>();

            this.PushToDatabaseTracker = new PushToDatabaseTracker();

            this.changedEventArgs = new ChangedEventArgs(this);

            this.isHandlingChanges = false;
            this.changeDetectorByOperand = new Dictionary<IOperand, IChangeDetector>();
            this.changeDetectors = new HashSet<IChangeDetector>();

            this.Services.OnInit(this);

            this.ObjectFactory = this.Services.Get<IObjectFactory>();
        }

        public event ChangedEventHandler Changed;

        public IObjectFactory ObjectFactory { get; set; }

        public Connection Connection { get; }

        public IWorkspaceServices Services { get; }

        public IMetaPopulation MetaPopulation { get; }

        // TODO: Cache
        public bool HasModifications => this.StrategyById.Any(kvp => kvp.Value.HasChanges);

        public PushToDatabaseTracker PushToDatabaseTracker { get; }

        public long Version { get; private set; }

        protected Dictionary<long, Strategy> StrategyById { get; }

        public IDisposable Subscribe(IObserver<IWorkspace> observer)
        {
        }

        public override string ToString() => $"workspace: {base.ToString()}";

        internal static bool IsNewId(long id) => id < 0;

        public void Reset()
        {
            // TODO: Optimize, only reset changed or created strategies
            foreach (var strategy in this.StrategyById.Values.ToArray())
            {
                strategy.Reset();
            }
        }

        public abstract IStrategy Create(IClass @class);

        #region Instantiate
        public IStrategy Instantiate(IStrategy @object) => this.Instantiate(@object.Id);

        public IStrategy Instantiate(long? id) => id.HasValue ? this.Instantiate((long)id) : default;

        public IStrategy Instantiate(long id) => this.GetStrategy(id);

        public IStrategy Instantiate(string idAsString) => long.TryParse(idAsString, out var id) ? this.GetStrategy(id) : default;

        public IEnumerable<IStrategy> Instantiate(IEnumerable<IStrategy> objects) => objects != null ? objects.Select(this.Instantiate) : Array.Empty<IStrategy>();

        public IEnumerable<IStrategy> Instantiate(IEnumerable<long> ids) => ids != null ? ids.Select(this.Instantiate) : Array.Empty<IStrategy>();

        public IEnumerable<IStrategy> Instantiate(IEnumerable<string> ids) => ids != null ? this.Instantiate(ids.Select(
            v =>
            {
                long.TryParse(v, out var id);
                return id;
            })) : Array.Empty<IStrategy>();

        public IEnumerable<IStrategy> Instantiate(IComposite objectType)
        {
            foreach (var @class in objectType.Classes)
            {
                if (this.strategiesByClass.TryGetValue(@class, out var strategies))
                {
                    foreach (var strategy in strategies)
                    {
                        yield return strategy;
                    }
                }
            }
        }

        #endregion

        public Strategy GetStrategy(long id)
        {
            if (id == 0)
            {
                return null;
            }

            return this.StrategyById.TryGetValue(id, out var strategy) ? strategy : null;
        }

        protected void AddStrategy(Strategy strategy)
        {
            this.StrategyById.Add(strategy.Id, strategy);

            var @class = strategy.Class;
            if (!this.strategiesByClass.TryGetValue(@class, out var strategies))
            {
                this.strategiesByClass[@class] = new HashSet<Strategy> { strategy };
            }
            else
            {
                strategies.Add(strategy);
            }
        }

        protected void OnDatabasePushResponseNew(long workspaceId, long databaseId)
        {
            var strategy = this.StrategyById[workspaceId];
            this.PushToDatabaseTracker.Created.Remove(strategy);
            strategy.OnPushNewId(databaseId);
            this.AddStrategy(strategy);
            strategy.OnPushed();
        }

        public abstract Task<IInvokeResult> InvokeAsync(IMethod method, InvokeOptions options = null);

        public abstract Task<IInvokeResult> InvokeAsync(IMethod[] methods, InvokeOptions options = null);

        public abstract Task<IPullResult> PullAsync(params Pull[] pull);

        public abstract Task<IPushResult> PushAsync();

        public IUnitRole<byte[]> BinaryRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IUnitRole<byte[]>)role;
            }

            role = new UnitRole<byte[]>(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IUnitRole<byte[]>)role;
        }

        public IUnitRole<bool?> BooleanRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IUnitRole<bool?>)role;
            }

            role = new UnitRole<bool?>(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IUnitRole<bool?>)role;
        }

        public IUnitRole<DateTime?> DateTimeRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IUnitRole<DateTime?>)role;
            }

            role = new UnitRole<DateTime?>(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IUnitRole<DateTime?>)role;
        }

        public IUnitRole<decimal?> DecimalRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IUnitRole<decimal?>)role;
            }

            role = new UnitRole<decimal?>(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IUnitRole<decimal?>)role;
        }

        public IUnitRole<double?> FloatRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IUnitRole<double?>)role;
            }

            role = new UnitRole<double?>(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IUnitRole<double?>)role;
        }

        public IUnitRole<int?> IntegerRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IUnitRole<int?>)role;
            }

            role = new UnitRole<int?>(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IUnitRole<int?>)role;
        }

        public IUnitRole<string> StringRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IUnitRole<string>)role;
            }

            role = new UnitRole<string>(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IUnitRole<string>)role;
        }

        public IUnitRole<Guid?> UniqueRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IUnitRole<Guid?>)role;
            }

            role = new UnitRole<Guid?>(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IUnitRole<Guid?>)role;
        }

        public ICompositeRole CompositeRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (ICompositeRole)role;
            }

            role = this.ObjectFactory.CompositeRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (ICompositeRole)role;
        }

        public CompositeRole<T> CompositeRole<T>(Strategy strategy, IRoleType roleType)
            where T : class, IObject
        {
            return (CompositeRole<T>)this.CompositeRole(strategy, roleType);
        }

        public ICompositesRole CompositesRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (ICompositesRole)role;
            }

            role = this.ObjectFactory.CompositesRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (ICompositesRole)role;
        }

        public CompositesRole<T> CompositesRole<T>(Strategy strategy, IRoleType roleType)
            where T : class, IObject
        {
            return (CompositesRole<T>)this.CompositesRole(strategy, roleType);
        }

        public ICompositeAssociation CompositeAssociation(Strategy strategy, IAssociationType associationType)
        {
            var associationByStrategy = this.GetAssociationByStrategy(associationType);

            if (associationByStrategy.TryGetValue(strategy, out var association))
            {
                return (ICompositeAssociation)association;
            }

            association = this.ObjectFactory.CompositeAssociation(strategy, associationType);
            associationByStrategy[strategy] = association;
            return (ICompositeAssociation)association;
        }

        public CompositeAssociation<T> CompositeAssociation<T>(Strategy strategy, IAssociationType associationType)
            where T : class, IObject
        {
            return (CompositeAssociation<T>)this.CompositeAssociation(strategy, associationType);
        }

        public ICompositesAssociation CompositesAssociation(Strategy strategy, IAssociationType associationType)
        {
            var associationByStrategy = this.GetAssociationByStrategy(associationType);

            if (associationByStrategy.TryGetValue(strategy, out var association))
            {
                return (ICompositesAssociation)association;
            }

            association = this.ObjectFactory.CompositesAssociation(strategy, associationType);
            associationByStrategy[strategy] = association;
            return (ICompositesAssociation)association;
        }

        public CompositesAssociation<T> CompositesAssociation<T>(Strategy strategy, IAssociationType associationType)
            where T : class, IObject
        {
            return (CompositesAssociation<T>)this.CompositesAssociation(strategy, associationType);
        }

        public IMethod Method(Strategy strategy, IMethodType methodType)
        {
            var methodByStrategy = this.GetMethodByStrategy(methodType);

            if (methodByStrategy.TryGetValue(strategy, out var method))
            {
                return method;
            }

            method = new Method(strategy, methodType);
            methodByStrategy[strategy] = method;
            return method;
        }

        #region signals

        public void Add(Method method, ChangedEventHandler handler)
        {
            if (!this.changeDetectorByOperand.TryGetValue(method, out var signaler))
            {
                signaler = new MethodChangeDetector(method);
                this.changeDetectorByOperand[method] = signaler;
            }

            signaler.Changed += handler;
        }

        public void Add<T>(UnitRole<T> unitRole, ChangedEventHandler handler)
        {
            if (!this.changeDetectorByOperand.TryGetValue(unitRole, out var signaler))
            {
                signaler = new UnitRoleChangeDetector(unitRole);
                this.changeDetectorByOperand[unitRole] = signaler;
            }

            signaler.Changed += handler;
        }

        public void Add<T>(CompositeRole<T> compositeRole, ChangedEventHandler handler) where T : class, IObject
        {

            if (!this.changeDetectorByOperand.TryGetValue(compositeRole, out var signaler))
            {
                signaler = new CompositeRoleChangeDetector(compositeRole);
                this.changeDetectorByOperand[compositeRole] = signaler;
            }

            signaler.Changed += handler;
        }

        public void Add<T>(CompositesRole<T> compositesRole, ChangedEventHandler handler) where T : class, IObject
        {
            if (!this.changeDetectorByOperand.TryGetValue(compositesRole, out var signaler))
            {
                signaler = new CompositesRoleChangeDetector(compositesRole);
                this.changeDetectorByOperand[compositesRole] = signaler;
            }

            signaler.Changed += handler;
        }

        public void Add<T>(CompositeAssociation<T> compositeAssociation, ChangedEventHandler handler) where T : class, IObject
        {
            if (!this.changeDetectorByOperand.TryGetValue(compositeAssociation, out var signaler))
            {
                signaler = new CompositeAssociationChangeDetector(compositeAssociation);
                this.changeDetectorByOperand[compositeAssociation] = signaler;
            }

            signaler.Changed += handler;
        }

        public void Add<T>(CompositesAssociation<T> compositesAssociation, ChangedEventHandler handler) where T : class, IObject
        {
            if (!this.changeDetectorByOperand.TryGetValue(compositesAssociation, out var signaler))
            {
                signaler = new CompositesAssociationChangeDetector(compositesAssociation);
                this.changeDetectorByOperand[compositesAssociation] = signaler;
            }

            signaler.Changed += handler;
        }

        public void Remove(IOperand operand, ChangedEventHandler handler)
        {
            if (this.changeDetectorByOperand.TryGetValue(operand, out var signaler))
            {
                signaler.Changed -= handler;
                if (!signaler.HasHandlers)
                {
                    this.changeDetectors.Remove(signaler);
                    this.changeDetectorByOperand.Remove(operand);
                }
            }
        }

        public void OnPull()
        {
            this.changeDetectors.UnionWith(this.changeDetectorByOperand.Values);

            this.Signal();
        }

        public void OnModifiedOperands()
        {
            this.Signal();
        }

        public void RegisterModifiedOperand(Strategy association, IRoleType roleType)
        {
            var role = this.Role(association, roleType);

            if (this.changeDetectorByOperand.TryGetValue(role, out var signaler))
            {
                this.changeDetectors.Add(signaler);
            }
        }

        public void RegisterModifiedOperand(Strategy role, IAssociationType associationType)
        {
            var association = this.Association(role, associationType);

            if (this.changeDetectorByOperand.TryGetValue(association, out var signaler))
            {
                this.changeDetectors.Add(signaler);
            }
        }

        private void Signal()
        {
            if (this.isHandlingChanges)
            {
                return;
            }

            this.isHandlingChanges = true;

            try
            {
                IChangeDetector changeDetector;

                do
                {
                    changeDetector = this.changeDetectors.FirstOrDefault();
                    if (changeDetector != null)
                    {
                        this.changeDetectors.Remove(changeDetector);
                        changeDetector.Handle();
                    }

                } while (changeDetector != null);

            }
            finally
            {
                this.isHandlingChanges = false;
            }


            this.Changed?.Invoke(this, this.changedEventArgs);
        }
        #endregion

        #region role, association and method
        private IOperand Role(Strategy association, IRoleType roleType)
        {
            var objectType = roleType.ObjectType;

            if (objectType.IsUnit)
            {
                return roleType.ObjectType.Tag switch
                {
                    UnitTags.Binary => this.BinaryRole(association, roleType),
                    UnitTags.Boolean => this.BooleanRole(association, roleType),
                    UnitTags.DateTime => this.DateTimeRole(association, roleType),
                    UnitTags.Decimal => this.DecimalRole(association, roleType),
                    UnitTags.Float => this.FloatRole(association, roleType),
                    UnitTags.Integer => this.IntegerRole(association, roleType),
                    UnitTags.String => this.StringRole(association, roleType),
                    UnitTags.Unique => this.UniqueRole(association, roleType),
                    _ => throw new Exception("Unknown unit role")
                };
            }

            if (roleType.IsOne)
            {
                return CompositeRole(association, roleType);
            }

            return CompositesRole(association, roleType);
        }

        private IOperand Association(Strategy role, IAssociationType associationType)
        {
            if (associationType.IsOne)
            {
                return this.CompositeAssociation(role, associationType);
            }

            return this.CompositesAssociation(role, associationType);
        }

        private IDictionary<IStrategy, IRole> GetRoleByStrategy(IRoleType roleType)
        {
            if (this.roleByStrategyByRoleType.TryGetValue(roleType, out var roleByStrategy))
            {
                return roleByStrategy;
            }

            roleByStrategy = new Dictionary<IStrategy, IRole>();
            this.roleByStrategyByRoleType.Add(roleType, roleByStrategy);
            return roleByStrategy;
        }

        private IDictionary<IStrategy, IAssociation> GetAssociationByStrategy(IAssociationType associationType)
        {
            if (this.associationByStrategyByAssociationType.TryGetValue(associationType, out var associationByStrategy))
            {
                return associationByStrategy;
            }

            associationByStrategy = new Dictionary<IStrategy, IAssociation>();
            this.associationByStrategyByAssociationType.Add(associationType, associationByStrategy);
            return associationByStrategy;
        }

        private IDictionary<IStrategy, IMethod> GetMethodByStrategy(IMethodType methodType)
        {
            if (this.methodByStrategyByMethodType.TryGetValue(methodType, out var methodByStrategy))
            {
                return methodByStrategy;
            }

            methodByStrategy = new Dictionary<IStrategy, IMethod>();
            this.methodByStrategyByMethodType.Add(methodType, methodByStrategy);
            return methodByStrategy;
        }

        #endregion

        internal IDisposable Subscribe(IObservable<IOperand> observable, IObserver<IOperand> observer)
        {
            throw new NotImplementedException();
        }
    }
}
