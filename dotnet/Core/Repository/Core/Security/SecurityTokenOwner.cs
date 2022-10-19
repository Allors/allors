// <copyright file="SecurityTokenOwner.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("a69cad9c-c2f1-463f-9af1-873ce65aeea6")]

#endregion

public partial interface SecurityTokenOwner : Object
{
    #region Allors

    [Id("5fb15e8b-011c-46f7-83dd-485d4cc4f9f2")]

    #endregion

    [SingleAssociation]
    [Indexed]
    [Required]
    [Derived]
    SecurityToken OwnerSecurityToken { get; set; }

    #region Allors

    [Id("056914ed-a658-4ae5-b859-97300e1b8911")]

    #endregion

    [SingleAssociation]
    [Indexed]
    [Derived]
    Grant OwnerGrant { get; set; }
}
