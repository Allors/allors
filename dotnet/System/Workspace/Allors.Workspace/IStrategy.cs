// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
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

        IUnitRole<T> UnitRole<T>(IRoleType roleType);

        ICompositeRole CompositeRole(IRoleType roleType);

        ICompositeRole<T> CompositeRole<T>(IRoleType roleType) where T : class, IObject;

        ICompositesRole CompositesRole(IRoleType roleType);

        ICompositesRole<T> CompositesRole<T>(IRoleType roleType) where T : class, IObject;

        IAssociation Association(IAssociationType associationType);

        ICompositeAssociation CompositeAssociation(IAssociationType associationType);

        ICompositeAssociation<T> CompositeAssociation<T>(IAssociationType associationType) where T : class, IObject;

        ICompositesAssociation CompositesAssociation(IAssociationType associationType);

        ICompositesAssociation<T> CompositesAssociation<T>(IAssociationType associationType) where T : class, IObject;

        IMethod Method(IMethodType methodType);
    }
}
