// <copyright file="TraceX.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("B97A5FE8-6E74-419B-A563-E406F9F835C7")]

#endregion

public class TraceX : Object
{
    #region Unit

    #region Allors

    [Id("44242071-BE48-48D7-AEBC-6B1B088B79A2")]

    #endregion

    [Size(-1)]
    public byte[] AllorsBinary { get; set; }

    #region Allors

    [Id("14AFE99C-2CCE-4845-8E36-752468992107")]

    #endregion

    public bool AllorsBoolean { get; set; }

    #region Allors

    [Id("4A1A214F-AD8C-4BB7-9E10-7A055437291E")]

    #endregion

    public DateTime AllorsDateTime { get; set; }

    #region Allors

    [Id("BD2E3DC2-A3AF-4FF6-BF4D-3C1D17198F72")]

    #endregion

    [Precision(10)]
    [Scale(2)]
    public decimal AllorsDecimal { get; set; }

    #region Allors

    [Id("D974A73E-176F-478F-84D2-DF1657CA3552")]

    #endregion

    public double AllorsDouble { get; set; }

    #region Allors

    [Id("2BFD32AD-AB26-406B-844C-441E6E988B84")]

    #endregion

    [Indexed]
    public int AllorsInteger { get; set; }

    #region Allors

    [Id("DC0E483D-5547-4B43-B6B7-384AA510435F")]

    #endregion

    [Size(256)]
    public string AllorsString { get; set; }

    #region Allors

    [Id("41D3AD29-5E4F-4F6E-997E-FFF9B984A1C9")]

    #endregion

    public Guid AllorsUnique { get; set; }

    #endregion

    #region Composite

    #region Allors

    [Id("497B30F1-CE00-4D63-AF20-49DE6AF9D1BA")]
    [Indexed]

    #endregion

    public TraceY[] Many2Manies { get; set; }

    #region Allors

    [Id("EBEF52B2-2AF8-452B-80D4-92B60A99684E")]
    [Indexed]

    #endregion

    public TraceY Many2One { get; set; }

    #region Allors

    [Id("2920C99F-2D02-4641-BB6F-07CA88B467C8")]
    [Indexed]

    #endregion

    [SingleAssociation]
    public TraceY[] One2Manies { get; set; }

    #region Allors

    [Id("833CCA45-4AFA-4307-81A9-046C8D7AD31C")]
    [Indexed]

    #endregion

    [SingleAssociation]
    public TraceY One2One { get; set; }

    #endregion

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild() { }

    public void OnInit() { }

    public void OnPostDerive() { }

    #endregion
}
