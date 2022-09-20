// <copyright file="ValiData.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;


#region Allors
[Id("CC635728-B7AE-4A07-BBF1-E16AEEC07750")]
#endregion

public class ValiData : Object
{
    #region Allors
    [Id("C90E7744-9AFD-46A2-9F6F-3D76D681106A")]
    [Indexed]
    #endregion
    
    [Required]
    public Person RequiredPerson { get; set; }

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
