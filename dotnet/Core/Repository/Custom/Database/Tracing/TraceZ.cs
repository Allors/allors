// <copyright file="TraceZ.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Attributes;


#region Allors
[Id("E8A343E3-31ED-4839-9365-79CBFF561C4D")]
#endregion

public class TraceZ : Object
{
    #region Unit
    #region Allors
    [Id("D6F48267-A9F2-4237-94D3-7E3EF73EE0A9")]
    #endregion
    [Size(-1)]
    
    public byte[] AllorsBinary { get; set; }

    #region Allors
    [Id("7CBFEA75-4945-4A72-943A-3E70F4B1913F")]
    #endregion
    
    public bool AllorsBoolean { get; set; }

    #region Allors
    [Id("8C2FC40C-9C8C-42EA-8D19-A17278F84178")]
    #endregion
    
    public DateTime AllorsDateTime { get; set; }

    #region Allors
    [Id("5D5928D3-9569-4462-BA6C-CF54F1D018E9")]
    #endregion
    [Precision(10)]
    [Scale(2)]
    
    public decimal AllorsDecimal { get; set; }

    #region Allors
    [Id("5E7E7311-05C6-46BE-9E5D-94328185FA7B")]
    #endregion
    
    public double AllorsDouble { get; set; }

    #region Allors
    [Id("A917D0C9-E1CA-460E-BD84-ED3BA17608A5")]
    #endregion
    [Indexed]
    
    public int AllorsInteger { get; set; }

    #region Allors
    [Id("0C9F25F0-C231-48D8-997E-9C443E570FBF")]
    #endregion
    [Size(256)]
    
    public string AllorsString { get; set; }

    #region Allors
    [Id("C82C7CDB-19FD-4900-AED3-0A330C66FCDE")]
    #endregion
    
    public Guid AllorsUnique { get; set; }
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
