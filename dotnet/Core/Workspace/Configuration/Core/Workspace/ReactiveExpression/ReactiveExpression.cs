// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class ReactiveExpression<TObject, TValue> : IExpression<TObject, TValue>, IDependencyTracker
        where TObject : IObject
    {
        private readonly Func<TObject, IDependencyTracker, TValue> expression;

        private ISet<INotifyPropertyChanged> dependencies;

        private TValue value;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveExpression(TObject @object, Func<TObject, IDependencyTracker, TValue> expression)
        {
            this.Object = @object;
            this.expression = expression;
        }

        public TObject Object { get; }

        public TValue Value
        {
            get
            {
                this.TrackAndEvaluate();
                return this.value;
            }
        }

        public void Track(INotifyPropertyChanged dependency)
        {
            this.dependencies ??= new HashSet<INotifyPropertyChanged>();
            this.dependencies.Add(dependency);
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

            this.TrackAndEvaluate();
        }

        private void TrackAndEvaluate()
        {
            if (this.dependencies == null)
            {
                var newValue = this.expression(this.Object, this);
                if (!Equals(this.value, newValue))
                {
                    this.value = newValue;

                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                }

                if (this.dependencies != null)
                {
                    foreach (var dependency in this.dependencies)
                    {
                        dependency.PropertyChanged += this.Dependency_PropertyChanged;
                    }
                }
            }
        }
    }
}
