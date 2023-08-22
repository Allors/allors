// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;

    public class Method : IMethod
    {
        private readonly IMethod method;

        public Method(IStrategy strategy, IMethodType methodType)
        {
            this.method = strategy.Method(methodType);
        }

        public IStrategy Object => this.method.Object;

        public IMethodType MethodType => this.method.MethodType;
    }
}
