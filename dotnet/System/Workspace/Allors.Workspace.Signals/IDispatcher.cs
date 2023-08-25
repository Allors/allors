// <copyright file="IDispatcher.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;

    public interface IDispatcher
    {
        IUnitRoleSignal<T> CreateUnitRoleSignal<T>(IUnitRole<T> role);

        ICompositeRoleSignal<T> CreateCompositeRoleSignal<T>(ICompositeRole<T> role) where T : class, IObject;

        ICompositesRoleSignal<T> CreateCompositesRoleSignal<T>(ICompositesRole<T> role) where T : class, IObject;

        ICompositeAssociationSignal<T> CreateCompositeAssociationSignal<T>(ICompositeAssociation<T> role) where T : class, IObject;

        ICompositesAssociationSignal<T> CreateCompositesAssociationSignal<T>(ICompositesAssociation<T> role) where T : class, IObject;

        ICalculatedSignal<T> CreateCalculatedSignal<T>(Func<IDependencyTracker, T> expression);

        IEffect CreateEffect(IEnumerable<ISignal> dependencies, Action action);
    }
}
