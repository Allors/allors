// <copyright file="TaskAssignment.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("4092d0b4-c6f4-4b81-b023-66be3f4c90bd")]

#endregion

public class TaskAssignment : Deletable, Object
{
    #region Allors

    [Id("c32c19f1-3f41-4d11-b19d-b8b2aa360166")]
    [Indexed]

    #endregion

    [Required]
    public User User { get; set; }

    #region Allors

    [Id("f4e05932-89c0-4f40-b4b2-f241ac42d8a0")]
    [SingleAssociation]
    [Indexed]

    #endregion

    public Notification Notification { get; set; }

    #region Allors

    [Id("8a01f221-480f-4d61-9a12-72e3689a8224")]
    [Indexed]

    #endregion

    [Required]
    public Task Task { get; set; }

    #region inherited

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
