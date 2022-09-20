// <copyright file="Right.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region

[Id("E4BC4E69-831C-4D9B-93D9-531D226819E1")]

#endregion

public class Right : DerivationCounted
{
    #region Allors

    [Id("658FE4F7-FC40-4B3A-ABB1-84723E66F20C")]

    #endregion

    [Required]
    public int Counter { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public int DerivationCount { get; set; }

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
