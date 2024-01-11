﻿// <copyright file="SecurityToken.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Linq;

    public partial class SecurityToken
    {
        public string DebuggerDisplay
        {
            get
            {
                if (this.UniqueId == DefaultSecurityTokenId)
                {
                    return "Default";
                }

                if (this.UniqueId == InitialSecurityTokenId)
                {
                    return "Initial";
                }

                if (this.UniqueId == AdministratorSecurityTokenId)
                {
                    return "Administrator";
                }

                var toString = string.Join(",", this.Grants.ToArray().Select(v => v.ToString()));
                return $"{toString} [{this.strategy.ObjectId}]";
            }
        }
    }
}
