// <copyright file="Workspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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

        private readonly IDictionary<IRoleType, IDictionary<IStrategy, IRoleInternals>> roleByStrategyByRoleType;
        private readonly IDictionary<IAssociationType, IDictionary<IStrategy, IAssociationInternals>> associationByStrategyByAssociationType;
        private readonly IDictionary<IMethodType, IDictionary<IStrategy, IMethod>> methodByStrategyByMethodType;

        private readonly Queue<IReactiveInternals> reactives;

        protected Workspace(Connection connection, IWorkspaceServices services)
        {
            this.Connection = connection;
            this.Services = services;

            this.MetaPopulation = this.Connection.Configuration.MetaPopulation;

            this.StrategyById = new Dictionary<long, Strategy>();
            this.strategiesByClass = new Dictionary<IClass, ISet<Strategy>>();

            this.roleByStrategyByRoleType = new Dictionary<IRoleType, IDictionary<IStrategy, IRoleInternals>>();
            this.associationByStrategyByAssociationType = new Dictionary<IAssociationType, IDictionary<IStrategy, IAssociationInternals>>();
            this.methodByStrategyByMethodType = new Dictionary<IMethodType, IDictionary<IStrategy, IMethod>>();

            this.reactives = new Queue<IReactiveInternals>();

            this.PushToDatabaseTracker = new PushToDatabaseTracker();

            this.Services.OnInit(this);
        }

        public Connection Connection { get; }

        public IWorkspaceServices Services { get; }

        public IMetaPopulation MetaPopulation { get; }

        // TODO: Cache
        public bool HasChanges => this.StrategyById.Any(kvp => kvp.Value.HasChanges);

        public PushToDatabaseTracker PushToDatabaseTracker { get; }

        protected Dictionary<long, Strategy> StrategyById { get; }

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

        public IBinaryRole BinaryRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IBinaryRole)role;
            }

            role = new BinaryRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IBinaryRole)role;
        }

        public IBooleanRole BooleanRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IBooleanRole)role;
            }

            role = new BooleanRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IBooleanRole)role;
        }

        public IDateTimeRole DateTimeRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IDateTimeRole)role;
            }

            role = new DateTimeRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IDateTimeRole)role;
        }

        public IDecimalRole DecimalRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IDecimalRole)role;
            }

            role = new DecimalRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IDecimalRole)role;
        }

        public IFloatRole FloatRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IFloatRole)role;
            }

            role = new FloatRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IFloatRole)role;
        }

        public IIntegerRole IntegerRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IIntegerRole)role;
            }

            role = new IntegerRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IIntegerRole)role;
        }

        public IStringRole StringRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IStringRole)role;
            }

            role = new StringRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IStringRole)role;
        }

        public IUniqueRole UniqueRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (IUniqueRole)role;
            }

            role = new UniqueRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (IUniqueRole)role;
        }

        public ICompositeRole CompositeRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (ICompositeRole)role;
            }

            role = new CompositeRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (ICompositeRole)role;
        }

        public ICompositesRole CompositesRole(Strategy strategy, IRoleType roleType)
        {
            var roleByStrategy = this.GetRoleByStrategy(roleType);

            if (roleByStrategy.TryGetValue(strategy, out var role))
            {
                return (ICompositesRole)role;
            }

            role = new CompositesRole(strategy, roleType);
            roleByStrategy[strategy] = role;
            return (ICompositesRole)role;
        }

        public ICompositeAssociation CompositeAssociation(Strategy strategy, IAssociationType associationType)
        {
            var associationByStrategy = this.GetAssociationByStrategy(associationType);

            if (associationByStrategy.TryGetValue(strategy, out var association))
            {
                return (ICompositeAssociation)association;
            }

            association = new CompositeAssociation(strategy, associationType);
            associationByStrategy[strategy] = association;
            return (ICompositeAssociation)association;
        }

        public ICompositesAssociation CompositesAssociation(Strategy strategy, IAssociationType associationType)
        {
            var associationByStrategy = this.GetAssociationByStrategy(associationType);

            if (associationByStrategy.TryGetValue(strategy, out var association))
            {
                return (ICompositesAssociation)association;
            }

            association = new CompositesAssociation(strategy, associationType);
            associationByStrategy[strategy] = association;
            return (ICompositesAssociation)association;
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

        public void HandleReactions()
        {
            while (this.reactives.Count > 0)
            {
                var reactive = this.reactives.Dequeue();
                reactive.Reaction?.React();
            }
        }

        public void RegisterReactions(IEnumerable<IAssociationType> associationTypes)
        {
            foreach (var associationType in associationTypes)
            {
                if (this.associationByStrategyByAssociationType.TryGetValue(associationType, out var associationByStrategy))
                {
                    foreach (var kvp in associationByStrategy)
                    {
                        var association = kvp.Value;
                        this.reactives.Enqueue(association);
                    }
                }
            }
        }

        public void RegisterReaction(Strategy association, IRoleType roleType)
        {
            if (this.roleByStrategyByRoleType.TryGetValue(roleType, out var roleByStrategy))
            {
                if (roleByStrategy.TryGetValue(association, out var role))
                {
                    this.reactives.Enqueue(role);
                }
            }
        }

        public void RegisterReaction(Strategy role, IAssociationType associationType)
        {
            if (this.associationByStrategyByAssociationType.TryGetValue(associationType, out var associationByStrategy))
            {
                if (associationByStrategy.TryGetValue(role, out var association))
                {
                    this.reactives.Enqueue(association);
                }
            }
        }

        #region role, association and method
        private IDictionary<IStrategy, IRoleInternals> GetRoleByStrategy(IRoleType roleType)
        {
            if (this.roleByStrategyByRoleType.TryGetValue(roleType, out var roleByStrategy))
            {
                return roleByStrategy;
            }

            roleByStrategy = new Dictionary<IStrategy, IRoleInternals>();
            this.roleByStrategyByRoleType.Add(roleType, roleByStrategy);
            return roleByStrategy;
        }

        private IDictionary<IStrategy, IAssociationInternals> GetAssociationByStrategy(IAssociationType associationType)
        {
            if (this.associationByStrategyByAssociationType.TryGetValue(associationType, out var associationByStrategy))
            {
                return associationByStrategy;
            }

            associationByStrategy = new Dictionary<IStrategy, IAssociationInternals>();
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
    }
}
