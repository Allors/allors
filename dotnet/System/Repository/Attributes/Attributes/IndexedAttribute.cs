// <copyright file="IndexedAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class IndexedAttribute : RepositoryAttribute
{
    public IndexedAttribute(bool value = true) => this.Value = value;

    public bool Value { get; set; }
}
