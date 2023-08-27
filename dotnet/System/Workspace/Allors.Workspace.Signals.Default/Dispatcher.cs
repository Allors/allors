// <copyright file="IDispatcher.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals.Default
{
    using System;
    using System.Collections.Generic;

    public class Dispatcher : IDispatcher
    {
        public IUnitRoleSignal<T> CreateUnitRoleSignal<T>(IUnitRole<T> role)
        {
            throw new NotImplementedException();
        }

        public ICompositeRoleSignal<T> CreateCompositeRoleSignal<T>(ICompositeRole<T> role) where T : class, IObject
        {
            throw new NotImplementedException();
        }

        public ICompositesRoleSignal<T> CreateCompositesRoleSignal<T>(ICompositesRole<T> role) where T : class, IObject
        {
            throw new NotImplementedException();
        }

        public ICompositeAssociationSignal<T> CreateCompositeAssociationSignal<T>(ICompositeAssociation<T> role) where T : class, IObject
        {
            throw new NotImplementedException();
        }

        public ICompositesAssociationSignal<T> CreateCompositesAssociationSignal<T>(ICompositesAssociation<T> role) where T : class, IObject
        {
            throw new NotImplementedException();
        }

        public ICalculatedSignal<T> CreateCalculatedSignal<T>(Func<IDependencyTracker, T> expression)
        {
            return new CalculatedSignal<T>(expression);
        }

        public IEffect CreateEffect(IEnumerable<ISignal> dependencies, Action action)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }
    }
}
