// <copyright file="ExtendsAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Struct)]
public partial class ExtendsAttribute : RepositoryAttribute
{
    public ExtendsAttribute(string value)
    {
        this.Value = value;
    }

    public string Value { get; set; }
}
