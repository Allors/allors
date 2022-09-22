// <copyright file="MediaTypeAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class MediaTypeAttribute : RepositoryAttribute, IExtensionAttribute
{
    public MediaTypeAttribute(string value)
    {
        this.Value = @$"""{value}""";
    }

    public string Name => "MediaType";

    public string Value { get; }
}
