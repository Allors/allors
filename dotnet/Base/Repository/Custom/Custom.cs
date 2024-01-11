﻿// <copyright file="Custom.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository;

using Allors.Repository.Attributes;

[Id("af96e2b7-3bb5-4cd1-b02c-39a67c99a11a")]
[Domain]
[Extends(nameof(Base))]
public struct Custom
{
}
