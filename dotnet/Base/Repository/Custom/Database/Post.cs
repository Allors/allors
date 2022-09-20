// <copyright file="Post.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("4B66BA88-A452-4AE4-91E7-7FBC28E65B31")]

#endregion

public class Post : Object
{
    #region Allors

    [Id("D8714378-149D-4E4B-8A18-0D8622BCD32D")]

    #endregion

    [Required]
    public int Counter { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }
    public void OnPostBuild() { }

    public void OnInit() { }

    public void OnPostDerive() { }

    #endregion
}
