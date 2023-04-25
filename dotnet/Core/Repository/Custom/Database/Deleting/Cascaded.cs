// <copyright file="C1.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("5727ED51-3C7E-4D3A-9CC7-744E8128E9ED")]
    #endregion
    public partial class Cascaded : Deletable
    {
        #region inherited properties
        public Revocation[] Revocations { get; set; }

        public DelegatedAccess AccessDelegation { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region inherited methods

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
