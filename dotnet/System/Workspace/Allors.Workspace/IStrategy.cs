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

        IRole Role(IRoleType roleType);

        IUnitRole<T> UnitRole<T>(IRoleType roleType);

        ICompositeRole CompositeRole(IRoleType roleType);

        ICompositesRole CompositesRole(IRoleType roleType);

        IAssociation Association(IAssociationType associationType);

        ICompositeAssociation CompositeAssociation(IAssociationType associationType);

        ICompositesAssociation CompositesAssociation(IAssociationType associationType);

        IMethod Method(IMethodType methodType);
    }
}
