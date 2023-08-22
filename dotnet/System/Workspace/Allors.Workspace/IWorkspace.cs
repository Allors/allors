// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Workspace.Meta;
using System.Collections.Generic;
using System.Threading.Tasks;
using Allors.Workspace.Data;

namespace Allors.Workspace
{
    public interface IWorkspace : INotifyChanged
    {
        IWorkspaceServices Services { get; }

        IMetaPopulation MetaPopulation { get; }

        bool HasModifications { get; }

        void Reset();

        IStrategy Create(IClass @class);

        #region Instantiate
        IStrategy Instantiate(IStrategy @object);

        IStrategy Instantiate(long? id);

        IStrategy Instantiate(long id);

        IStrategy Instantiate(string idAsString);

        IEnumerable<IStrategy> Instantiate(IEnumerable<IStrategy> objects);

        IEnumerable<IStrategy> Instantiate(IEnumerable<long> ids);

        IEnumerable<IStrategy> Instantiate(IEnumerable<string> ids);

        IEnumerable<IStrategy> Instantiate(IComposite objectType);
        #endregion

        Task<IInvokeResult> InvokeAsync(IMethod method, InvokeOptions options = null);

        Task<IInvokeResult> InvokeAsync(IMethod[] methods, InvokeOptions options = null);

        Task<IPullResult> PullAsync(params Pull[] pull);

        Task<IPushResult> PushAsync();
    }
}
