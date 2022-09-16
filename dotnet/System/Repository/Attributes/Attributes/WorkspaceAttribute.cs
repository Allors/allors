// <copyright file="WorkspaceAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
public class WorkspaceAttribute : RepositoryAttribute
{
    private static readonly string[] DefaultNames = {"Default"};

    public WorkspaceAttribute(params string[] names) => this.Names = names.Length > 0 ? names : DefaultNames;

    public string[] Names { get; }
}
