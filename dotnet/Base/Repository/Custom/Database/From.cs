// <copyright file="From.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;

#region Allors
[Id("6217b428-4ad0-4f7f-ad4b-e334cf0b3ab1")]
#endregion
public class From : Object
{
    #region Allors
    [Id("d9a9896d-e175-410a-9916-9261d83aa229")]
    [SingleAssociation]
    [Indexed]
    #endregion
    public To[] Tos { get; set; }

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
