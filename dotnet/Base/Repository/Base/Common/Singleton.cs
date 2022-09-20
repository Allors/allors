// <copyright file="Singleton.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("313b97a5-328c-4600-9dd2-b5bc146fb13b")]

#endregion

public partial class Singleton : Object
{
    #region Allors

    [Id("9c1634ab-be99-4504-8690-ed4b39fec5bc")]

    #endregion

    [Indexed]
    public Locale DefaultLocale { get; set; }

    #region Allors

    [Id("9e5a3413-ed33-474f-adf2-149ad5a80719")]

    #endregion

    [SingleAssociation]
    [Indexed]
    public Locale[] AdditionalLocales { get; set; }

    #region Allors

    [Id("615AC72B-B3DF-4057-9B7C-C8528341F5FE")]

    #endregion

    [SingleAssociation]
    [Indexed]
    [Derived]

    public Locale[] Locales { get; set; }

    #region Allors

    [Id("B2166062-84DA-449D-B34F-983A0C81BC31")]

    #endregion

    [Indexed]
    public Media LogoImage { get; set; }

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
