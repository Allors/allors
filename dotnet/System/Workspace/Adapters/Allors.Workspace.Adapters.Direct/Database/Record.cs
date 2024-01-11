// <copyright file="Record.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using Shared.Ranges;

    public class Record : Adapters.Record
    {
        private readonly AccessControl[] accessControls;
        private readonly ValueRange<long> deniedPermissionIds;

        private readonly Dictionary<IRoleType, object> roleByRoleType;

        internal Record(IClass @class, long id)
            : base(@class, id, 0)
        {
        }

        internal Record(IClass @class, long id, long version, Dictionary<IRoleType, object> roleByRoleType, ValueRange<long> deniedPermissionIds, AccessControl[] accessControls)
            : base(@class, id, version)
        {
            this.roleByRoleType = roleByRoleType;
            this.deniedPermissionIds = deniedPermissionIds;
            this.accessControls = accessControls;
        }

        public override object GetRole(IRoleType roleType)
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
