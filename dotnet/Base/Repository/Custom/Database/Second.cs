// <copyright file="Second.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository;

using Attributes;

#region Allors
[Id("c1f169a1-553b-4a24-aba7-01e0b7102fe5")]
#endregion
public class Second : Object, DerivationCounted
{
    #region Allors
    [Id("4f0eba0d-09b4-4bbc-8e42-15de94921ab5")]
    [SingleAssociation]
    [Indexed]
    #endregion
    public Third Third { get; set; }

    #region Allors
    [Id("8a7b7af9-f421-4e96-a1a7-04d4c4bdd1d7")]
    #endregion
    public bool IsDerived { get; set; }

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
