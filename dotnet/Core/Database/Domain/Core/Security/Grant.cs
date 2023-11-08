// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Linq;
    using Allors.Database.Security;

    public partial class Grant : IGrant
    {
        public static readonly Guid CreatorsId = new Guid("1ED18B97-44F1-4AE7-A8DD-C7DA0BAE21E4");
        public static readonly Guid GuestCreatorsId = new Guid("16AC80CE-FC54-408D-AD33-DDAD249B82E4");

        public static readonly Guid AdministratorId = new Guid("282C4874-10EC-437B-9B0D-FAADFDFEC63E");
        public static readonly Guid GuestId = new Guid("07AED92A-84E7-4DA6-96A3-C764093D2A58");

        public void CoreOnPostDerive(ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(this, this.Meta.Subjects, this.Meta.SubjectGroups);
        }

        IPermission[] IGrant.Permissions => this.EffectivePermissions.ToArray();
    }
}
