// <copyright file="ClassWithoutRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;

#region Allors
[Id("e1008840-6d7c-4d44-b2ad-1545d23f90d8")]
#endregion
[Plural("ClassWithourRoleses")]
public class ClassWithoutRoles : Object
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
