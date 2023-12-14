// <copyright file="DerivationCounted.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("B1B19B64-A507-435D-9D26-102AC7D97049")]

#endregion

public interface DerivationCounted : Object
{
    #region Allors

    [Id("4C66ED2E-6C08-4F9C-9A58-75F71AF9BAD1")]

    #endregion

    [Required]
    int DerivationCount { get; set; }
}
