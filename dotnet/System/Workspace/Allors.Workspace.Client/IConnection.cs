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

        IMetaPopulation MetaPopulation { get; }

        Task<IResponse> ExecuteAsync(IRequest request);
    }
}
