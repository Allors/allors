// <copyright file="SecurityToken.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("a53f1aed-0e3f-4c3c-9600-dc579cccf893")]

#endregion

public partial class SecurityToken : UniquelyIdentifiable, Deletable
{
    #region Allors

    [Id("6503574b-8bab-4da8-a19d-23a9bcffe01e")]

    #endregion

    [Indexed]
    public Grant[] Grants { get; set; }

    #region Allors

    [Id("E094E1DD-A3B0-4B6A-B2FA-B00E98BDC0D6")]

    #endregion

    [Derived]
    public Guid Fingerprint { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public Guid UniqueId { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    public void Delete() { }

    #endregion
}
