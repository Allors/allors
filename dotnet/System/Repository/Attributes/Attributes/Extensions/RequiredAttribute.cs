// <copyright file="RequiredAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : RepositoryAttribute, IExtensionAttribute
{
    public RequiredAttribute(bool value = true)
    {
        this.Value = value ? "true" : "false";
    }

    public string Name => "Required";

    public string Value { get; }
}
