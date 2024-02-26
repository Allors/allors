// <copyright file="C4.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors
[Id("20049a79-20c7-478b-a5ba-c54b1e615168")]
#endregion
public class C4 : Object, I4, I34
{
    #region Allors
    [Id("9f24fc51-8568-4ffc-b47a-c5c317d00954")]
    [Size(256)]
    #endregion
    public string C4AllorsString { get; set; }

    #region inherited properties
    public string Name { get; set; }

    public double S1234AllorsDouble { get; set; }

    public decimal S1234AllorsDecimal { get; set; }

    public int S1234AllorsInteger { get; set; }

    public S1234 S1234many2one { get; set; }

    public C2 S1234C2one2one { get; set; }

    public C2[] S1234C2many2manies { get; set; }

    public S1234[] S1234one2manies { get; set; }

    public C2[] S1234C2one2manies { get; set; }

    public S1234[] S1234many2manies { get; set; }

    public string ClassName { get; set; }

    public DateTime S1234AllorsDateTime { get; set; }

    public S1234 S1234one2one { get; set; }

    public C2 S1234C2many2one { get; set; }

    public string S1234AllorsString { get; set; }

    public bool S1234AllorsBoolean { get; set; }

    public decimal I34AllorsDecimal { get; set; }

    public bool I34AllorsBoolean { get; set; }

    public double I34AllorsDouble { get; set; }

    public int I34AllorsInteger { get; set; }

    public string I34AllorsString { get; set; }
    #endregion

    #region inherited methods
    public void OnBuild()
    {
    }

    public void OnPostBuild()
    {
    }

    #endregion
}
