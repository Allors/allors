﻿// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;

    public interface IRelationEnd : IOperand
    {
        IStrategy Object { get; }

        IRelationType RelationType { get; }

        object Value { get; }
    }

    public interface IRelationEnd<out T> : IRelationEnd, IOperand<IRelationEnd<T>>
    {
        new T Value { get; }
    }
}
