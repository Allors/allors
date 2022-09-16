// <copyright file="Extender.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;

#region Allors
[Id("830cdcb1-31f1-4481-8399-00c034661450")]
#endregion
public class Extender : Object
{
    #region Allors
    [Id("525bbc9e-d488-419f-ac02-0ab6ac409bac")]
    [Size(256)]
    #endregion
    public string AllorsString { get; set; }

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
