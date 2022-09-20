// <copyright file="WorkspaceXYObject2.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;


#region Allors
[Id("04B46AEE-10E9-493B-9F12-809460E341C7")]
#endregion

public class WorkspaceXYObject2 : Object
{
    #region inherited
    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive() { }
    #endregion
}
