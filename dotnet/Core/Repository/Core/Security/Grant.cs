// <copyright file="Grant.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("c4d93d5e-34c3-4731-9d37-47a8e801d9a8")]

#endregion

public partial class Grant : UniquelyIdentifiable, Deletable
{
    #region Allors

    [Id("0dbbff5c-3dca-4257-b2da-442d263dcd86")]

    #endregion

    [Indexed]
    public UserGroup[] SubjectGroups { get; set; }

    #region Allors

    [Id("37dd1e27-ba75-404c-9410-c6399d28317c")]

    #endregion

    [Indexed]
    public User[] Subjects { get; set; }

    #region Allors

    [Id("69a9dae8-678d-4c1c-a464-2e5aa5caf39e")]

    #endregion

    [Indexed]
    [Required]
    public Role Role { get; set; }

    #region Allors

    [Id("5e218f37-3b07-4002-87a4-7581a53f01ba")]

    #endregion

    [Indexed]
    [Derived]
    public Permission[] EffectivePermissions { get; set; }

    #region Allors

    [Id("50ecae85-e5a9-467e-99a3-78703d954b2f")]

    #endregion

    [Indexed]
    [Derived]
    public User[] EffectiveUsers { get; set; }

    #region inherited

    public Guid UniqueId { get; set; }

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }


    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    public void Delete() { }

    #endregion
}
