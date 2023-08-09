// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;

    public class ReactiveExpression<TObject, TValue> : IReactiveExpression<TValue>
    {
        private readonly Func<TObject, TValue> expression;
        private readonly TObject @object;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveExpression(LambdaExpression expression, TObject @object)
        {
            this.@object = @object;
            this.expression = (Func<TObject, TValue>)expression.Compile();
        }

        public TValue Value
        {
            get
            {
                return this.expression(this.@object);
            }
        }
    }
}
