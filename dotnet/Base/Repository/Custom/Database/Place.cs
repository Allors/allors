// <copyright file="Place.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("268f63d2-17da-4f29-b0d0-76db611598c6")]

#endregion

public class Place : Object
{
    #region Allors

    [Id("1bf1cc1e-75bf-4a3f-87bd-a2fae2697855")]
    [Indexed]

    #endregion

    public Country Country { get; set; }

    #region Allors

    [Id("d029f486-4bb8-43a1-8356-98b9bee10de4")]
    [Size(256)]

    #endregion

    public string City { get; set; }

    #region Allors

    [Id("d80d7c6a-138a-43dd-9748-8ffb89b1dabb")]
    [Size(256)]

    #endregion

    public string PostalCode { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }


    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    #endregion
}
