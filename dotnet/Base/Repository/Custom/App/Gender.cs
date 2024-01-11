// <copyright file="Gender.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("270f0dc8-1bc2-4a42-9617-45e93d5403c8")]
    #endregion
    public partial class Gender : Enumeration
    {
        #region inherited properties
        public string Key { get; set; }

        public LocalisedText[] LocalisedNames { get; set; }

        public bool IsActive { get; set; }

        public Revocation[] Revocations { get; set; }

        public DelegatedAccess AccessDelegation { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }

        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPostDerive() { }

        #endregion
    }
}
