// <copyright file="Employment.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("CE633852-D115-468E-A52F-22A777E27198")]

#endregion

public class Employment : Period, Deletable
{
    #region Allors

    [Id("93B8F2E1-9902-4C0B-BFAC-74629C494346")]
    [Indexed]

    #endregion

    [Required]
    public Person Employee { get; set; }

    #region Allors

    [Id("75FCF8E7-1DB4-466B-9428-5A9E45467D15")]
    [Indexed]

    #endregion

    [Required]
    public Organization Employer { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public DateTime FromDate { get; set; }
    public DateTime ThroughDate { get; set; }

    public void OnPostBuild() { }

    public void OnInit() { }

    public void OnPostDerive() { }

    public void Delete() { }

    #endregion
}
