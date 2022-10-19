// <copyright file="DerivedAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public partial class DerivedAttribute : RepositoryAttribute
{
    public DerivedAttribute(bool value = true)
    {
        this.Value = value;
    }

    public bool Value { get; set; }
}
