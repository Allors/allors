// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class Role
    {
        public static readonly Guid AdministratorId = new Guid("9D162C26-15B2-428e-AB80-DB4B3EBDBB7A");

        public static readonly Guid GuestId = new Guid("86445257-3F62-41e0-8B4A-2DF9FB18A8AA");

        public static readonly Guid GuestCreatorId = new Guid("5281A811-81A2-4381-94BB-90B1AED0CC40");

        public static readonly Guid CreatorId = new Guid("3A3D1E25-4A91-4D07-8203-A9F3EA691598");

        public static readonly Guid OwnerId = new Guid("E22EA50F-E616-4429-92D5-B91684AD3C2A");
    }
}
