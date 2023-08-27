﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System;
    using System.Linq.Expressions;

    public class ReactiveFuncBuilder : IReactiveFuncBuilder
    {
        public Func<IDependencyTracker, TValue> Build<TValue>(Expression<Func<TValue>> expression)
        {
            var reactiveVisitor = new ReactiveFuncVisitor();
            var reactiveExpression = (LambdaExpression)reactiveVisitor.Visit(expression);
            return (Func<IDependencyTracker, TValue>)reactiveExpression.Compile();
        }
    }
}
