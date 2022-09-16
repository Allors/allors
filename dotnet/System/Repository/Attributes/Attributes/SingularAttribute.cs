// <copyright file="SingularAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class SingularAttribute : RepositoryAttribute
{
    public SingularAttribute(string value) => this.Value = value;

    public string Value { get; set; }
}
