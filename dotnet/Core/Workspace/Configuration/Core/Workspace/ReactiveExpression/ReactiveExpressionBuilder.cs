// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System;
    using System.Linq.Expressions;

    public class ReactiveExpressionBuilder : IReactiveFuncBuilder
    {
        public Func<TObject, DependencyTracker, TValue> Build<TObject, TValue>(Expression<Func<TObject, TValue>> expression)
        {
            var reactiveVisitor = new ReactiveVisitor();
            var reactiveExpression = (LambdaExpression)reactiveVisitor.Visit(expression);
            return (Func<TObject, DependencyTracker, TValue>)reactiveExpression.Compile();
        }
    }
}
