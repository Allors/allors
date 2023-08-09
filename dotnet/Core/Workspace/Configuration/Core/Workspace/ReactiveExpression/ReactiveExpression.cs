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
        private readonly Func<TObject, IDependencyTracker, TValue> expression;
        private readonly TObject @object;

        private DependencyTracker tracker;
        private TValue value;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveExpression(LambdaExpression expression, TObject @object)
        {
            this.@object = @object;
            this.expression = (Func<TObject, IDependencyTracker, TValue>)expression.Compile();
        }

        public TValue Value
        {
            get
            {
                if (this.tracker == null)
                {
                    this.tracker = new DependencyTracker();
                    this.value = this.expression(this.@object, this.tracker);

                    foreach (var dependency in this.tracker.Dependencies)
                    {
                        dependency.PropertyChanged += this.Dependency_PropertyChanged;
                    }
                }

                return this.value;
            }
        }

        private void Dependency_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.tracker != null)
            {
                foreach (var source in this.tracker.Dependencies)
                {
                    source.PropertyChanged -= this.Dependency_PropertyChanged;
                }

                this.tracker = null;
            }
        }
    }
}
