﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Adapters;
    using Meta;

    public class Method : IMethod
    {
        public Method(Strategy strategy, IMethodType methodType)
        {
            this.Object = strategy;
            this.MethodType = methodType;
        }

        IStrategy IMethod.Object => this.Object;

        public Strategy Object { get; }

        public IMethodType MethodType { get; }

        public bool CanExecute => this.Object.CanExecute(this.MethodType);

        public long WorkspaceVersion => this.Object.WorkspaceVersion(this.MethodType);
    }
}
