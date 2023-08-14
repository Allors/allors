// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System;
    using System.Linq.Expressions;

    public class ReactiveExpressionBuilder : IReactiveExpressionBuilder
    {
        public IExpression<TObject, TValue> Build<TObject, TValue>(TObject @object, Func<TObject, IDependencyTracker, TValue> reactiveFunc)
            where TObject : IObject
        {
            return new ReactiveExpression<TObject, TValue>(@object, reactiveFunc);
        }
    }
}
