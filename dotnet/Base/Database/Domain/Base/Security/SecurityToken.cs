// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Linq;
    using Allors.Database.Security;

    public partial class SecurityToken : ISecurityToken
    {
        public static readonly Guid InitialSecurityTokenId = new Guid("BE3404FF-1FF1-4C26-935F-777DC0AF983C");
        public static readonly Guid DefaultSecurityTokenId = new Guid("EF20E782-0BFB-4C59-B9EB-DC502C2256CA");
        public static readonly Guid AdministratorSecurityTokenId = new Guid("8C7FE74E-A769-49FC-BF69-549DBABD55D8");

        IGrant[] ISecurityToken.Grants => this.Grants.ToArray();
    }
}
