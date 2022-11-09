// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request.Visitor;
    using Allors.Workspace.Response;

    public class MethodRequest : IRequest, IVisitable
    {
        public MethodRequest(IObject @object, IMethodType methodType, IRecord input = null)
        {
            this.Object = @object;
            this.MethodType = methodType;
            this.Input = input;
        }

        public IObject Object { get; }

        public IMethodType MethodType { get; }

        public IRecord Input { get; }

        public void Accept(IVisitor visitor) => visitor.VisitMethodCall(this);
    }
}
