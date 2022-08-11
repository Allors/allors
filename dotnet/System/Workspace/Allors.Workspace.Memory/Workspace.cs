// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Memory
{
    using Meta;
    using Ranges;

    public sealed class Workspace : IWorkspace
    {
        public event WorkspaceChangedEventHandler WorkspaceChanged;

        public bool CanRead(RoleType roleType) => throw new System.NotImplementedException();

        public bool CanWrite(RoleType roleType) => throw new System.NotImplementedException();

        public bool CanExecute(MethodType methodType) => throw new System.NotImplementedException();

        public object GetUnitRole(long id, RoleType roleType) => throw new System.NotImplementedException();

        public long GetCompositeRole(long id, RoleType roleType) => throw new System.NotImplementedException();

        public IRange<long> GetCompositesRole(long id, RoleType roleTYpe) => throw new System.NotImplementedException();

        public long GetCompositeAssociation(long id, AssociationType roleType) => throw new System.NotImplementedException();

        public IRange<long> GetCompositesAssociation(long id, AssociationType roleTYpe) => throw new System.NotImplementedException();
    }
}
