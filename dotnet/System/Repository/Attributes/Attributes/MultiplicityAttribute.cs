// <copyright file="MultiplicityAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class MultiplicityAttribute : RepositoryAttribute
{
    public MultiplicityAttribute(Multiplicity value) => this.Value = value;

    public Multiplicity Value { get; set; }
}
