// <copyright file="IDispatcher.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals.Default
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Dispatcher : IDispatcher
    {
        private readonly Dictionary<IOperand, OperandSignal> operandSignalByOperand;
        private readonly IList<Effect> effects;
        private readonly ISet<Effect> scheduledEffects;

        public Dispatcher(IWorkspace workspace)
        {
            this.effects = new List<Effect>();
            this.scheduledEffects = new HashSet<Effect>();

            workspace.DatabaseChanged += this.WorkspaceOnDatabaseChanged;
            workspace.WorkspaceChanged += WorkspaceOnWorkspaceChanged;
        }

        public OperandSignal GetOrCreateOperandSignal(IOperand operand)
        {
            if (!this.operandSignalByOperand.TryGetValue(operand, out var operandSignal))
            {
                operandSignal = new OperandSignal(operand);
                this.operandSignalByOperand.Add(operand, operandSignal);
            }

            return operandSignal;
        }

        public IValueSignal<T> CreateValueSignal<T>(T value)
        {
            return new ValueSignal<T>(value);
        }

        public IComputedSignal<T> CreateComputedSignal<T>(Func<ITracker, T> calculation)
        {
            var computedSignal =  new ComputedSignal<T>(calculation);
            return computedSignal;
        }

        public IEffect CreateEffect(Action<ITracker> dependencies, Action action)
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

        public void RemoveEffect(Effect effect)
        {
            this.effects.Remove(effect);
        }

        private void WorkspaceOnDatabaseChanged(object sender, DatabaseChangedEventArgs e)
        {
            foreach (var operandSignal in this.operandSignalByOperand.Select(kvp => kvp.Value))
            {
                operandSignal.OnChanged();
            }
        }

        internal void UpdateTracked(IUpstream upstream, ISet<IOperand> trackedOperands)
        {
            foreach (var trackedOperand in trackedOperands)
            {
                var downstream = trackedOperand as IDownstream ?? this.GetOrCreateOperandSignal(trackedOperand);
                downstream.TrackedBy(upstream);
            }
        }

        private void WorkspaceOnWorkspaceChanged(object sender, WorkspaceChangedEventArgs e)
        {
            var operands = e.Operands;
            foreach (var operand in operands)
            {
                if (this.operandSignalByOperand.TryGetValue(operand, out var operandSignal))
                {
                    operandSignal.OnChanged();
                }
            }
        }

        public void Schedule(Effect effect)
        {
            this.scheduledEffects.Add(effect);
        }
    }
}
