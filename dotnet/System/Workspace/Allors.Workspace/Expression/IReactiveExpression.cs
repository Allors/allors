// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IExpression<out TObject, out TValue> : IReactive
        where TObject : IObject
    {
        TObject Object { get; }

        TValue Value { get; }
    }
}
