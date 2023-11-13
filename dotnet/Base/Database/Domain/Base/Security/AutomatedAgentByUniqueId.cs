// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class AutomatedAgentByUniqueId : IScoped
    {
        private readonly ICache<Guid, AutomatedAgent> cache;

        public AutomatedAgentByUniqueId(ITransaction transaction)
        {
            this.cache = transaction.Caches().AutomatedAgentByUniqueId();
        }

        public AutomatedAgent Guest => this.cache[AutomatedAgent.GuestId];

        public AutomatedAgent System => this.cache[AutomatedAgent.SystemId];
    }
}
