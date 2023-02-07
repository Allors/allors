// <copyright file="CreatePermission.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors
[Id("412994F9-4D0E-4D75-AE27-3063046869F0")]
#endregion
public partial class CreatePermission : Permission
{
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
