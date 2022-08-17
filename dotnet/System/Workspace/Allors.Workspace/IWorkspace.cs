// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;
    using Shared.Ranges;

    public interface IWorkspace : INotifyWorkspaceChanged
    {
        bool CanRead(RoleType roleType);

        bool CanWrite(RoleType roleType);

        bool CanExecute(MethodType methodType);

        object GetUnitRole(long id, RoleType roleType);

        long GetCompositeRole(long id, RoleType roleType);

        ValueRange<long> GetCompositesRole(long id, RoleType roleTYpe);

        long GetCompositeAssociation(long id, AssociationType roleType);

        ValueRange<long> GetCompositesAssociation(long id, AssociationType roleTYpe);
    }
}
