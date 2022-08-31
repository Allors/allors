// <copyright file="Workspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System.Threading.Tasks;
    using Data;
    using Meta;

    public abstract class Connection : IConnection
    {
        protected Connection(string name, MetaPopulation metaPopulation)
        {
            this.Name = name;
            this.MetaPopulation = metaPopulation;
        }

        public string Name { get; }

        public MetaPopulation MetaPopulation { get; }

        public abstract Task<IInvokeResult> InvokeAsync(Method method, InvokeOptions options = null);

        public abstract Task<IInvokeResult> InvokeAsync(Method[] methods, InvokeOptions options = null);

        public abstract Task<Allors.Workspace.IPullResult> CallAsync(Procedure procedure, params Pull[] pull);

        public abstract Task<Allors.Workspace.IPullResult> CallAsync(object args, string name);

        public abstract Task<Allors.Workspace.IPullResult> PullAsync(params Pull[] pull);

        public abstract Record GetRecord(long id);

        public abstract long GetPermission(Class @class, IOperandType operandType, Operations operation);
    }
}
