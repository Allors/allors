// <copyright file="PrefetchPolicyBuilderExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>


namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using Allors.Database.Data;
    using Allors.Database.Meta;

    public static partial class PrefetchPolicyBuilderExtensions
    {
        public static PrefetchPolicyBuilder WithSecurityRules(this PrefetchPolicyBuilder @this, MetaIndex m)
        {
            // Object
            @this.WithRule(m.Object.SecurityTokens);
            @this.WithRule(m.Object.Revocations);

            // TODO: Use caching
            // Shared Security
            var delegatedAccessPolicy = new PrefetchPolicyBuilder()
                .WithRule(m.DelegatedAccess.DelegatedSecurityTokens)
                .WithRule(m.DelegatedAccess.DelegatedRevocations)
                .Build();

            @this.WithRule(m.Object.AccessDelegation, delegatedAccessPolicy);

            return @this;
        }

        public static PrefetchPolicyBuilder WithWorkspaceRules(this PrefetchPolicyBuilder @this, MetaIndex m, IEnumerable<RoleType> roleTypes)
        {
            @this.WithSecurityRules(m);

            foreach (var roleType in roleTypes)
            {
                if (roleType.ObjectType.IsComposite)
                {
                    var securityPrefetch = new PrefetchPolicyBuilder().WithSecurityRules(m).Build();
                    @this.WithRule(roleType, securityPrefetch);
                }
                else
                {
                    @this.WithRule(roleType);
                }
            }

            return @this;
        }

        public static PrefetchPolicyBuilder WithNodes(this PrefetchPolicyBuilder @this, Node[] treeNodes, MetaIndex m)
        {
            foreach (var node in treeNodes)
            {
                @this.WithNode(node, m);
            }

            return @this;
        }

        private static PrefetchPolicyBuilder WithNode(this PrefetchPolicyBuilder @this, Node treeNode, MetaIndex m)
        {
            if (treeNode.Nodes == null || treeNode.Nodes.Length == 0)
            {
                @this.WithRule(treeNode.RelationEndType);
            }
            else
            {
                var nestedPrefetchPolicyBuilder = new PrefetchPolicyBuilder();
                foreach (var node in treeNode.Nodes)
                {
                    @this.WithNode(node, m);
                }

                var nestedPrefetchPolicy = nestedPrefetchPolicyBuilder.Build();
                @this.WithRule(treeNode.RelationEndType, nestedPrefetchPolicy);
            }

            if (treeNode.RelationEndType.ObjectType is Composite)
            {
                @this.WithSecurityRules(m);
            }

            return @this;
        }
    }
}
