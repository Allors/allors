// <copyright file="Test.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using System.Threading.Tasks;
    using Allors.Workspace;
    using Xunit;

    public interface IProfile : IAsyncLifetime
    {
        IWorkspaceConnection CreateExclusiveWorkspaceConnection();

        IWorkspaceConnection CreateWorkspaceConnection();

        IWorkspaceConnection WorkspaceConnection { get; }

        Task Login(string userName);
    }
}
