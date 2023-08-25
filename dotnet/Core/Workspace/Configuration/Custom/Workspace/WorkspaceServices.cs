// <copyright file="IDatabaseScope.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using Configuration;
    using Meta;

    public partial class WorkspaceServices : IWorkspaceServices
    {
        public WorkspaceServices(IObjectFactory objectFactory, M m)
        {
            this.ObjectFactory = objectFactory;
            this.M = m;
            this.DispatcherBuilder = new DispatcherBuilder();
            this.ReactiveFuncBuilder = new ReactiveFuncBuilder();
        }

        public IObjectFactory ObjectFactory { get; }

        public M M { get; private set; }

        public IDispatcherBuilder DispatcherBuilder { get; }

        public IReactiveFuncBuilder ReactiveFuncBuilder { get; }

        public ITime Time { get; private set; }

        public void OnInit(IWorkspace workspace)
        {
            this.Time = new Time();
        }

        public void Dispose()
        {
        }

        public T Get<T>() =>
           typeof(T) switch
           {
               // Core
               { } type when type == typeof(M) => (T)this.M,
               { } type when type == typeof(IObjectFactory) => (T)this.ObjectFactory,
               { } type when type == typeof(IDispatcherBuilder) => (T)this.DispatcherBuilder,
               { } type when type == typeof(IReactiveFuncBuilder) => (T)this.ReactiveFuncBuilder,
               { } type when type == typeof(ITime) => (T)this.Time,
               _ => throw new NotSupportedException($"Service {typeof(T)} not supported")
           };
    }
}
