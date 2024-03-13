// <copyright file="MediaTypeAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public partial class MediaTypeAttribute : RepositoryAttribute, IExtensionAttribute
{
    public MediaTypeAttribute(string value)
    {
        this.Value = @$"""{value}""";
    }

    public bool ForRelationType => false;

    public bool ForAssociationType => false;

    public bool ForRoleType => true;

    public bool ForCompositeRoleType => false;

    public bool ForMethodType => false;

    public bool ForCompositeMethodType => false;

    public string Name => "MediaType";

    public string Value { get; }
}
