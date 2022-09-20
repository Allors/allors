// <copyright file="PluralAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property)]
public class PluralAttribute : RepositoryAttribute
{
    public PluralAttribute(string value)
    {
        this.Value = value;
    }

    public string Value { get; set; }
}
