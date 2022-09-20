// <copyright file="DelegatedAccess.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("86AE10F1-9E60-4347-9046-D6A68E1B9381")]

#endregion

public class DelegatedAccess : UniquelyIdentifiable, Deletable
{
    #region Allors

    [Id("E40496A8-21E5-48C9-9F9E-AF1985387698")]

    #endregion

    [Indexed]
    public SecurityToken[] DelegatedSecurityTokens { get; set; }

    #region Allors

    [Id("50A5189E-2E1E-44A5-A629-B5DB2E8FDBB5")]

    #endregion

    [Indexed]
    [Derived]
    public Revocation[] DelegatedRevocations { get; set; }

    #region inherited

    public SecurityToken[] SecurityTokens { get; set; }

    public DelegatedAccess AccessDelegation { get; set; }

    public Revocation[] Revocations { get; set; }


    public Guid UniqueId { get; set; }

    public void OnPostBuild() { }

    public void OnInit() { }

    public void OnPostDerive() { }

    public void Delete() { }

    #endregion
}
