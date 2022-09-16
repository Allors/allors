// <copyright file="Build.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Attributes;

#region Allors
[Id("FFFCC7BD-252E-4EE7-B825-99CCBE2D5F49")]
#endregion
public class Build : Object
{
    #region Allors
    [Id("A3DED776-B516-4C38-9B5F-5DEBFAFD15CB")]
    #endregion
    public Guid Guid { get; set; }

    #region Allors
    [Id("19112701-B610-49FC-82B8-FB786EEBCDB4")]
    #endregion
    public string String { get; set; }

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
