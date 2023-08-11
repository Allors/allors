// <copyright file="ITime.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Linq.Expressions;

    public interface IReactiveFuncBuilder
    {
        Func<TObject, DependencyTracker, TValue> Build<TObject, TValue>(Expression<Func<TObject, TValue>> expression);
    }
}
