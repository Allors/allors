﻿// <copyright file="ObjectsBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Database.Meta;
    using Population;

    public abstract partial class ObjectsBase<T> : IObjects where T : IObject
    {
        protected ObjectsBase(ITransaction transaction)
        {
            this.Transaction = transaction;
            this.M = this.Transaction.Database.Services.Get<M>();
        }

        public M M { get; }

        public abstract IComposite ObjectType { get; }

        public ITransaction Transaction { get; private set; }

        protected virtual void CorePrepare(Setup setup) => setup.Add(this);

        protected virtual void CoreSetup(Setup setup)
        {
            if (this.ObjectType is IClass @class)
            {
                var recordsByClass = setup.Config.RecordsByClass;

                if (recordsByClass != null && recordsByClass.TryGetValue(@class, out var records))
                {
                    var keyRoleType = @class.KeyRoleType;

                    var objectByKey = this.Transaction.Extent(@class).ToDictionary(v => v.Strategy.GetUnitRole(keyRoleType));

                    foreach (var record in records)
                    {
                        var key = record.ValueByRoleType[@class.KeyRoleType];

                        if (!objectByKey.TryGetValue(key, out var @object))
                        {
                            @object = this.Transaction.Build(@class, v =>
                            {
                                var strategy = v.Strategy;
                                foreach ((IRoleType roleType, object value) in record.ValueByRoleType.Where(role => role.Key.ObjectType.IsUnit))
                                {
                                    strategy.SetRole(roleType, value);
                                }
                            });

                            setup.OnCreated(@object);

                            objectByKey.Add(key, @object);
                        }
                    }
                }
            }
        }

        protected virtual void CorePrepare(Security security) => security.Add(this);

        protected virtual void CoreSecure(Security config)
        {
        }
    }
}
