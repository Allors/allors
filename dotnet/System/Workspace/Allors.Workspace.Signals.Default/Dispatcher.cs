// <copyright file="IDispatcher.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals.Default
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Dispatcher : IDispatcher
    {
        private readonly Dictionary<ISignal, WeakReference<IUpstream>[]> upstreamsBySignal;
        private readonly IList<Effect> effects;

        public Dispatcher(IWorkspace workspace)
        {
            this.upstreamsBySignal = new Dictionary<ISignal, WeakReference<IUpstream>[]>();
            this.effects = new List<Effect>();

            workspace.DatabaseChanged += this.WorkspaceOnDatabaseChanged;
            workspace.WorkspaceChanged += WorkspaceOnWorkspaceChanged;
        }

        public IValueSignal<T> CreateValueSignal<T>(T value)
        {
            return new ValueSignal<T>(this, value);
        }

        public IComputedSignal<T> CreateComputedSignal<T>(Func<ITracker, T> calculation)
        {
            var computedSignal = new ComputedSignal<T>(this, calculation);
            return computedSignal;
        }

        public IEffect CreateEffect(Action<ITracker> dependencies, Action action)
        {
            var effect = new Effect(this, dependencies, action);
            this.effects.Add(effect);

            effect.Handle();

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

        public void RemoveEffect(Effect effect)
        {
            this.effects.Remove(effect);
        }

        internal void UpdateTracked(IUpstream upstream, IEnumerable<ISignal> trackedSignals)
        {
            foreach (var trackedSignal in trackedSignals)
            {
                if (trackedSignal is IDownstream downstream)
                {
                    downstream.TrackedBy(upstream);
                }
                else
                {
                    this.upstreamsBySignal.TryGetValue(trackedSignal, out var upstreams);
                    upstreams = upstreams.Update(upstream);
                    this.upstreamsBySignal[trackedSignal] = upstreams;
                }
            }
        }

        internal void HandleEffects()
        {
            foreach (var effect in this.effects)
            {
                effect.Handle();
            }
        }

        private void WorkspaceOnDatabaseChanged(object sender, DatabaseChangedEventArgs e)
        {
            foreach (var weakSignal in this.upstreamsBySignal.Select(kvp => kvp.Value))
            {
                weakSignal.Invalidate();
            }

            this.HandleEffects();
        }

        private void WorkspaceOnWorkspaceChanged(object sender, WorkspaceChangedEventArgs e)
        {
            var operands = e.Operands;
            foreach (var operand in operands)
            {
                if (this.upstreamsBySignal.TryGetValue(operand, out var operandSignal))
                {
                    operandSignal.Invalidate();
                }
            }

            this.HandleEffects();
        }

        public void Dispose()
        {
            foreach (var effect in this.effects)
            {
                effect.Dispose();
            }

            this.effects.Clear();
        }
    }
}
