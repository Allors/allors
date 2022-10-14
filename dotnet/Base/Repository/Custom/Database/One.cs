// <copyright file="One.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("5d9b9cad-3720-47c3-9693-289698bf3dd0")]

#endregion

public class One : Object, Shared
{
    #region Allors

    [Id("448878af-c992-4256-baa7-239335a26bc6")]
    [Indexed]

    #endregion

    public Two Two { get; set; }

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
