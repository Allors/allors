﻿// <copyright file="TestTransactionController.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Server.Controllers
{
    using Allors.Services;
    using Database;
    using Domain;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public class TestTransactionController : Controller
    {
        public TestTransactionController(ITransactionService sessionService, IWorkspaceService workspaceService)
        {
            this.WorkspaceService = workspaceService;
            this.Transaction = sessionService.Transaction;
            this.TreeCache = this.Transaction.Database.Services.Get<ITreeCache>();
        }

        private ITransaction Transaction { get; }

        public IWorkspaceService WorkspaceService { get; }

        public ITreeCache TreeCache { get; }
        
        [HttpPost]
        [AllowAnonymous]
        [Authorize]
        public IActionResult UserName()
        {
            var userService = this.Transaction.Services.Get<IUserService>();
            var result = (userService?.User as User)?.UserName ?? string.Empty;
            return this.Content(result);
        }
    }
}
