// <copyright file="ReadPermission.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("0716C285-841C-419B-A8C4-A67BFA585CDA")]

#endregion

public class ReadPermission : Permission
{
    #region Allors

    [Id("88A27D41-E97E-4446-86D7-2E2FC10C5004")]
    [Indexed]

    #endregion

    [Required]
    public Guid RelationTypePointer { get; set; }

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
