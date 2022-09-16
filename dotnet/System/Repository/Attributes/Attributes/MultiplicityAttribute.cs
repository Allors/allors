// <copyright file="MultiplicityAttribute.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository.Attributes;

using System;

[AttributeUsage(AttributeTargets.Property)]
public class SingleAssociationAttribute : RepositoryAttribute
{
}
