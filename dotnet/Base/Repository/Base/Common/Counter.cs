// <copyright file="Counter.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("0568354f-e3d9-439e-baac-b7dce31b956a")]

#endregion

public class Counter : UniquelyIdentifiable
{
    #region Allors

    [Id("309d07d9-8dea-4e99-a3b8-53c0d360bc54")]

    #endregion

    [Required]
    public int Value { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public Guid UniqueId { get; set; }

    public void OnPostBuild()
    {
    }

    public void OnInit()
    {
    }

    public void OnPostDerive()
    {
    }

    #endregion
}
