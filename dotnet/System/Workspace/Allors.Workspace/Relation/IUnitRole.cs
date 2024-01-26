// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public interface IUnitRole : IRole, IOperand<IUnitRole>
    {
    }

    public interface IUnitRole<T> : IUnitRole, IRole<T>, IOperand<IUnitRole<T>>
    {
        new T Value { get; set; }
    }
}
