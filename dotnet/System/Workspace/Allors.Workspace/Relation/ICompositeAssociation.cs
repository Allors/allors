﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public interface ICompositeAssociation : IAssociation
    {
        new IStrategy Value { get; }
    }

    public interface ICompositeAssociation<out T> : ICompositeAssociation
        where T : class, IObject
    {
        new T Value { get; }
    }
}
