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
        private readonly ISet<IConflict> conflicts;

        protected Workspace(Connection database, IWorkspaceServices services)
        {
            this.Connection = database;
            this.Services = services;

            this.StrategyByWorkspaceId = new Dictionary<long, Strategy>();
            this.strategiesByClass = new Dictionary<IClass, ISet<Strategy>>();

            this.conflicts = new HashSet<IConflict>();

            this.PushToDatabaseTracker = new PushToDatabaseTracker();

            this.Services.OnInit(this);
        }

        public Connection Connection { get; }

        public IWorkspaceServices Services { get; }

        public IMetaPopulation MetaPopulation => this.Connection.Configuration.MetaPopulation;

        public bool HasChanges => this.StrategyByWorkspaceId.Any(kvp => kvp.Value.HasChanges);

        public event EventHandler OnChange;

        public virtual void OnChanged(EventArgs e) => this.OnChange?.Invoke(this, e);

        public PushToDatabaseTracker PushToDatabaseTracker { get; }

        protected Dictionary<long, Strategy> StrategyByWorkspaceId { get; }

        public override string ToString() => $"workspace: {base.ToString()}";

        internal static bool IsNewId(long id) => id < 0;

        public void Reset()
        {
            // TODO: Optimize, only reset changed or created strategies
            foreach (var strategy in this.StrategyByWorkspaceId.Values.ToArray())
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

            return this.StrategyByWorkspaceId.TryGetValue(id, out var sessionStrategy) ? sessionStrategy : null;
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

        protected void OnDatabasePushResponseNew(long workspaceId, long databaseId)
        {
            var strategy = this.StrategyByWorkspaceId[workspaceId];
            this.PushToDatabaseTracker.Created.Remove(strategy);
            strategy.OnPushNewId(databaseId);
            this.AddStrategy(strategy);
            strategy.OnPushed();
        }

        private IEnumerable<Strategy> StrategiesForClass(IComposite objectType)
        {
            // TODO: Optimize
            var classes = new HashSet<IClass>(objectType.Classes);
            return this.StrategyByWorkspaceId.Where(v => classes.Contains(v.Value.Class)).Select(v => v.Value).Distinct();
        }

        public abstract Task<IInvokeResult> InvokeAsync(IMethod method, InvokeOptions options = null);

        public abstract Task<IInvokeResult> InvokeAsync(IMethod[] methods, InvokeOptions options = null);

        public abstract Task<IPullResult> PullAsync(params Pull[] pull);

        public abstract Task<IPushResult> PushAsync();
    }
}
