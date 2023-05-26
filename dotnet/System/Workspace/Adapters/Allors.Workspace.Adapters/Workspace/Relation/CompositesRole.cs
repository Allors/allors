// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Adapters;
    using Meta;

    public class CompositesRole : ICompositesRole
    {
        public IRelationType RelationType => this.RoleType.RelationType;

        public IRoleType RoleType { get; }

        public IStrategy Object { get; }


        public CompositesRole(Strategy strategy, IRoleType roleType)
        {
            this.Object = strategy;
            this.RoleType = roleType;
        }
    }
}
