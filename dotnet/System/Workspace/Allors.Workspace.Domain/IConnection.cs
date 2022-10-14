// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Threading.Tasks;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request;
    using Allors.Workspace.Response;

    public interface IConnection
    {
        string Name { get; }

        MetaPopulation MetaPopulation { get; }

        Task<IInvokeResult> InvokeAsync(MethodRequest method, BatchOptions options = null);

        Task<IInvokeResult> InvokeAsync(MethodRequest[] methods, BatchOptions options = null);

        Task<IPullResult> PullAsync(params PullRequest[] pull);
    }
}
