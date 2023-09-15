﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using Signals;
    // TODO: Create fine grained signal implementation
    //using Signals.Fine;
    using Signals.Coarse;

    public class FineDispatcherBuilder : IDispatcherBuilder
    {
        public IDispatcher Build(IWorkspace workspace)
        {
            return new Dispatcher(workspace);
        }
    }
}
