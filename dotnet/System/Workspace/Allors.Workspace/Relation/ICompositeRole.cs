// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public interface ICompositeRole : IRole
    {
        new IStrategy Value { get; set; }
    }

    public interface ICompositeRole<T> : ICompositeRole where T : class, IObject
    {
        new T Value { get; set; }
    }
}
