// <copyright file="Currency.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Attributes;
    
    #region Allors
    [Id("fd397adf-40b4-4ef8-b449-dd5a24273df3")]
    #endregion
    [Plural("Currencies")]
    public partial class Currency : Enumeration
    {
        #region inherited
        public Revocation[] Revocations { get; set; }
        public DelegatedAccess AccessDelegation { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        public string Key { get; set; }
        public LocalisedText[] LocalisedNames { get; set; }
        public bool IsActive { get; set; }
        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPostDerive() { }
        #endregion
    }
}
