// <copyright file="Permission.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("2E839427-58D6-4567-B9AA-FBE6071590E3")]
    #endregion
    public class ExecutePermission : Permission
    {
        #region Allors
        [Id("CB76C8B7-681E-450B-A3EC-95C32E1ED5B6")]
        [Indexed]
        #endregion
        [Required]
        public Guid MethodTypePointer { get; set; }

        #region inherited

        public DelegatedAccess AccessDelegation { get; set; }
        public Revocation[] Revocations { get; set; }
        

        public SecurityToken[] SecurityTokens { get; set; }

        public Guid ClassPointer { get; set; }


        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
