﻿// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using Meta;

    public interface IAssociation : IRelationEnd
    {
        IAssociationType AssociationType { get; }
    }

    public interface IAssociation<out T> : IAssociation, IRelationEnd<T>, IOperand<IAssociation<T>> where T : class, IObject
    {
        new IEnumerable<T> Value { get; }
    }
}
