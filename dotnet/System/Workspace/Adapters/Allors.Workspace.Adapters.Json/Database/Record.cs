// <copyright file="RemoteDatabaseObject.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api.Sync;
    using Meta;
    using Shared.Ranges;

    internal class Record : Adapters.Record
    {
        private readonly Connection database;

        private Dictionary<IRoleType, object> roleByRoleType;
        private SyncResponseRole[] syncResponseRoles;

        internal Record(Connection database, IClass @class, long id, long version) : base(@class, id, version) => this.database = database;

        internal static Record FromResponse(Connection database, ResponseContext ctx, SyncResponseObject syncResponseObject) =>
            new Record(database, (IClass)database.Configuration.MetaPopulation.FindByTag(syncResponseObject.c), syncResponseObject.i, syncResponseObject.v)
            {
                syncResponseRoles = syncResponseObject.ro,
                GrantIds = ValueRange<long>.Load(ctx.CheckForMissingGrants(syncResponseObject.g)),
                RevocationIds = ValueRange<long>.Load(ctx.CheckForMissingRevocations(syncResponseObject.r))
            };

        internal ValueRange<long> GrantIds { get; private set; }

        internal ValueRange<long> RevocationIds { get; private set; }

        private Dictionary<IRoleType, object> RoleByRoleType
        {
            get
            {
                if (this.syncResponseRoles != null)
                {
                    var metaPopulation = this.database.Configuration.MetaPopulation;
                    this.roleByRoleType = this.syncResponseRoles.ToDictionary(
                        v => (IRoleType)metaPopulation.FindByTag(v.t),
                        v =>
                        {
                            var roleType = ((IRoleType)metaPopulation.FindByTag(v.t));
                            var objectType = roleType.ObjectType;

                            if (objectType.IsUnit)
                            {
                                return this.database.UnitConvert.UnitFromJson(objectType.Tag, v.v);
                            }

                            if (roleType.IsOne)
                            {
                                return v.o;
                            }

                            return ValueRange<long>.Load(v.c);
                        });

                    this.syncResponseRoles = null;
                }

                return this.roleByRoleType;
            }
        }

        public override object GetRole(IRoleType roleType)
        {
            object @object = null;
            this.RoleByRoleType?.TryGetValue(roleType, out @object);
            return @object;
        }

        public override bool IsPermitted(long permission)
        {
            if (this.GrantIds.IsEmpty)
            {
                return false;
            }

            return !this.RevocationIds.Any(v => this.database.RevocationById[v].PermissionIds.Contains(permission)) && this.GrantIds.Any(v => this.database.AccessControlById[v].PermissionIds.Contains(permission));
        }
    }
}
