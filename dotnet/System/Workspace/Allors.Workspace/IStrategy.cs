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

        IClass Class { get; }

        long Id { get; }

        long Version { get; }

        bool IsNew { get; }

        bool IsDeleted { get; }

        bool HasChanges { get; }

        void Delete();

        IRole Role(IRoleType roleType);

        IUnitRole UnitRole(IRoleType roleType);

        IBinaryRole BinaryRole(IRoleType roleType);

        IBooleanRole BooleanRole(IRoleType roleType);

        IDateTimeRole DateTimeRole(IRoleType roleType);

        IDecimalRole DecimalRole(IRoleType roleType);

        IFloatRole FloatRole(IRoleType roleType);

        IIntegerRole IntegerRole(IRoleType roleType);

        IStringRole StringRole(IRoleType roleType);

        IUniqueRole UniqueRole(IRoleType roleType);
        
        ICompositeRole CompositeRole(IRoleType roleType);

        ICompositesRole CompositesRole(IRoleType roleType);

        IAssociation Association(IAssociationType associationType);

        ICompositeAssociation CompositeAssociation(IAssociationType associationType);

        ICompositesAssociation CompositesAssociation(IAssociationType associationType);

        IMethod Method(IMethodType methodType);

        #region Deprecated
        bool CanRead(IRoleType roleType);

        bool CanWrite(IRoleType roleType);

        bool CanExecute(IMethodType methodType);

        bool ExistRole(IRoleType roleType);

        bool IsModified(IRoleType roleType);

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
        #endregion
    }
}
