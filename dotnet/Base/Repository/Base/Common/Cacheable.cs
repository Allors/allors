// <copyright file="Cacheable.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Attributes;

#region Allors
[Id("B17AFC19-9E91-4631-B6D8-43B32A65E0A0")]
#endregion
public interface Cacheable : Object
{
    #region Allors
    [Id("EF6F1F4C-5B62-49DC-9D05-0F02973ACCB3")]
    #endregion
    [Derived]
    Guid CacheId { get; set; }
}
