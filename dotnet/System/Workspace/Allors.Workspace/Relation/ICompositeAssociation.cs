// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public interface ICompositeAssociation : IAssociation, IObservable<ICompositeAssociation>
    {
        new IStrategy Value { get; }
    }

    public interface ICompositeAssociation<out T> : ICompositeAssociation, IObservable<ICompositeAssociation<T>> where T : class, IObject
    {
        new T Value { get; }
    }
}
