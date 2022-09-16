// <copyright file="Country.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Attributes;
using static Workspaces;

#region Allors
[Id("c22bf60e-6428-4d10-8194-94f7be396f28")]
#endregion
[Plural("Countries")]
[Workspace(Default)]
public class Country : Object
{
    #region Allors
    [Id("62009cef-7424-4ec0-8953-e92b3cd6639d")]
    #endregion
    [Indexed]
    [Workspace(Default)]
    public Currency Currency { get; set; }

    #region Allors
    [Id("f93acc4e-f89e-4610-ada9-e58f21c165bc")]
    #endregion
    [Required]
    [Size(2)]
    [Workspace(Default)]
    public string IsoCode { get; set; }

    #region Allors
    [Id("6b9c977f-b394-440e-9781-5d56733b60da")]
    #endregion
    [Indexed]
    [Size(256)]
    [Required]
    [Workspace(Default)]
    public string Name { get; set; }

    #region Allors
    [Id("8236a702-a76d-4bb5-9afd-acacb1508261")]
    #endregion
    [SingleAssociation]
    [Indexed]
    [Workspace(Default)]
    public LocalizedText[] LocalizedNames { get; set; }

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
