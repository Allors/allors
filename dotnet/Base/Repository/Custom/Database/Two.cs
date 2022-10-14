// <copyright file="Two.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("9ec7e136-815c-4726-9991-e95a3ec9e092")]

#endregion

public class Two : Object, Shared
{
    #region Allors

    [Id("8930c13c-ad5a-4b0e-b3bf-d7cdf6f5b867")]
    [Indexed]

    #endregion

    public Shared Shared { get; set; }

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
