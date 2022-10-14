// <copyright file="Left.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region

[Id("DBAF849D-E8B0-4CEA-85E0-DFB934A06F96")]

#endregion

public class Left : DerivationCounted
{
    #region Allors

    [Id("C86BBB90-F678-4627-B651-657F86B2D2EB")]
    [Indexed]

    #endregion

    [SingleAssociation]
    public Middle Middle { get; set; }

    #region Allors

    [Id("92CF5496-063D-428E-9A24-F36321A10675")]

    #endregion

    [Required]
    public int Counter { get; set; }

    #region Allors

    [Id("8C454674-AE11-4305-A055-55A915139F16")]

    #endregion

    [Required]
    public bool CreateMiddle { get; set; }

    #region inherited

    public int DerivationCount { get; set; }


    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

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
