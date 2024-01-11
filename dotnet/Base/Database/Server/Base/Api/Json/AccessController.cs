// <copyright file="DatabaseController.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System;
    using System.Threading;
    using Allors.Protocol.Json.Api.Security;
    using Allors.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NLog;

    [ApiController]
    [Route("allors/access")]
    public class AccessController : ControllerBase
    {
        public AccessController(ITransactionService transactionService, IWorkspaceService workspaceService, IPolicyService policyService)
        {
            this.TransactionService = transactionService;
            this.WorkspaceService = workspaceService;
            this.PolicyService = policyService;
        }

        private ITransactionService TransactionService { get; }

        public IWorkspaceService WorkspaceService { get; }

        private IPolicyService PolicyService { get; }

        public Logger Logger => LogManager.GetCurrentClassLogger();

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public ActionResult<AccessResponse> Post([FromBody] AccessRequest accessRequest, CancellationToken cancellationToken) =>
            this.PolicyService.SyncPolicy.Execute(
                () =>
                {
                    try
                    {
                        using var transaction = this.TransactionService.Transaction;
                        var api = new Api(transaction, this.WorkspaceService.Name, cancellationToken);
                        return api.Access(accessRequest);
                    }
                    catch (Exception e)
                    {
                        this.Logger.Error(e, "AccessRequest {request}", accessRequest);
                        throw;
                    }
                });
    }
}
