﻿// <copyright file="DefaultDomainTransactionServices.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration
{
    using System;
    using Database;
    using Domain;
    using Meta;
    using Services;

    public class TransactionServices : ITransactionServices
    {
        private readonly UserService userService;

        private ICaches caches;
        private IDatabaseAclsService databaseAclsService;
        private IWorkspaceAclsService workspaceAclsService;
        private IObjectBuilderService objectBuilderService;
        private IDeleting deleting;

        public TransactionServices()
        {
            this.userService = new UserService();
            this.userService.UserChanged += OnUserChanged;
        }

        public ITransaction Transaction { get; private set; }

        public IDatabaseServices DatabaseServices => this.Transaction.Database.Services;

        public T Get<T>() =>
            typeof(T) switch
            {
                // System
                { } type when type == typeof(IObjectBuilderService) => (T)(this.objectBuilderService ??= new ObjectBuilderService(this.Transaction)),
                // Core
                { } type when type == typeof(ICaches) => (T)this.caches,
                { } type when type == typeof(IUserService) => (T)(IUserService)this.userService,
                { } type when type == typeof(IDatabaseAclsService) => (T)(this.databaseAclsService ??= new DatabaseAclsService(this.userService.User, this.DatabaseServices.Get<ISecurity>())),
                { } type when type == typeof(IWorkspaceAclsService) => (T)(this.workspaceAclsService ??= new WorkspaceAclsService(this.DatabaseServices.Get<ISecurity>(), this.DatabaseServices.Get<IWorkspaceMask>(), this.userService.User)),
                { } type when type == typeof(IDeleting) => (T)(this.deleting ??= new Deleting()),
                _ => throw new NotSupportedException($"Service {typeof(T)} not supported")
            };

        public virtual void OnInit(ITransaction transaction)
        {
            this.Transaction = transaction;
            this.caches = new Caches(transaction, this.DatabaseServices.Get<M>());
            transaction.Database.Services.Get<IPermissions>().Load(transaction);
        }

        public void Dispose()
        {
        }

        private void OnUserChanged(object sender, EventArgs e)
        {
            this.databaseAclsService = null;
            this.workspaceAclsService = null;
        }
    }
}
