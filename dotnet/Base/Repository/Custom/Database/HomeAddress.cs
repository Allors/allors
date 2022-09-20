// <copyright file="HomeAddress.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("2561e93c-5b85-44fb-a924-a1c0d1f78846")]

#endregion

[Plural("HomeAddresses")]
public class HomeAddress : Object, Address
{
    #region Allors

    [Id("6f0f42c4-9b47-47c2-a632-da8e08116be4")]
    [Size(256)]

    #endregion

    public string Street { get; set; }

    #region Allors

    [Id("b181d077-e897-4add-9456-67b9760d32e8")]
    [Size(256)]

    #endregion

    public string HouseNumber { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public Place Place { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive() { }

    #endregion
}
