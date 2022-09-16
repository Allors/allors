// <copyright file="Singleton.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;
using static Workspaces;

public partial class Singleton : Object
{
    #region Allors
    [Id("AD981D2E-32E8-4DC6-91A2-F8A2F44086F3")]
    #endregion
    [Workspace(Default)]
    [Indexed]
    public Person AutocompleteDefault { get; set; }

    #region Allors
    [Id("E459A1DF-8866-4B56-9EB2-F8F890BC67ED")]
    #endregion
    [Workspace(Default)]
    [Indexed]
    public Person SelectDefault { get; set; }
}
