// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Response
{
    using System.Collections.Generic;
    using Meta;

    public interface IObject
    {
        IWorkspace Workspace { get; }

        Class Class { get; }

        long Id { get; }

        long Version { get; }

        bool CanRead(RoleType roleType);

        bool CanWrite(RoleType roleType);

        bool CanExecute(MethodType methodType);

        bool ExistRole(RoleType roleType);

        object GetRole(RoleType roleType);

        object GetUnitRole(RoleType roleType);

        IObject GetCompositeRole(RoleType roleType);

        IEnumerable<IObject> GetCompositesRole(RoleType roleType);
    }
}
