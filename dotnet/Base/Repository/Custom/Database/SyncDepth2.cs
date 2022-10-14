// <copyright file="SyncDepth2.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("B9996F8F-12FB-4E42-8B7F-907433A622B2")]

#endregion

public class SyncDepth2 : Object, DerivationCounted
{
    #region Allors

    [Id("C6254113-00CB-475E-AE15-B45FC3E623BC")]

    #endregion

    [Required]
    public int Value { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public int DerivationCount { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    #endregion
}
