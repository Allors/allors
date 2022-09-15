// <copyright file="Custom.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Attributes;

    [Domain]
    [Extends(nameof(Core))]
    [Id("ACAD1D11-277B-4CAB-8C5E-4AC915985C45")]
    public struct Base
    {
    }
}
