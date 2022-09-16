// <copyright file="ValidationC2.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Attributes;

#region Allors
[Id("c7563dd3-77b2-43ff-92f9-a4f98db36acf")]
#endregion
public class ValidationC2 : Object, ValidationI12
{
    #region inherited
    public Guid UniqueId { get; set; }

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive() { }
    #endregion
}
