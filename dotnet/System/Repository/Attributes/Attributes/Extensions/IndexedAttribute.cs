// <copyright file="IndexedAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public partial class IndexedAttribute : RepositoryAttribute, IExtensionAttribute
{
    public IndexedAttribute(bool value = true)
    {
        this.Value = value ? "true" : "false";
    }

    public bool ForRelationType => true;

    public bool ForAssociationType => false;

    public bool ForRoleType => false;

    public bool ForCompositeRoleType => false;

    public bool ForMethodType => false;

    public bool ForCompositeMethodType => false;

    public string Name => "IsIndexed";

    public string Value { get; }
}
