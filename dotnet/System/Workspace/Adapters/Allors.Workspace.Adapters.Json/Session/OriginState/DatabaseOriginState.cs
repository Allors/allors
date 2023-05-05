// <copyright file="DatabaseOriginState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api.Push;
    using Shared.Ranges;

    internal sealed class DatabaseOriginState : Adapters.DatabaseOriginState
    {
        internal DatabaseOriginState(Strategy strategy, DatabaseRecord record) : base(record) => this.RemoteStrategy = strategy;

        public override Adapters.Strategy Strategy => this.RemoteStrategy;
        private Strategy RemoteStrategy { get; }

        internal PushRequestNewObject PushNew() => new PushRequestNewObject
        {
            w = this.Id,
            t = this.Class.Tag,
            r = this.PushRoles()
        };

        internal PushRequestObject PushExisting() => new PushRequestObject
        {
            d = this.Id,
            v = this.Version,
            r = this.PushRoles()
        };

        private PushRequestRole[] PushRoles()
        {
            if (this.ChangedRoleByRelationType?.Count > 0)
            {
                var database = this.RemoteStrategy.Session.Workspace.DatabaseConnection;
                var roles = new List<PushRequestRole>();

                foreach (var keyValuePair in this.ChangedRoleByRelationType)
                {
                    var relationType = keyValuePair.Key;
                    var roleValue = keyValuePair.Value;

                    var pushRequestRole = new PushRequestRole { t = relationType.Tag };

                    if (relationType.RoleType.ObjectType.IsUnit)
                    {
                        pushRequestRole.u = database.UnitConvert.ToJson(roleValue);
                    }
                    else if (relationType.RoleType.IsOne)
                    {
                        pushRequestRole.c = ((Adapters.Strategy)roleValue)?.Id;
                    }
                    else
                    {
                        var roleStrategies = RefRange<Adapters.Strategy>.Ensure(roleValue);
                        var roleIds = ValueRange<long>.Load(roleStrategies.Select(v => v.Id));

                        if (!this.ExistRecord)
                        {
                            pushRequestRole.a = roleIds.Save();
                        }
                        else
                        {
                            var databaseRole = ValueRange<long>.Ensure(this.DatabaseRecord.GetRole(relationType.RoleType));
                            if (databaseRole.IsEmpty)
                            {
                                pushRequestRole.a = roleIds.Save();
                            }
                            else
                            {
                                pushRequestRole.a = roleIds.Except(databaseRole).Save();
                                pushRequestRole.r = databaseRole.Except(roleIds).Save();
                            }
                        }
                    }

                    roles.Add(pushRequestRole);
                }

                return roles.ToArray();
            }

            return null;
        }
    }
}