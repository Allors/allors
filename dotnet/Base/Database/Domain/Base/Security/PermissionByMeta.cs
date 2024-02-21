// <copyright file="Permissions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the role type.</summary>

namespace Allors.Database.Domain
{
    using Allors.Database.Meta.Extensions;
    using Allors.Database.Meta;

    public partial class PermissionByMeta : IScoped
    {
        public PermissionByMeta(ITransaction transaction)
        {
            this.Transaction = transaction;
        }

        public ITransaction Transaction { get; }

        public Permission Get(IClass @class, IRoleType roleType, Operations operation)
        {
            var id = operation switch
            {
                Operations.Read => @class.ReadPermissionIdByRelationTypeId()[roleType.RelationType.Id],
                Operations.Write => @class.WritePermissionIdByRelationTypeId()[roleType.RelationType.Id],
                Operations.Create => 0,
                Operations.Execute => 0,
            };

            return (Permission)this.Transaction.Instantiate(id);
        }

        // TODO: Make extension method on Class
        public Permission Get(IClass @class, MethodType methodType)
        {
            var id = @class.ExecutePermissionIdByMethodTypeId()[methodType.Id];
            return (Permission)this.Transaction.Instantiate(id);
        }
    }
}
