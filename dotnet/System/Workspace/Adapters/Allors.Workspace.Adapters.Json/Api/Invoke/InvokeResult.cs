// <copyright file="RemoteInvokeResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using Allors.Protocol.Json.Api.Invoke;
    using Allors.Workspace.Response;

    public class InvokeResult : Result, IInvokeResult
    {
        public InvokeResult(Workspace workspace, InvokeResponse invokeResponse) : base(workspace, invokeResponse)
        {
        }
    }
}
