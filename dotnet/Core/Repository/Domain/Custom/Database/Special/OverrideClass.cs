// <copyright file="UnitSample.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;
    using Attributes;
    using static Workspaces;

    #region Allors
    [Id("EF49F009-8532-429D-988D-B2626BD4D56E")]
    #endregion
    [Workspace(Default)]
    public partial class OverrideClass : OverrideInterface
    {
        [Required]
        public string OverrideRequired { get; set; }

        #region inherited

        public Revocation[] Revocations { get; set; }
        public Guid SecurityFingerPrint { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPostDerive() { }
        #endregion
    }
}
