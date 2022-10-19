// <copyright file="Currency.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("fd397adf-40b4-4ef8-b449-dd5a24273df3")]

#endregion

[Plural("Currencies")]
public partial class Currency : Enumeration, Object
{
    #region Allors

    [Id("294a4bdc-f03a-47a2-a649-419e6b9021a3")]

    #endregion

    [Required]
    [Size(256)]
    public string IsoCode { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public Guid UniqueId { get; set; }

    public string Name { get; set; }

    public LocalizedText[] LocalizedNames { get; set; }

    public bool IsActive { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    #endregion
}
