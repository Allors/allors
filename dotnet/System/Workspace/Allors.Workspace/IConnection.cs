// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Threading.Tasks;
    using Meta;
    using Request;
    using Response;

    public interface IConnection
    {
        string Name { get; }

        MetaPopulation MetaPopulation { get; }

        Task<IInvokeResult> InvokeAsync(MethodCall method, InvokeOptions options = null);

        Task<IInvokeResult> InvokeAsync(MethodCall[] methods, InvokeOptions options = null);

        Task<IPullResult> CallAsync(ProcedureCall procedureCall, params Pull[] pull);

        Task<IPullResult> CallAsync(object args, string name);

        Task<IPullResult> PullAsync(params Pull[] pull);
    }
}
