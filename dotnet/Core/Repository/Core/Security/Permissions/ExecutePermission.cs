// <copyright file="ExecutePermission.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("2E839427-58D6-4567-B9AA-FBE6071590E3")]

#endregion

public partial class ExecutePermission : Permission
{
    #region Allors
    [Id("CB76C8B7-681E-450B-A3EC-95C32E1ED5B6")]
    #endregion
    [Indexed]
    [Required]
    public Guid MethodTypePointer { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public Guid ClassPointer { get; set; }


    public void OnPostBuild() { }

    public void OnInit() { }

    public void OnPostDerive() { }

    public void Delete() { }

    #endregion
}
