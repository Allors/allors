// <copyright file="TrimFrom.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("A32F39D4-2A0F-4270-9CE9-0FADB1121113")]

#endregion

public class TrimFrom : Object
{
    #region Allors

    [Id("F5EEBEF2-A317-4A4A-9D51-3B00FCFBF7F9")]

    #endregion

    [Size(256)]
    public string Name { get; set; }

    #region Allors

    [Id("11DD2A3B-5C61-4E95-93DD-F1B8BDB14EB1")]
    [Indexed]

    #endregion

    public TrimTo[] Many2Manies { get; set; }

    #region Allors

    [Id("0F01CBAE-6991-4F5E-B788-C8F4AB799D91")]
    [Indexed]

    #endregion

    public TrimTo Many2One { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild() { }

    public void OnInit() { }

    public void OnPostDerive() { }

    #endregion
}
