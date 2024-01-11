// <copyright file="Language.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Attributes;

    #region Allors
    [Id("4a0eca4b-281f-488d-9c7e-497de882c044")]
    #endregion
    public partial class Language : Enumeration
    {
        #region Allors
        [Id("842CC899-3F37-455A-AE91-51D29D615E69")]
        #endregion
        [Indexed]
        [Required]
        // [Unique] If Unique is enabled then make sure your database supports the range of unicode characters (e.g. use collation 'Latin1_General_100_CI_AS_SC' in sql server)
        [Size(256)]
        public string NativeName { get; set; }

        #region inherited
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
