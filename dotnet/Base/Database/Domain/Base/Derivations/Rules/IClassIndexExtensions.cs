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
    using Allors.Database.Data;
    using Allors.Database.Meta;

    public static class IClassIndexExtensions
    {
        // AssociationType
        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, IAssociationTypeIndex> associationType) where T : ICompositeIndex => new AssociationPattern(associationType(composite), composite);

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Composite ofType) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType);

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Func<T, Node> node, Composite ofType = null) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType) { Tree = [node(composite)] };

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Func<T, IEnumerable<Node>> nodes, Composite ofType = null) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType) { Tree = nodes(composite).ToArray() };

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Expression<Func<T, IRelationEndTypeIndex>> path, Composite ofType = null) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] };

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Expression<Func<T, ICompositeIndex>> path, Composite ofType = null) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] };

        // AssociationTypes
        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, composite));

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Composite ofType) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType));

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Func<T, Node> node, Composite ofType = null) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = [node(composite)] });

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Func<T, IEnumerable<Node>> nodes, Composite ofType = null) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = nodes(composite).ToArray() });

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Expression<Func<T, IRelationEndTypeIndex>> path, Composite ofType = null) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] });

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Expression<Func<T, ICompositeIndex>> path, Composite ofType = null) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] });

        // RoleType
        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType) where T : ICompositeIndex => new RolePattern(roleType(composite), composite);

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Composite ofType) where T : ICompositeIndex => new RolePattern(roleType(composite), ofType);

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Func<T, Node> node, Composite ofType = null) where T : ICompositeIndex => new RolePattern(roleType(composite), ofType) { Tree = [node(composite)] };

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Func<T, IEnumerable<Node>> nodes, Composite ofType = null) where T : ICompositeIndex => new RolePattern(roleType(composite), ofType) { Tree = nodes(composite).ToArray() };

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Expression<Func<T, IRelationEndTypeIndex>> path, Composite ofType = null) where T : ICompositeIndex => new RolePattern(roleType(composite), ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] };

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, Expression<Func<T, ICompositeIndex>> path, Composite ofType = null) where T : ICompositeIndex => new RolePattern(roleType(composite), ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] };

        // RoleTypes
        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes) where T : ICompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, composite));

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Composite ofType) where T : ICompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType));

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Func<T, Node> node, Composite ofType = null) where T : ICompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType) { Tree = [node(composite)] });

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Func<T, IEnumerable<Node>> nodes, Composite ofType = null) where T : ICompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType) { Tree = nodes(composite).ToArray() });

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Expression<Func<T, IRelationEndTypeIndex>> path, Composite ofType = null) where T : ICompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] });

        public static IEnumerable<RolePattern> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Expression<Func<T, ICompositeIndex>> path, Composite ofType = null) where T : ICompositeIndex => roleTypes(composite).Select(v => new RolePattern(v, ofType) { Tree = [path.Node(composite.Composite.MetaPopulation)] });
    }
}
