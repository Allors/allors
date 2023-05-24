﻿// <copyright file="Object.cs" company="Allors bvba">
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

        IObject Object { get; }

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

        T GetCompositeRole<T>(IRoleType roleType) where T : class, IObject;

        void SetCompositeRole<T>(IRoleType roleType, T value) where T : class, IObject;

        IEnumerable<T> GetCompositesRole<T>(IRoleType roleType) where T : class, IObject;

        void AddCompositesRole<T>(IRoleType roleType, T value) where T : class, IObject;

        void RemoveCompositesRole<T>(IRoleType roleType, T value) where T : class, IObject;

        void SetCompositesRole<T>(IRoleType roleType, in IEnumerable<T> role) where T : class, IObject;

        T GetCompositeAssociation<T>(IAssociationType associationType) where T : class, IObject;

        IEnumerable<T> GetCompositesAssociation<T>(IAssociationType associationType) where T : class, IObject;
    }
}
