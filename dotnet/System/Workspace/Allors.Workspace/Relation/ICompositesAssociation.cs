﻿// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;

    public interface ICompositesAssociation : IAssociation, IObservable<ICompositesAssociation>
    {
        new IEnumerable<IStrategy> Value { get; }
    }

    public interface ICompositesAssociation<out T> : ICompositesAssociation, IObservable<ICompositesAssociation<T>>
    {
        new IEnumerable<T> Value { get; }
    }
}
