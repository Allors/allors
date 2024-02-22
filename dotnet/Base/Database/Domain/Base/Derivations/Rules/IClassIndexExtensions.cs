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

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, IComposite ofType) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType);

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Func<T, Node> node, IComposite ofType = null) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType) { Tree = [node(composite)] };

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Func<T, IEnumerable<Node>> nodes, IComposite ofType = null) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType) { Tree = nodes(composite).ToArray() };

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Expression<Func<T, RelationEndType>> path, IComposite ofType = null) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType) { Tree = [path.Node(composite.MetaPopulation)] };

        public static AssociationPattern AssociationPattern<T>(this T composite, Func<T, AssociationType> associationType, Expression<Func<T, IComposite>> path, IComposite ofType = null) where T : ICompositeIndex => new AssociationPattern(associationType(composite), ofType) { Tree = [path.Node(composite.MetaPopulation)] };

        // AssociationTypes
        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, composite));

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, IComposite ofType) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType));

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Func<T, Node> node, IComposite ofType = null) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = [node(composite)] });

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Func<T, IEnumerable<Node>> nodes, IComposite ofType = null) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = nodes(composite).ToArray() });

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Expression<Func<T, RelationEndType>> path, IComposite ofType = null) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = [path.Node(composite.MetaPopulation)] });

        public static IEnumerable<AssociationPattern> AssociationPattern<T>(this T composite, Func<T, IEnumerable<AssociationType>> associationTypes, Expression<Func<T, IComposite>> path, IComposite ofType = null) where T : ICompositeIndex => associationTypes(composite).Select(v => new AssociationPattern(v, ofType) { Tree = [path.Node(composite.MetaPopulation)] });

        // RoleType
        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType) where T : IComposite => new RolePattern(roleType(composite), composite);

        public static RolePattern RolePattern<T>(this T composite, Func<T, RoleType> roleType, IComposite ofType) where T : IComposite => new RolePattern<T>(composite, roleType) { OfType = ofType };

        public static RolePattern<T> RolePattern<T>(this T composite, Func<T, RoleType> roleType, Func<T, Node> node, IComposite ofType = null) where T : IComposite => new RolePattern<T>(composite, roleType, node) { OfType = ofType };

        public static RolePattern<T> RolePattern<T>(this T composite, Func<T, RoleType> roleType, Func<T, IEnumerable<Node>> nodes, IComposite ofType = null) where T : IComposite => new RolePattern<T>(composite, roleType, nodes) { OfType = ofType };

        public static RolePattern<T> RolePattern<T>(this T composite, Func<T, RoleType> roleType, Expression<Func<T, RelationEndType>> path, IComposite ofType = null) where T : IComposite => new RolePattern<T>(composite, roleType, path) { OfType = ofType };

        public static RolePattern<T> RolePattern<T>(this T composite, Func<T, RoleType> roleType, Expression<Func<T, IComposite>> path, IComposite ofType = null) where T : IComposite => new RolePattern<T>(composite, roleType, path) { OfType = ofType };

        // RoleTypes
        public static IEnumerable<RolePattern<T>> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes) where T : IComposite => roleTypes(composite).Select(v => new RolePattern<T>(composite, v));

        public static IEnumerable<RolePattern<T>> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, IComposite ofType) where T : IComposite => roleTypes(composite).Select(v => new RolePattern<T>(composite, v) { OfType = ofType });

        public static IEnumerable<RolePattern<T>> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Func<T, Node> node, IComposite ofType = null) where T : IComposite => roleTypes(composite).Select(v => new RolePattern<T>(composite, v, node) { OfType = ofType });

        public static IEnumerable<RolePattern<T>> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Func<T, IEnumerable<Node>> nodes, IComposite ofType = null) where T : IComposite => roleTypes(composite).Select(v => new RolePattern<T>(composite, v, nodes) { OfType = ofType });

        public static IEnumerable<RolePattern<T>> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Expression<Func<T, RelationEndType>> path, IComposite ofType = null) where T : IComposite => roleTypes(composite).Select(v => new RolePattern<T>(composite, v, path) { OfType = ofType });

        public static IEnumerable<RolePattern<T>> RolePattern<T>(this T composite, Func<T, IEnumerable<RoleType>> roleTypes, Expression<Func<T, IComposite>> path, IComposite ofType = null) where T : IComposite => roleTypes(composite).Select(v => new RolePattern<T>(composite, v, path) { OfType = ofType });
    }
}
