// <copyright file="Third.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;

#region Allors
[Id("39116edf-34cf-45a6-ac09-2e4f98f28e14")]
#endregion
public class Third : Object, DerivationCounted
{
    #region Allors
    [Id("6ab5a7af-a0f0-4940-9be3-6f6430a9e728")]
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
