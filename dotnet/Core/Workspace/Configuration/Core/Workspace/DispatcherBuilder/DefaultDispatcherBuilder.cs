// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using Signals.Default;

    public class DefaultDispatcherBuilder : IDispatcherBuilder
    {
        public IDispatcher Build(IWorkspace workspace)
        {
            return new Dispatcher(workspace);
        }
    }
}
