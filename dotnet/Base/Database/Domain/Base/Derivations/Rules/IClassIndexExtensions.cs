// <copyright file="ChangedRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Domain.Derivations.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using Allors.Database.Data;
    using Allors.Database.Meta;

    public static class IClassIndexExtensions
    {
        // AssociationType
        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationTypeIndex> associationType) where T : CompositeIndex => new(associationType(composite), composite);

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Composite ofType) where T : CompositeIndex => new(associationType(composite), ofType);

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Func<T, Node> node, Composite ofType = null) where T : CompositeIndex => new(associationType(composite), ofType) { Tree = Tree(composite, node) };

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Func<T, IEnumerable<Node>> nodes, Composite ofType = null) where T : CompositeIndex => new(associationType(composite), ofType) { Tree = Tree(composite, nodes) };

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Expression<Func<T, RelationEndTypeIndex>> path, Composite ofType = null) where T : CompositeIndex => new(associationType(composite), ofType) { Tree = Tree(composite, path) };

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Expression<Func<T, CompositeIndex>> path, Composite ofType = null) where T : CompositeIndex => new(associationType(composite), ofType) { Tree = Tree(composite, path) };
        
        // AssociationTypes
        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes) where T : CompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, composite));

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Composite ofType) where T : CompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType));

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Func<T, Node> node, Composite ofType = null) where T : CompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = [node(composite)] });

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Func<T, IEnumerable<Node>> nodes, Composite ofType = null) where T : CompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = nodes(composite).ToArray() });

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Expression<Func<T, RelationEndTypeIndex>> path, Composite ofType = null) where T : CompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] });

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Expression<Func<T, CompositeIndex>> path, Composite ofType = null) where T : CompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] });

        // RoleType
        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType) where T : CompositeIndex => new(roleType(composite), composite);

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Composite ofType) where T : CompositeIndex => new(roleType(composite), ofType);

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Func<T, Node> node, Composite ofType = null) where T : CompositeIndex => new(roleType(composite), ofType) { Tree = Tree(composite, node) };

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Func<T, IEnumerable<Node>> nodes, Composite ofType = null) where T : CompositeIndex => new(roleType(composite), ofType) { Tree = Tree(composite, nodes) };

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Expression<Func<T, RelationEndTypeIndex>> path, Composite ofType = null) where T : CompositeIndex => new(roleType(composite), ofType) { Tree = Tree(composite, path) };

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Expression<Func<T, CompositeIndex>> path, Composite ofType = null) where T : CompositeIndex => new(roleType(composite), ofType) { Tree = Tree(composite, path) };

        // RoleTypes
        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes) where T : CompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, composite));

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Composite ofType) where T : CompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType));

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Func<T, Node> node, Composite ofType = null) where T : CompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType) { Tree = Tree(composite, node) });

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Func<T, IEnumerable<Node>> nodes, Composite ofType = null) where T : CompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType) { Tree = Tree(composite, nodes) });

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Expression<Func<T, RelationEndTypeIndex>> path, Composite ofType = null) where T : CompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType) { Tree = Tree(composite, path) });

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Expression<Func<T, CompositeIndex>> path, Composite ofType = null) where T : CompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType) { Tree = Tree(composite, path) });

        private static IEnumerable<Node> Tree<T>(T composite, Func<T, Node> nodeFn) where T : CompositeIndex
        {
            var node = nodeFn(composite);
            if (node != null)
            {
                yield return node;
            }
        }

        private static IEnumerable<Node> Tree<T>(T composite, Func<T, IEnumerable<Node>> nodes) where T : CompositeIndex
        {
            return nodes(composite);
        }
        
        private static IEnumerable<Node> Tree<T>(T composite, Expression<Func<T, RelationEndTypeIndex>> path) where T : CompositeIndex
        {
            var node = path.Node(composite.Composite.MetaPopulation);
            if (node != null)
            {
                yield return node;
            }
        }

        private static IEnumerable<Node> Tree<T>(T composite, Expression<Func<T, CompositeIndex>> path) where T : CompositeIndex
        {
            var node = path.Node(composite.Composite.MetaPopulation);
            if (node != null)
            {
                yield return node;
            }
        }
    }
}
