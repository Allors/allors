// <copyright file="IPropertyTypeExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Workspace.Request.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Response;

    public static class IPropertyTypeExtensions
    {
        public static Node Node<T>(this T @this) where T : IPropertyType => new Node(@this);

        public static Node Node<T>(this T @this, Func<T, Node> child) where T : IPropertyType => new Node(@this, new[] { child(@this) });

        public static Node Node<T>(this T @this, params Func<T, Node>[] children) where T : IPropertyType =>
            new Node(@this, children.Select(v => v(@this)));

        public static Node Node<T>(this T @this, Func<T, IEnumerable<Node>> children) where T : IPropertyType =>
            new Node(@this, children(@this));

        public static object Get<T>(this T @this, IObject strategy, IComposite ofType = null) where T : IPropertyType
        {
            if (@this is RoleType roleType)
            {
                if (roleType.IsOne)
                {
                    var association = strategy.GetCompositeRole(roleType);

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return !ofType.IsAssignableFrom(association.Class) ? null : association;
                }
                else
                {
                    var association = strategy.GetCompositesRole(roleType);

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return association.Where(v => ofType.IsAssignableFrom(v.Class));
                }
            }

            throw new ArgumentException("Get only supports RoleType");
        }
    }
}
