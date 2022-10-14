// <copyright file="Middle.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region

[Id("62173428-589F-43FA-8FA5-5579F084B8E3")]

#endregion

public class Middle : DerivationCounted
{
    #region Allors

    [Id("27D0ABFD-EBA0-46FE-812C-C67D8E3D12D0")]
    [Indexed]

    #endregion

    [SingleAssociation]
    public Right Right { get; set; }

    #region Allors

    [Id("4616201B-7C52-4C5D-B390-4D9C0A8CADAD")]

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

    public void OnPostDerive(OnPostDeriveInput input)
    {
    }

    #endregion
}
