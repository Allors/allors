// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;
    using Allors.Workspace.Response;
    using Allors.Workspace.Request.Visitor;

    public class MethodRequest : IRequest, IVisitable
    {
        public MethodRequest(IObject @object, IMethodType methodType)
        {
            this.Object = @object;
            this.MethodType = methodType;
        }

        public IObject Object { get; }

        public IMethodType MethodType { get; }

        public void Accept(IVisitor visitor) => visitor.VisitMethodCall(this);
    }
}
