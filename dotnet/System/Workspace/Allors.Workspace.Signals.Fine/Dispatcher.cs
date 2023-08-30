// <copyright file="IDispatcher.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals.Fine
{
    using System;
    using System.Collections.Generic;

    public class Dispatcher : IDispatcher
    {
        private readonly IList<ICacheable> cacheables;
        private readonly IList<Effect> effects;
       
        public Dispatcher(IWorkspace workspace)
        {
            this.cacheables = new List<ICacheable>();
            this.effects = new List<Effect>();

            workspace.DatabaseChanged += this.WorkspaceOnDatabaseChanged;
            workspace.WorkspaceChanged += WorkspaceOnWorkspaceChanged;
        }

        public IValueSignal<T> CreateValueSignal<T>(T value)
        {
            return new ValueSignal<T>(this, value);
        }

        public IComputedSignal<T> CreateCalculatedSignal<T>(Func<IDependencyTracker, T> calculation)
        {
            var computedSignal =  new ComputedSignal<T>(calculation);
            this.cacheables.Add(computedSignal);
            return computedSignal;
        }

        public IEffect CreateEffect(Action<IDependencyTracker> dependencies, Action action)
        {
            var effect = new Effect(this, dependencies, action);
            this.effects.Add(effect);

            effect.Raise();
            
            return effect;
        }
        
        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Schedule()
        {
            this.OnChange();
        }

        public void RemoveEffect(Effect effect)
        {
            this.effects.Remove(effect);
        }

        private void WorkspaceOnDatabaseChanged(object sender, DatabaseChangedEventArgs e)
        {
            this.OnChange();
        }

        private void WorkspaceOnWorkspaceChanged(object sender, WorkspaceChangedEventArgs e)
        {
            this.OnChange();
        }

        private void OnChange()
        {
            foreach (var cacheable in this.cacheables)
            {
                cacheable.InvalidateCache();
            }

            foreach (var effect in this.effects)
            {
                effect.Raise();
            }
        }
    }
}
