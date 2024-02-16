// <copyright file="IRelationEndTypeExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Workspace.Data
{
    using Allors.Workspace.Meta;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class IRelationEndTypeExtensions
    {
        public static Node Node<T>(this T @this) where T : IRelationEndType => new Node(@this);

        public static Node Node<T>(this T @this, Func<T, Node> child) where T : IRelationEndType => new Node(@this, [child(@this)]);

        public static Node Node<T>(this T @this, params Func<T, Node>[] children) where T : IRelationEndType => new Node(@this, children.Select(v => v(@this)));

        public static Node Node<T>(this T @this, Func<T, IEnumerable<Node>> children) where T : IRelationEndType => new Node(@this, children(@this));

        public static object Get<T>(this T @this, IStrategy strategy, IComposite ofType = null) where T : IRelationEndType
        {
            if (@this is IRoleType roleType)
            {
                if (roleType.IsOne)
                {
                    var association = strategy.CompositeRole(roleType)?.Value;

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return !ofType.IsAssignableFrom(association.Class) ? null : association;
                }
                else
                {
                    var association = strategy.CompositesRole(roleType).Value;

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return association.Where(v => ofType.IsAssignableFrom(v.Class));
                }
            }

            if (@this is IAssociationType associationType)
            {
                if (associationType.IsOne)
                {
                    var association = strategy.CompositeAssociation(associationType).Value;

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return !ofType.IsAssignableFrom(association.Class) ? null : association;
                }
                else
                {
                    var association = strategy.CompositesAssociation(associationType).Value;

                    if (ofType == null || association == null)
                    {
                        return association;
                    }

                    return association.Where(v => ofType.IsAssignableFrom(v.Class));
                }
            }

            throw new ArgumentException("Get only supports RoleType or AssociationType");
        }
    }
}
