// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Response
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;

    public interface IObject
    {
        IWorkspace Workspace { get; }

        IClass Class { get; }

        long Id { get; }

        long Version { get; }

        bool CanRead(IRoleType roleType);

        bool CanWrite(IRoleType roleType);

        bool CanExecute(IMethodType methodType);

        bool ExistRole(IRoleType roleType);

        object GetRole(IRoleType roleType);

        object GetUnitRole(IRoleType roleType);

        IObject GetCompositeRole(IRoleType roleType);

        IEnumerable<IObject> GetCompositesRole(IRoleType roleType);
    }
}
