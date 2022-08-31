// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Threading.Tasks;
    using Data;
    using Meta;

    public interface IConnection
    {
        string Name { get; }

        MetaPopulation MetaPopulation { get; }

        Task<IInvokeResult> InvokeAsync(Method method, InvokeOptions options = null);

        Task<IInvokeResult> InvokeAsync(Method[] methods, InvokeOptions options = null);

        Task<IPullResult> CallAsync(Procedure procedure, params Pull[] pull);

        Task<IPullResult> CallAsync(object args, string name);

        Task<IPullResult> PullAsync(params Pull[] pull);
    }
}
