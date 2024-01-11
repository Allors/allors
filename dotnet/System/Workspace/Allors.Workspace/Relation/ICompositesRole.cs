// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;

    public interface ICompositesRole : IRole
    {
        void Add(IStrategy strategy);

        void Remove(IStrategy strategy);

        new IEnumerable<IStrategy> Value { get; set; }
    }

    public interface ICompositesRole<T> : ICompositesRole, IOperand<ICompositesRole<T>>
        where T : class, IObject
    {
        void Add(T @object);

        void Remove(T @object);

        new IEnumerable<T> Value { get; set; }
    }
}
