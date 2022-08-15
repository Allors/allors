// <copyright file="SecurityTokenSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("86AE10F1-9E60-4347-9046-D6A68E1B9381")]
    #endregion
    public partial class SecurityTokenGroup : SecurityStamped, UniquelyIdentifiable, Deletable
    {
        #region Allors
        [Id("8FC9E101-26B2-4834-8EF4-B433C1EECD21")]
        #endregion
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public SecurityToken[] Members { get; set; }

        #region inherited
        public Guid SecurityStamp { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public SecurityTokenGroup SharedSecurity { get; set; }

        public Revocation[] Revocations { get; set; }

        public Guid SecurityFingerPrint { get; set; }

        public Guid UniqueId { get; set; }

        public void OnPostBuild() { }

        public void OnInit() { }

        public void OnPostDerive() { }

        public void Delete() { }

        #endregion
    }
}
