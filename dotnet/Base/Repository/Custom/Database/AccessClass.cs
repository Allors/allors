// <copyright file="AccessClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;

#region Allors
[Id("8E66F2E1-27FA-4A1C-B410-F082CA1621C7")]
#endregion
public class AccessClass : AccessInterface
{
    #region Allors
    [Id("A67189D3-CD06-425B-98BB-59E0E73AC211")]
    #endregion
    public string Property { get; set; }

    #region inherited
    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild()
    {
    }

    public void OnInit()
    {
    }

    public void OnPostDerive()
    {
    }
    #endregion
}
