// <copyright file="RequiredAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : RepositoryAttribute
{
    public RequiredAttribute(bool value = true) => this.Value = value;

    public bool Value { get; set; }
}
