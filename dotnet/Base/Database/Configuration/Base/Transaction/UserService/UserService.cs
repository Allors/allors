// <copyright file="HttpContext.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>


using Allors.Database.Security;

namespace Allors.Database.Configuration
{
    using System;
    using Services;

    public class UserService : IUserService
    {
        private IUser user;

        public event EventHandler UserChanged;

        public IUser User
        {
            get => this.user;

            set
            {
                this.user = value;
                this.UserChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
