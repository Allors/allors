// <copyright file="ScaleAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public partial class ScaleAttribute : RepositoryAttribute
{
    public ScaleAttribute(int value)
    {
        this.Value = value;
    }

    public int Value { get; set; }
}
