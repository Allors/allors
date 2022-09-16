// <copyright file="PrecisionAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class PrecisionAttribute : RepositoryAttribute
{
    public PrecisionAttribute(int value) => this.Value = value;

    public int Value { get; set; }
}
