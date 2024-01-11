// <copyright file="ServiceCollectionExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors.Workspace.Configuration
{
    public class FuncWorkspaceFactory : IWorkspaceFactory
    {
        private readonly Func<IWorkspace> func;

        public FuncWorkspaceFactory(Func<IWorkspace> func)
        {
            this.func = func;
        }

        public IWorkspace CreateWorkspace() => func();
    }
}
