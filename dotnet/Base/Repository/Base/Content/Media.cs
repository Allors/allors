// <copyright file="Media.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("da5b86a3-4f33-4c0d-965d-f4fbc1179374")]

#endregion

public class Media : UniquelyIdentifiable, Deletable, Object
{
    #region Allors

    [Id("B74C2159-739A-4F1C-ADA7-C2DCC3CDCF83")]

    #endregion
    [Indexed]
    [Derived]
    public Guid Revision { get; set; }

    #region Allors

    [Id("67082a51-1502-490b-b8db-537799e550bd")]

    #endregion
    [SingleAssociation]
    [Indexed]
    [Required]
    public MediaContent MediaContent { get; set; }

    #region Allors

    [Id("DCBF8D02-84A5-4C1B-B2C1-16D6E97EEA06")]

    #endregion
    [Indexed]
    [Size(1024)]
    public string InType { get; set; }

    #region Allors

    [Id("18236718-1835-430C-A936-7EC461EEE2CF")]

    #endregion
    [Size(-1)]
    public byte[] InData { get; set; }

    #region Allors

    [Id("79B04065-F13B-43B3-B86E-F3ADBBAAF0C4")]

    #endregion
    [Size(-1)]
    public string InDataUri { get; set; }

    #region Allors

    [Id("E03239E9-2039-49DC-9615-36CEA3C971D3")]

    #endregion
    [Size(256)]
    public string InFileName { get; set; }

    #region Allors

    [Id("DDD6C005-0104-44CA-A19C-1150B8BEB4A3")]

    #endregion
    [Indexed]
    [Size(256)]
    public string Name { get; set; }

    #region Allors

    [Id("29541613-0B16-49AD-8F40-3309A7C7D7B8")]

    #endregion
    [Indexed]
    [Size(1024)]
    [Derived]
    public string Type { get; set; }

    #region Allors

    [Id("AC462C32-3945-4C39-BAEF-9D228EEA80A6")]

    #endregion
    [Indexed]
    [Size(256)]
    [Derived]
    public string FileName { get; set; }

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

    public void Delete() { }

    #endregion
}
