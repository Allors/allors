// <copyright file="AccessClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;
using static Workspaces;

#region Allors
[Id("8E66F2E1-27FA-4A1C-B410-F082CA1621C7")]
#endregion
[Workspace(Default)]
public class AccessClass : AccessInterface
{
    #region Allors
    [Id("A67189D3-CD06-425B-98BB-59E0E73AC211")]
    #endregion
    [Workspace(Default)]
    public string Property { get; set; }

    #region Allors
    [Id("4DB6E69E-F870-4643-A2F6-D3C700FBB431")]
    #endregion
    [Workspace(Default)]
    public string AnotherProperty { get; set; }

    #region inherited
    public SecurityToken[] SecurityTokens { get; set; }

    public DelegatedAccess AccessDelegation { get; set; }

    public Revocation[] Revocations { get; set; }


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
