// <copyright file="TrimTo.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("C04D372D-6D58-4EA7-AFF8-33208D4A9519")]

#endregion

public class TrimTo : Object
{
    #region Allors

    [Id("09E9D2E5-E406-4D3B-9E3D-5AA3D69408CF")]

    #endregion

    [Size(256)]
    public string Name { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild() { }

    public void OnInit() { }

    public void OnPostDerive(OnPostDeriveInput input) { }

    #endregion
}
