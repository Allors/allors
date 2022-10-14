// <copyright file="Person.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("c799ca62-a554-467d-9aa2-1663293bb37f")]

#endregion

public partial class Person : User
{
    #region Allors

    [Id("ed4b710a-fe24-4143-bb96-ed1bd9beae1a")]

    #endregion

    [Size(256)]
    public string FirstName { get; set; }

    #region Allors

    [Id("eb18bb28-da9c-47b4-a091-2f8f2303dcb6")]

    #endregion

    [Size(256)]
    public string MiddleName { get; set; }

    #region Allors

    [Id("8a3e4664-bb40-4208-8e90-a1b5be323f27")]

    #endregion

    [Size(256)]
    public string LastName { get; set; }

    #region inherited

    public Guid UniqueId { get; set; }

    public SecurityToken OwnerSecurityToken { get; set; }

    public Grant OwnerGrant { get; set; }


    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public string UserName { get; set; }

    public string NormalizedUserName { get; set; }

    public string InExistingUserPassword { get; set; }

    public string InUserPassword { get; set; }

    public string UserPasswordHash { get; set; }

    public string UserEmail { get; set; }

    public string NormalizedUserEmail { get; set; }

    public bool UserEmailConfirmed { get; set; }

    public string UserSecurityStamp { get; set; }

    public string UserPhoneNumber { get; set; }

    public bool UserPhoneNumberConfirmed { get; set; }

    public bool UserTwoFactorEnabled { get; set; }

    public DateTime UserLockoutEnd { get; set; }

    public bool UserLockoutEnabled { get; set; }

    public int UserAccessFailedCount { get; set; }

    public Login[] Logins { get; set; }

    public NotificationList NotificationList { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    public void Delete()
    {
    }

    #endregion
}
