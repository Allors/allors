// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using Meta;

    public interface IStrategy
    {
        IWorkspace Workspace { get; }

        IClass Class { get; }

        long Id { get; }

        long Version { get; }

        bool IsNew { get; }

        bool IsDeleted { get; }

        bool HasChanges { get; }

        void Delete();

        bool CanRead(IRoleType roleType);

        bool CanWrite(IRoleType roleType);

        bool CanExecute(IMethodType methodType);

        bool ExistRole(IRoleType roleType);

        bool HasChanged(IRoleType roleType);

        void RestoreRole(IRoleType roleType);

        object GetRole(IRoleType roleType);

        void SetRole(IRoleType roleType, object role);

        void RemoveRole(IRoleType roleType);

        object GetUnitRole(IRoleType roleType);

        void SetUnitRole(IRoleType roleType, object role);

        IStrategy GetCompositeRole(IRoleType roleType);

        void SetCompositeRole(IRoleType roleType, IStrategy value);

        IEnumerable<IStrategy> GetCompositesRole(IRoleType roleType);

        void AddCompositesRole(IRoleType roleType, IStrategy value);

        void RemoveCompositesRole(IRoleType roleType, IStrategy value);

        void SetCompositesRole(IRoleType roleType, in IEnumerable<IStrategy> role);

        IStrategy GetCompositeAssociation(IAssociationType associationType);

        IEnumerable<IStrategy> GetCompositesAssociation(IAssociationType associationType);
    }
}
