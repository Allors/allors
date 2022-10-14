// <copyright file="MailboxAddress.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("7ee3b00b-4e63-4774-b744-3add2c6035ab")]

#endregion

[Plural("MailboxAddresses")]
public class MailboxAddress : Object, Address
{
    #region Allors

    [Id("03c9970e-d9d6-427d-83d0-00e0888f5588")]
    [Size(256)]

    #endregion

    public string PoBox { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public Place Place { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    #endregion
}
