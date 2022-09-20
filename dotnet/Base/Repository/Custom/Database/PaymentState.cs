// <copyright file="PaymentState.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("07E8F845-5ECC-4B42-83EF-BB86E6B10A69")]

#endregion

public class PaymentState : Object, ObjectState
{
    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public Revocation ObjectRevocation { get; set; }

    public string Name { get; set; }

    public Guid UniqueId { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive() { }

    #endregion
}
