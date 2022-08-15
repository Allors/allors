// <copyright file="SecurityToken.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using System;
    using Attributes;

    #region Allors
    [Id("D62A834D-A415-4EC9-AB84-261F6EDD57A4")]
    #endregion
    public partial interface SecurityStamped : Object
    {
        #region Allors
        [Id("E094E1DD-A3B0-4B6A-B2FA-B00E98BDC0D6")]
        #endregion
        [Derived]
        public Guid SecurityStamp { get; set; }
    }
}
