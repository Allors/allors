// <copyright file="IPropertyTypeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;

public static class IPropertyTypeExtensions
{
    public static Node Node<T>(this T @this) where T : IPropertyType => new(@this);

    public static Node Node<T>(this T @this, Func<T, Node> child) where T : IPropertyType => new(@this, new[] { child(@this) });

    public static Node Node<T>(this T @this, params Func<T, Node>[] children) where T : IPropertyType =>
        new(@this, children.Select(v => v(@this)));

    public static Node Node<T>(this T @this, Func<T, IEnumerable<Node>> children) where T : IPropertyType => new(@this, children(@this));
}
