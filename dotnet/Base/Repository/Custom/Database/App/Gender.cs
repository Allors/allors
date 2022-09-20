// <copyright file="Gender.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("270f0dc8-1bc2-4a42-9617-45e93d5403c8")]

#endregion

public class Gender : Enumeration
{
    #region inherited

    public LocalizedText[] LocalizedNames { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }


    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public Guid UniqueId { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive() { }

    #endregion
}
