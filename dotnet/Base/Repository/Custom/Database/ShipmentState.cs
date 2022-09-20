// <copyright file="ShipmentState.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("CE56A6E9-8E4B-4F40-8676-180F4B0513E2")]

#endregion

public class ShipmentState : Object, ObjectState
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
