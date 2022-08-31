// <copyright file="RemoteSession.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System.Collections.Generic;
    using Meta;

    public abstract class Workspace : IWorkspace
    {
        private readonly Dictionary<Class, ISet<Strategy>> strategiesByClass;

        protected Workspace(Connection connection)
        {
            this.Connection = connection;
            this.StrategyByWorkspaceId = new Dictionary<long, Strategy>();
            this.strategiesByClass = new Dictionary<Class, ISet<Strategy>>();
        }

        IConnection IWorkspace.Connection => this.Connection;

        public IEnumerable<IObject> Objects => this.StrategyByWorkspaceId.Values;

        public Connection Connection { get; }

        protected Dictionary<long, Strategy> StrategyByWorkspaceId { get; }

        public override string ToString() => $"session: {base.ToString()}";


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


    }
}
