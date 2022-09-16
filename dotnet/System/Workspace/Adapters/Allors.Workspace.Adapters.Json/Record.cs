// <copyright file="RemoteDatabaseObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
        private readonly Connection connection;

        private Dictionary<RelationType, object> roleByRelationType;
        private SyncResponseRole[] syncResponseRoles;

        internal Record(Connection connection, Class @class, long id, long version) : base(@class, id, version) =>
            this.connection = connection;

        internal ValueRange<long> GrantIds { get; private set; }

        internal ValueRange<long> RevocationIds { get; private set; }

        private Dictionary<RelationType, object> RoleByRelationType
        {
            get
            {
                if (this.syncResponseRoles != null)
                {
                    var metaPopulation = this.connection.MetaPopulation;
                    this.roleByRelationType = this.syncResponseRoles.ToDictionary(
                        v => (RelationType)metaPopulation.FindByTag(v.t),
                        v =>
                        {
                            var roleType = ((RelationType)metaPopulation.FindByTag(v.t)).RoleType;
                            var objectType = roleType.ObjectType;

                            if (objectType.IsUnit)
                            {
                                return this.connection.UnitConvert.UnitFromJson(objectType.Tag, v.v);
                            }

                            if (roleType.IsOne)
                            {
                                return v.o;
                            }

                            return ValueRange<long>.Load(v.c);
                        });

                    this.syncResponseRoles = null;
                }

                return this.roleByRelationType;
            }
        }

        internal static Record FromResponse(Connection database, ResponseContext ctx, SyncResponseObject syncResponseObject) =>
            new Record(database, (Class)database.MetaPopulation.FindByTag(syncResponseObject.c), syncResponseObject.i, syncResponseObject.v)
            {
                syncResponseRoles = syncResponseObject.ro,
                GrantIds = ValueRange<long>.Load(ctx.CheckForMissingGrants(syncResponseObject.g)),
                RevocationIds = ValueRange<long>.Load(ctx.CheckForMissingRevocations(syncResponseObject.r))
            };

        public override object GetRole(RoleType roleType)
        {
            object @object = null;
            this.RoleByRelationType?.TryGetValue(roleType.RelationType, out @object);
            return @object;
        }

        public override bool IsPermitted(long permission)
        {
            if (this.GrantIds.IsEmpty)
            {
                return false;
            }

            if (this.RevocationIds.Any(v => this.connection.RevocationById[v].PermissionIds.Contains(permission)))
            {
                return false;
            }


            return this.GrantIds.Any(v => this.connection.GrantById[v].PermissionIds.Contains(permission));
        }
    }
}
