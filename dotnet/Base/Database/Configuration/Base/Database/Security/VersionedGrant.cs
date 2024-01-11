// <copyright file="IBarcodeGenerator.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration
{
    using System.Collections.Generic;
    using Allors.Database.Domain;
    using Allors.Shared.Ranges;

    public class VersionedGrant : IVersionedGrant
    {
        public VersionedGrant(long id, long version, ISet<long> users, IEnumerable<long> permissions)
        {
            this.Id = id;
            this.Version = version;
            this.UserSet = users;
            this.PermissionRange = ValueRange<long>.Import(permissions);
            this.PermissionSet = new HashSet<long>(this.PermissionRange);
        }

        public long Id { get; }

        public long Version { get; }

        public ISet<long> UserSet { get; }

        public ISet<long> PermissionSet { get; }

        public ValueRange<long> PermissionRange { get; }
    }
}
