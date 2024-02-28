// <copyright file="TestNoTreeController.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Server.Controllers
{
    using System.Threading;
    using Allors.Services;
    using Domain;
    using Microsoft.AspNetCore.Mvc;
    using Database;
    using Meta;
    using Protocol.Json;


    public class TestNoTreeController : Controller
    {
        public TestNoTreeController(ITransactionService sessionService, IWorkspaceService workspaceService)
        {
            this.WorkspaceService = workspaceService;
            this.Transaction = sessionService.Transaction;
            this.TreeCache = this.Transaction.Database.Services.Get<ITreeCache>();
        }

        private ITransaction Transaction { get; }

        public IWorkspaceService WorkspaceService { get; }

        public ITreeCache TreeCache { get; }

        [HttpPost]
        public IActionResult Pull(CancellationToken cancellationToken)
        {
            var m = this.Transaction.Database.Services.Get<IMetaIndex>();

            var api = new Api(this.Transaction, this.WorkspaceService.Name, cancellationToken);
            var response = api.CreatePullResponseBuilder();

            response.AddObject("object", api.User);
            response.AddCollection("collection", m.Organization.Composite, this.Transaction.Extent<Organization>());
            return this.Ok(response.Build());
        }
    }
}
