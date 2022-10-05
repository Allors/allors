﻿// <copyright file="RequiredAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class UniqueAttribute : RepositoryAttribute, IExtensionAttribute
{
    public UniqueAttribute(bool value = true)
    {
        this.Value = value ? "true" : "false";
    }

    public bool ForRelationType => false;

    public bool ForAssociationType => false;

    public bool ForRoleType => false;

    public bool ForCompositeRoleType => true;

    public string Name => "AssignedIsUnique";

    public string Value { get; }
}
