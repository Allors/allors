// <copyright file="Locale.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Attributes;
    
    #region Allors
    [Id("45033ae6-85b5-4ced-87ce-02518e6c27fd")]
    #endregion
    public partial class Locale : Enumeration
    {
        #region Allors
        [Id("d8cac34a-9bb2-4190-bd2a-ec0b87e04cf5")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Required]
        public Language Language { get; set; }

        #region Allors
        [Id("ea778b77-2929-4ab4-ad99-bf2f970401a9")]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Country Country { get; set; }

        #region inherited methods
        public Revocation[] Revocations { get; set; }
        public DelegatedAccess AccessDelegation { get; set; }
        public SecurityToken[] SecurityTokens { get; set; }
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
