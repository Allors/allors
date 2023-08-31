// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System;
    using System.Linq.Expressions;

    public class TrackableFuncBuilder : ITrackableFuncBuilder
    {
        public Func<IDependency, TValue> Build<TValue>(Expression<Func<TValue>> funcExpression)
        {
            var reactiveVisitor = new TrackableFuncVisitor();
            var reactiveExpression = (LambdaExpression)reactiveVisitor.Visit(funcExpression);
            return (Func<IDependency, TValue>)reactiveExpression.Compile();
        }
    }
}
