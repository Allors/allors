﻿// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using Meta;

    public interface IRelationEnd : IOperand, IObservable<IRelationEnd>
    {
        IStrategy Object { get; }

        IRelationType RelationType { get; }

        object Value { get; }
    }
}
