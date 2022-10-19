// <copyright file="SizeAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public partial class SizeAttribute : RepositoryAttribute
{
    public SizeAttribute(int value)
    {
        this.Value = value;
    }

    public int Value { get; set; }
}
