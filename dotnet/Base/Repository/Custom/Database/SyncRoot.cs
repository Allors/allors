// <copyright file="SyncRoot.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("2A863DCF-C6FE-4838-8D3A-1212A2076D70")]

#endregion

public class SyncRoot : Object, DerivationCounted
{
    #region Allors

    [Id("615C6C58-513A-456F-A0CE-E472D173DCB0")]
    [SingleAssociation]
    [Indexed]

    #endregion

    [Derived]
    public SyncDepthI1 SyncDepth1 { get; set; }

    #region Allors

    [Id("4061BB19-494D-4CD4-AE7F-798FC62942AB")]

    #endregion

    [Required]
    public int Value { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public int DerivationCount { get; set; }


    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive() { }

    #endregion
}
