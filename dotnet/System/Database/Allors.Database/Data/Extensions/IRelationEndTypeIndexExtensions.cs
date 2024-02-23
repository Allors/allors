// <copyright file="IRelationEndTypeExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;

public static class IRelationEndTypeIndexExtensions
{
    public static Node Node<T>(this T @this) where T : RelationEndTypeIndex => new(@this);

    public static Node Node<T>(this T @this, Func<T, Node> child) where T : RelationEndTypeIndex => new(@this, [child(@this)]);

    public static Node Node<T>(this T @this, params Func<T, Node>[] children) where T : RelationEndTypeIndex => new(@this, children.Select(v => v(@this)));

    public static Node Node<T>(this T @this, Func<T, IEnumerable<Node>> children) where T : RelationEndTypeIndex => new(@this, children(@this));
}
