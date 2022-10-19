// <copyright file="UserGroup.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("60065f5d-a3c2-4418-880d-1026ab607319")]

#endregion

public partial class UserGroup : UniquelyIdentifiable
{
    #region Allors

    [Id("585bb5cf-9ba4-4865-9027-3667185abc4f")]

    #endregion

    [Indexed]
    public User[] Members { get; set; }

    #region Allors

    [Id("e94e7f05-78bd-4291-923f-38f82d00e3f4")]

    #endregion

    [Indexed]
    [Required]
    [Size(256)]
    public string Name { get; set; }

    #region inherited

    public Guid UniqueId { get; set; }

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    #endregion
}
