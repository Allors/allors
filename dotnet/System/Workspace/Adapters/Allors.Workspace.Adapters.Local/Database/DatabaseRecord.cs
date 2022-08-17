// <copyright file="DatabaseRecord.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Local
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using Shared.Ranges;

    public class DatabaseRecord : Adapters.DatabaseRecord
    {
        private readonly Grant[] accessControls;
        private readonly ValueRange<long> deniedPermissionIds;

        private readonly Dictionary<RoleType, object> roleByRoleType;

        internal DatabaseRecord(Class @class, long id)
            : base(@class, id, 0)
        {
        }

        internal DatabaseRecord(Class @class, long id, long version, Dictionary<RoleType, object> roleByRoleType, ValueRange<long> deniedPermissionIds, Grant[] accessControls)
            : base(@class, id, version)
        {
            this.roleByRoleType = roleByRoleType;
            this.deniedPermissionIds = deniedPermissionIds;
            this.accessControls = accessControls;
        }

        public override object GetRole(RoleType roleType)
        {
            if (this.roleByRoleType == null)
            {
                return null;
            }

            this.roleByRoleType.TryGetValue(roleType, out var role);
            return role;
        }

        public override bool IsPermitted(long permission)
        {
            if (this.accessControls == null)
            {
                return false;
            }

            return !this.deniedPermissionIds.Contains(permission) && this.accessControls.Any(v => v.PermissionIds.Contains(permission));
        }
    }
}
