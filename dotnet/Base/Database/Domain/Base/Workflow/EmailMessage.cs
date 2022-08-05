// <copyright file="EmailMessage.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class EmailMessage
    {
        public void BaseOnPostBuild(ObjectOnPostBuild method)
        {
            if (!this.ExistDateCreated)
            {
                this.DateCreated = this.Strategy.Transaction.Now();
            }
        }
    }
}
