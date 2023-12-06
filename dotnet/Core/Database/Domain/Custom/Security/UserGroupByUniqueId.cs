// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class UserGroupByUniqueId : IScoped
    {
        public UserGroup Operations => this.cache[UserGroup.OperationsId];

        public UserGroup Sales => this.cache[UserGroup.SalesId];

        public UserGroup Procurement => this.cache[UserGroup.ProcurementId];

    }
}
