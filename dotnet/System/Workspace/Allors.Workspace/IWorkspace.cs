﻿// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Meta;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Allors.Workspace.Data;

namespace Allors.Workspace
{
    public interface IWorkspace
    {
        IConfiguration Configuration { get; }

        IWorkspaceServices Services { get; }

        event EventHandler OnChange;

        bool HasChanges { get; }

        void Reset();

        T Create<T>() where T : class, IObject;

        T Create<T>(IClass @class) where T : class, IObject;

        #region Instantiate
        T Instantiate<T>(IObject @object) where T : class, IObject;

        T Instantiate<T>(T @object) where T : class, IObject;

        T Instantiate<T>(long? id) where T : class, IObject;

        T Instantiate<T>(long id) where T : class, IObject;

        T Instantiate<T>(string idAsString) where T : class, IObject;

        IEnumerable<T> Instantiate<T>(IEnumerable<IObject> objects) where T : class, IObject;

        IEnumerable<T> Instantiate<T>(IEnumerable<T> objects) where T : class, IObject;

        IEnumerable<T> Instantiate<T>(IEnumerable<long> ids) where T : class, IObject;

        IEnumerable<T> Instantiate<T>(IEnumerable<string> ids) where T : class, IObject;

        IEnumerable<T> Instantiate<T>() where T : class, IObject;

        IEnumerable<T> Instantiate<T>(IComposite objectType) where T : class, IObject;
        #endregion

        Task<IInvokeResult> InvokeAsync(Method method, InvokeOptions options = null);

        Task<IInvokeResult> InvokeAsync(Method[] methods, InvokeOptions options = null);

        Task<IPullResult> PullAsync(params Pull[] pull);

        Task<IPushResult> PushAsync();
    }
}
