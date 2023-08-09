// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System;
    using System.Linq.Expressions;
    using ExpressionTreeToString;

    public class ReactiveExpressionBuilder : IReactiveExpressionBuilder
    {
        public IReactiveExpression<TValue> Build<TObject, TValue>(Expression<Func<TObject, TValue>> expression, TObject @object)
        {
            var reactiveVisitor = new ReactiveVisitor();
            var reactiveExpression = (LambdaExpression)reactiveVisitor.Visit(expression);

            var code = reactiveExpression.ToString("C#");

            return new ReactiveExpression<TObject, TValue>(reactiveExpression, @object);
        }
    }
}
