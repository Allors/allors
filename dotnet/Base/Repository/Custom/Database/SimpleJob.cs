// <copyright file="SimpleJob.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("320985b6-d571-4b6c-b940-e02c04ad37d3")]

#endregion

public class SimpleJob : Object
{
    #region Allors

    [Id("7cd27660-13c6-4a15-8fd8-5775920cfd28")]

    #endregion

    public int Index { get; set; }

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
