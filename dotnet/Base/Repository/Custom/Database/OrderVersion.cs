// <copyright file="OrderVersion.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Attributes;

#region
[Id("6A3A9167-9A77-491E-A1C8-CCFE4572AFB4")]
#endregion
public class OrderVersion : Version
{
    #region Allors
    [Id("88BE9AFA-122A-469B-BD47-388ECC835EAB")]
    [Indexed]
    #endregion

    public OrderState OrderState { get; set; }

    #region Allors
    [Id("F144557C-B63C-49F7-B713-F2493BCA1E55")]
    [Indexed]
    #endregion

    public OrderLine[] OrderLines { get; set; }

    #region Allors
    [Id("49D672D8-3B3D-473A-B050-411251AE5365")]
    #endregion
    [Derived]
    public decimal Amount { get; set; }

    #region inherited
    public Guid DerivationId { get; set; }

    public DateTime DerivationTimeStamp { get; set; }


    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild()
    {
    }

    public void OnInit()
    {
    }

    public void OnPostDerive()
    {
    }
    #endregion
}
