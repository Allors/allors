// <copyright file="UniquelyIdentifiable.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Attributes;
using static Workspaces;

#region Allors
[Id("122ccfe1-f902-44c1-9d6c-6f6a0afa9469")]
#endregion
public interface UniquelyIdentifiable : Object
{
    #region Allors
    [Id("e1842d87-8157-40e7-b06e-4375f311f2c3")]
    #endregion
    [Workspace(Default)]
    [Indexed]
    [Required]
    Guid UniqueId { get; set; }
}
