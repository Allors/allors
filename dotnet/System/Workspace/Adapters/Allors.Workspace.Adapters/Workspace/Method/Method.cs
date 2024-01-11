// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Adapters;
    using Meta;
    using Signals;

    public class Method : IMethod
    {
        private bool previousCanExecute;
        private long workspaceVersion;

        public Method(Strategy strategy, IMethodType methodType)
        {
            this.Object = strategy;
            this.MethodType = methodType;
        }

        IStrategy IMethod.Object => this.Object;

        public Strategy Object { get; }

        public IMethodType MethodType { get; }

        public bool CanExecute => this.Object.CanExecute(this.MethodType);

        public long WorkspaceVersion
        {
            get
            {
                if (this.previousCanExecute != this.CanExecute)
                {
                    ++this.workspaceVersion;
                }

                return this.workspaceVersion;
            }
        }

        object ISignal.Value => this;

        IMethod ISignal<IMethod>.Value => this;
    }
}
