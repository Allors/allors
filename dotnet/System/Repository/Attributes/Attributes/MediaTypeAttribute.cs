// <copyright file="MediaTypeAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class MediaTypeAttribute : RepositoryAttribute
{
    public MediaTypeAttribute(string value)
    {
        this.Value = value;
    }

    public string Value { get; set; }
}
