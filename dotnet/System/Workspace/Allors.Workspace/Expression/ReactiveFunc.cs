// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.ComponentModel;
    
    public class ReactiveExpression<TObject, TValue> : IReactive
    {
        private readonly Func<TObject, DependencyTracker, TValue> expression;
        private readonly TObject @object;

        private DependencyTracker tracker;
        private TValue previousValue;
        private TValue value;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReactiveExpression(TObject @object, Func<TObject, DependencyTracker, TValue> expression)
        {
            this.@object = @object;
            this.expression = expression;
        }

        public TValue Value
        {
            get
            {
                this.TrackAndEvaluate();
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

            this.TrackAndEvaluate();
        }

        private void TrackAndEvaluate()
        {
            if (this.tracker == null)
            {
                this.tracker = new DependencyTracker();

                var newValue = this.expression(this.@object, this.tracker);
                if (!Equals(this.value, newValue))
                {
                    this.previousValue = this.value;
                    this.value = newValue;

                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                }

                foreach (var dependency in this.tracker.Dependencies)
                {
                    dependency.PropertyChanged += this.Dependency_PropertyChanged;
                }
            }
        }

    }
}
