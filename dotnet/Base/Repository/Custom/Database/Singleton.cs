// <copyright file="Singleton.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

public partial class Singleton : Object
{
    #region Allors

    [Id("AD981D2E-32E8-4DC6-91A2-F8A2F44086F3")]

    #endregion

    [Indexed]
    public Person AutocompleteDefault { get; set; }

    #region Allors

    [Id("E459A1DF-8866-4B56-9EB2-F8F890BC67ED")]

    #endregion

    [Indexed]
    public Person SelectDefault { get; set; }
}
