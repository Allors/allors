// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;
    using Attributes;

    public partial interface Object
    {
        #region Allors
        [Id("b816fccd-08e0-46e0-a49c-7213c3604416")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        SecurityToken[] SecurityTokens { get; set; }

        #region Allors
        [Id("E989F7D2-A4AC-43D8-AC7C-CBCDA2CFB6D3")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Derived]
        Revocation[] Revocations { get; set; }

        #region Allors
        [Id("D85377D7-49D9-4E3A-8FB2-F80CC1542141")]
        #endregion
        [Derived]
        Guid SecurityFingerPrint { get; set; }
    }
}
