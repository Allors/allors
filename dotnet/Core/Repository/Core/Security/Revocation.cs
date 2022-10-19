// <copyright file="Revocation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("753A230E-6C29-4C3C-9592-323BE0778ED6")]

#endregion

public partial class Revocation : UniquelyIdentifiable, Deletable
{
    #region Allors

    [Id("F7F98147-FD94-4BB1-A974-6405A3AB369E")]

    #endregion

    [Indexed]
    public Permission[] DeniedPermissions { get; set; }

    #region inherited

    public Guid UniqueId { get; set; }

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild() { }

    public void OnInit() { }

    public void OnPostDerive(OnPostDeriveInput input) { }

    public void Delete() { }

    #endregion
}
