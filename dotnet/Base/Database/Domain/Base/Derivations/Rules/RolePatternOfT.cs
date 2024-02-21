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

    public class RolePattern<T> : RolePattern where T : IComposite
    {
        public RolePattern(T objectType, RoleType roleType) : base(roleType, objectType) { }

        public RolePattern(T objectType, RoleType roleType, Func<T, Node> path) : base(roleType, objectType) => this.Tree = [path(objectType)];

        public RolePattern(T objectType, RoleType roleType, Func<T, IEnumerable<Node>> path) : base(roleType, objectType) => this.Tree = path(objectType).ToArray();

        public RolePattern(T objectType, RoleType roleType, Expression<Func<T, RelationEndType>> step) : base(roleType, objectType) => this.Tree = [step?.Node(objectType.MetaPopulation)];

        public RolePattern(T objectType, RoleType roleType, Expression<Func<T, IComposite>> step) : base(roleType, objectType) => this.Tree = [step?.Node(objectType.MetaPopulation)];

        public RolePattern(T objectType, Func<T, RoleType> role) : base(role(objectType), objectType) { }

        public RolePattern(T objectType, Func<T, RoleType> role, Func<T, Node> path) : base(role(objectType), objectType) => this.Tree = [path(objectType)];

        public RolePattern(T objectType, Func<T, RoleType> role, Func<T, IEnumerable<Node>> path) : base(role(objectType), objectType) => this.Tree = path(objectType).ToArray();

        public RolePattern(T objectType, Func<T, RoleType> role, Expression<Func<T, RelationEndType>> step) : base(role(objectType), objectType) => this.Tree = [step?.Node(objectType.MetaPopulation)];

        public RolePattern(T objectType, Func<T, RoleType> role, Expression<Func<T, IComposite>> step) : base(role(objectType), objectType) => this.Tree = [step?.Node(objectType.MetaPopulation)];
    }
}
