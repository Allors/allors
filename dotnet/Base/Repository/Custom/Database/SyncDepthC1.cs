// <copyright file="SyncDepthC1.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("4EA6AD12-C1FB-4661-B4F7-72B81435DD70")]

#endregion

public class SyncDepthC1 : SyncDepthI1
{
    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public int DerivationCount { get; set; }

    public SyncDepth2 SyncDepth2 { get; set; }

    public int Value { get; set; }


    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    #endregion
}
