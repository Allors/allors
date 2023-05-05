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


        protected Workspace(DatabaseConnection database, IWorkspaceServices services)
        {
            this.DatabaseConnection = database;
            this.Services = services;


            this.StrategyByWorkspaceId = new Dictionary<long, Strategy>();
            this.strategiesByClass = new Dictionary<IClass, ISet<Strategy>>();

            this.ChangeSetTracker = new ChangeSetTracker(this);
            this.PushToDatabaseTracker = new PushToDatabaseTracker();

            this.Services.OnInit(this);
        }

        public DatabaseConnection DatabaseConnection { get; }

        public IConfiguration Configuration => this.DatabaseConnection.Configuration;

        public IWorkspaceServices Services { get; }

        public bool HasChanges => this.StrategyByWorkspaceId.Any(kvp => kvp.Value.HasChanges);

        public event EventHandler OnChange;

        public virtual void OnChanged(EventArgs e) => this.OnChange?.Invoke(this, e);

        public ChangeSetTracker ChangeSetTracker { get; }

        public PushToDatabaseTracker PushToDatabaseTracker { get; }

        protected Dictionary<long, Strategy> StrategyByWorkspaceId { get; }

        public override string ToString() => $"session: {base.ToString()}";

        internal static bool IsNewId(long id) => id < 0;

        public void Reset()
        {
            var changeSet = this.Checkpoint();

            var strategies = new HashSet<IStrategy>(changeSet.Created);

            foreach (var roles in changeSet.RolesByAssociationType.Values)
            {
                strategies.UnionWith(roles);
            }

            foreach (var associations in changeSet.AssociationsByRoleType.Values)
            {
                strategies.UnionWith(associations);
            }

            //TODO: Koen, fix strategy = null
            foreach (var strategy in strategies.Where(v => v != null))
            {
                strategy.Reset();
            }
        }

        public abstract T Create<T>(IClass @class) where T : class, IObject;

        public T Create<T>() where T : class, IObject => this.Create<T>((IClass)this.DatabaseConnection.Configuration.ObjectFactory.GetObjectType<T>());
        public IChangeSet Checkpoint()
        {
            var changeSet = new ChangeSet(this, this.ChangeSetTracker.Created, this.ChangeSetTracker.Instantiated);

            if (this.ChangeSetTracker.DatabaseOriginStates != null)
            {
                foreach (var databaseOriginState in this.ChangeSetTracker.DatabaseOriginStates)
                {
                    databaseOriginState.Checkpoint(changeSet);
                }
            }

            this.ChangeSetTracker.Created = null;
            this.ChangeSetTracker.Instantiated = null;
            this.ChangeSetTracker.DatabaseOriginStates = null;

            return changeSet;
        }

        #region Instantiate
        public T Instantiate<T>(IObject @object) where T : class, IObject => this.Instantiate<T>(@object.Id);

        public T Instantiate<T>(T @object) where T : class, IObject => this.Instantiate<T>(@object.Id);

        public T Instantiate<T>(long? id) where T : class, IObject => id.HasValue ? this.Instantiate<T>((long)id) : default;

        public T Instantiate<T>(long id) where T : class, IObject => (T)this.GetStrategy(id)?.Object;

        public T Instantiate<T>(string idAsString) where T : class, IObject => long.TryParse(idAsString, out var id) ? (T)this.GetStrategy(id)?.Object : default;

        public IEnumerable<T> Instantiate<T>(IEnumerable<IObject> objects) where T : class, IObject => objects.Select(this.Instantiate<T>);

        public IEnumerable<T> Instantiate<T>(IEnumerable<T> objects) where T : class, IObject => objects.Select(this.Instantiate);

        public IEnumerable<T> Instantiate<T>(IEnumerable<long> ids) where T : class, IObject => ids.Select(this.Instantiate<T>);

        public IEnumerable<T> Instantiate<T>(IEnumerable<string> ids) where T : class, IObject => this.Instantiate<T>(ids.Select(
            v =>
            {
                long.TryParse(v, out var id);
                return id;
            }));

        public IEnumerable<T> Instantiate<T>() where T : class, IObject
        {
            var objectType = (IComposite)this.DatabaseConnection.Configuration.ObjectFactory.GetObjectType<T>();
            return this.Instantiate<T>(objectType);
        }

        public IEnumerable<T> Instantiate<T>(IComposite objectType) where T : class, IObject
        {
            foreach (var @class in objectType.Classes)
            {
                if (this.strategiesByClass.TryGetValue(@class, out var strategies))
                {
                    foreach (var strategy in strategies)
                    {
                        yield return (T)strategy.Object;
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

            return this.StrategyByWorkspaceId.TryGetValue(id, out var sessionStrategy) ? sessionStrategy : null;
        }

        public Strategy GetCompositeAssociation(Strategy role, IAssociationType associationType)
        {
            var roleType = associationType.RelationType.RoleType;

            foreach (var association in this.StrategiesForClass(associationType.ObjectType))
            {
                if (!association.CanRead(roleType))
                {
                    continue;
                }

                if (association.IsCompositeAssociationForRole(roleType, role))
                {
                    return association;
                }
            }

            return null;
        }

        public IEnumerable<Strategy> GetCompositesAssociation(Strategy role, IAssociationType associationType)
        {
            var roleType = associationType.RelationType.RoleType;

            foreach (var association in this.StrategiesForClass(associationType.ObjectType))
            {
                if (!association.CanRead(roleType))
                {
                    continue;
                }

                if (association.IsCompositesAssociationForRole(roleType, role))
                {
                    yield return association;
                }
            }
        }

        protected void AddStrategy(Strategy strategy)
        {
            this.StrategyByWorkspaceId.Add(strategy.Id, strategy);

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

        public void OnDatabasePushResponseNew(long workspaceId, long databaseId)
        {
            var strategy = this.StrategyByWorkspaceId[workspaceId];
            this.PushToDatabaseTracker.Created.Remove(strategy);
            strategy.OnDatabasePushNewId(databaseId);
            this.AddStrategy(strategy);
            strategy.OnDatabasePushed();
        }

        private IEnumerable<Strategy> StrategiesForClass(IComposite objectType)
        {
            // TODO: Optimize
            var classes = new HashSet<IClass>(objectType.Classes);
            return this.StrategyByWorkspaceId.Where(v => classes.Contains(v.Value.Class)).Select(v => v.Value).Distinct();
        }

        public abstract Task<IInvokeResult> InvokeAsync(Method method, InvokeOptions options = null);
        public abstract Task<IInvokeResult> InvokeAsync(Method[] methods, InvokeOptions options = null);
        public abstract Task<IPullResult> CallAsync(object args, string name);
        public abstract Task<IPullResult> PullAsync(params Pull[] pull);
        public abstract Task<IPushResult> PushAsync();
    }
}
