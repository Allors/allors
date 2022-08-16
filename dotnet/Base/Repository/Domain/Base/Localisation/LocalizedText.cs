// <copyright file="LocalizedText.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;
    using Attributes;
    using static Workspaces;


    #region Allors
    [Id("020f5d4d-4a59-4d7b-865a-d72fc70e4d97")]
    #endregion
    [Workspace(Default)]
    public partial class LocalizedText : Localized, Deletable, Object
    {
        #region Allors
        [Id("50dc85f0-3d22-4bc1-95d9-153674b89f7a")]
        #endregion
        [Size(-1)]
        [Workspace(Default)]
        public string Text { get; set; }

        #region inherited

        
        public DelegatedAccess AccessDelegation { get; set; }
        public Revocation[] Revocations { get; set; }
        public Guid SecurityFingerPrint { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Locale Locale { get; set; }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPostDerive() { }

        public void Delete() { }
        #endregion
    }
}
