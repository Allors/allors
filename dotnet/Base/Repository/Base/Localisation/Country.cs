// <copyright file="Country.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("c22bf60e-6428-4d10-8194-94f7be396f28")]
    #endregion
    [Plural("Countries")]
    public partial class Country : Enumeration
    {
        #region inherited properties
        public Revocation[] Revocations { get; set; }
        public DelegatedAccess AccessDelegation { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
        #endregion

        #region Allors
        [Id("62009cef-7424-4ec0-8953-e92b3cd6639d")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Currency Currency { get; set; }

        #region inherited
        public string Key { get; set; }
        public LocalisedText[] LocalisedNames { get; set; }
        public bool IsActive { get; set; }

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPostDerive() { }
        #endregion
    }
}
