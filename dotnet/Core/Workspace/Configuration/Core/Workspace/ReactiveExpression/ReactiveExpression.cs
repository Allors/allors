// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;

    public class ReactiveExpression<TObject, TValue> : IReactiveExpression<TValue>
    {
        private readonly Func<TObject, HashSet<INotifyPropertyChanged>, TValue> expression;
        private readonly TObject @object;

        private HashSet<INotifyPropertyChanged> dependencies;
        private TValue value;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveExpression(LambdaExpression expression, TObject @object)
        {
            this.@object = @object;
            this.expression = (Func<TObject, HashSet<INotifyPropertyChanged>, TValue>)expression.Compile();
        }

        public TValue Value
        {
            get
            {
                if (this.dependencies == null)
                {
                    this.dependencies = new HashSet<INotifyPropertyChanged>();
                    this.value = this.expression(this.@object, this.dependencies);

                    foreach (var dependency in this.dependencies)
                    {
                        dependency.PropertyChanged += this.Dependency_PropertyChanged;
                    }
                }

                return this.value;
            }
        }

        private void Dependency_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.dependencies != null)
            {
                foreach (var source in this.dependencies)
                {
                    source.PropertyChanged -= this.Dependency_PropertyChanged;
                }

                this.dependencies = null;
            }
        }
    }
}
