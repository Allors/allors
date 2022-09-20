// <copyright file="MediaTyped.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;


#region Allors
[Id("355AEFD2-F5B2-499A-81D2-DD9C9F62832C")]
#endregion
public class MediaTyped : Object
{
    #region Allors
    [Id("D23961DF-6688-44D1-87D4-0E5D0C2ED533")]
    #endregion
    [Size(-1)]
    [MediaType("text/markdown")]
    
    public string Markdown { get; set; }

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
