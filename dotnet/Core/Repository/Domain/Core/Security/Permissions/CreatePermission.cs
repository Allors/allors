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
    [Id("412994F9-4D0E-4D75-AE27-3063046869F0")]
    #endregion
    public class CreatePermission : Permission
    {

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
