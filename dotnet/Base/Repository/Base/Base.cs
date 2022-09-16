// <copyright file="Base.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Attributes;

[Domain]
[Extends(nameof(Core))]
[Id("ACAD1D11-277B-4CAB-8C5E-4AC915985C45")]
public struct Base
{
}
