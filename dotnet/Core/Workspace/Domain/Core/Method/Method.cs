// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;

    public abstract class Method : IMethod
    {
        protected Method(IStrategy strategy, IMethodType methodType)
        {
            this.Object = strategy;
            this.MethodType = methodType;
        }

        public IStrategy Object { get; }

        public IMethodType MethodType { get; }
    }
}
