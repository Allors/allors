// <copyright file="IdAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Struct | AttributeTargets.Class |
                AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Method)]
public partial class IdAttribute : RepositoryAttribute
{
    public IdAttribute(string value)
    {
        this.Value = value;
    }

    public string Value { get; set; }
}
