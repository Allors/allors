// <copyright file="WritePermission.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors
[Id("4F00E50D-4324-4005-A405-6DFD1232982A")]
#endregion
public partial class WritePermission : Permission
{
    #region Allors
    [Id("86675DEA-D9F0-4930-99EC-13F2137CFB45")]
    #endregion
    [Indexed]
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
