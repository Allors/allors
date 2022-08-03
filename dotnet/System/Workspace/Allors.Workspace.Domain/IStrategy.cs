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
        ISession Session { get; }

        IObject Object { get; }

        Class Class { get; }

        long Id { get; }

        long Version { get; }

        bool IsNew { get; }

        bool HasChanges { get; }

        void Reset();

        IReadOnlyList<IDiff> Diff();

        bool CanRead(RoleType roleType);

        bool CanWrite(RoleType roleType);

        bool CanExecute(MethodType methodType);

        bool ExistRole(RoleType roleType);

        bool HasChanged(RoleType roleType);

        void RestoreRole(RoleType roleType);

        object GetRole(RoleType roleType);

        void SetRole(RoleType roleType, object value);

        void RemoveRole(RoleType roleType);

        object GetUnitRole(RoleType roleType);

        void SetUnitRole(RoleType roleType, object value);

        T GetCompositeRole<T>(RoleType roleType) where T : class, IObject;

        void SetCompositeRole<T>(RoleType roleType, T value) where T : class, IObject;

        IEnumerable<T> GetCompositesRole<T>(RoleType roleType) where T : class, IObject;

        void AddCompositesRole<T>(RoleType roleType, T value) where T : class, IObject;

        void RemoveCompositesRole<T>(RoleType roleType, T value) where T : class, IObject;

        void SetCompositesRole<T>(RoleType roleType, in IEnumerable<T> role) where T : class, IObject;

        T GetCompositeAssociation<T>(AssociationType associationType) where T : class, IObject;

        IEnumerable<T> GetCompositesAssociation<T>(AssociationType associationType) where T : class, IObject;
    }
}
