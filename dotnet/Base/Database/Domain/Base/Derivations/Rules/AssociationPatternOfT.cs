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

    public class AssociationPattern<T> : AssociationPattern where T : IComposite
    {
        public AssociationPattern(T objectType, IAssociationType associationType) : base(associationType, objectType)
        {
        }

        public AssociationPattern(T objectType, IAssociationType associationType, Func<T, Node> node) : base(associationType, objectType) => this.Tree = [node(objectType)];

        public AssociationPattern(T objectType, IAssociationType associationType, Func<T, IEnumerable<Node>> tree) : base(associationType, objectType) => this.Tree = tree(objectType).ToArray();

        public AssociationPattern(T objectType, IAssociationType associationType, Expression<Func<T, IComposite>> path) : base(associationType, objectType) => this.Tree = [path.Node(objectType.MetaPopulation)];

        public AssociationPattern(T objectType, IAssociationType associationType, Expression<Func<T, IRelationEndType>> path) : base(associationType, objectType) => this.Tree = [path.Node(objectType.MetaPopulation)];

        public AssociationPattern(T objectType, Func<T, IAssociationType> associationType) : base(associationType(objectType), objectType)
        {
        }

        public AssociationPattern(T objectType, Func<T, IAssociationType> associationType, Func<T, Node> node) : base(associationType(objectType), objectType) => this.Tree = [node(objectType)];

        public AssociationPattern(T objectType, Func<T, IAssociationType> associationType, Func<T, IEnumerable<Node>> tree) : base(associationType(objectType), objectType) => this.Tree = tree(objectType).ToArray();

        public AssociationPattern(T objectType, Func<T, IAssociationType> associationType, Expression<Func<T, IComposite>> path) : base(associationType(objectType), objectType) => this.Tree = [path.Node(objectType.MetaPopulation)];

        public AssociationPattern(T objectType, Func<T, IAssociationType> associationType, Expression<Func<T, IRelationEndType>> path) : base(associationType(objectType), objectType) => this.Tree = [path.Node(objectType.MetaPopulation)];
    }
}
