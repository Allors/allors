// <copyright file="Workspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System.Threading.Tasks;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request;
    using Allors.Workspace.Response;

    public abstract class Connection : IConnection
    {
        protected Connection(string name, MetaPopulation metaPopulation)
        {
            this.Name = name;
            this.MetaPopulation = metaPopulation;
        }

        public string Name { get; }

        public MetaPopulation MetaPopulation { get; }

        public abstract Task<IInvokeResult> InvokeAsync(MethodRequest method, BatchOptions options = null);

        public abstract Task<IInvokeResult> InvokeAsync(MethodRequest[] methods, BatchOptions options = null);

        public abstract Task<IPullResult> PullAsync(params PullRequest[] pull);

        public abstract Record GetRecord(long id);

        public abstract long GetPermission(Class @class, IOperandType operandType, Operations operation);
    }
}
