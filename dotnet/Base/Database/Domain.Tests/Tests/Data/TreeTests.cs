// <copyright file="FilterTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Data.Tests
{
    using System.Linq;
    using Allors.Database.Domain.Tests;
    using Domain;
    using Meta;
    using Xunit;

    public class TreeTests : DomainTest, IClassFixture<Fixture>
    {
        public TreeTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void RoleType()
        {
            var m = this.M;

            var tree = new UserGroupMembersNodeBuilder(m)
                .Build();

            var node = tree.First();

            Assert.Equal(m.UserGroup.Members, node.RelationEndType);
            Assert.Empty(node.Nodes);
        }

        [Fact]
        public void RoleTypeChild()
        {
            var m = this.M;

            var tree = new UserGroupMembersNodeBuilder(m)
            {
                User = new UserTreeBuilder
                {
                    UniqueId = new UniquelyIdentifiableUniqueIdNodeBuilder(m)
                }
            }
            .Build();

            var node = tree.First();

            Assert.Equal(m.UserGroup.Members, node.RelationEndType);
            Assert.Single(node.Nodes);

            var child = node.Nodes.First();

            Assert.Equal(m.User.UniqueId, child.RelationEndType);
            Assert.Empty(child.Nodes);
        }

        [Fact]
        public void ClassRoleTypeClassRoleType()
        {
            var builder = new OrganizationTreeBuilder
            {
                Owner = new(this.M)
                {
                    Person = new()
                    {
                        FirstName = new(this.M)
                    }
                }
            };

            var tree = builder.Build();

            Assert.Single(tree);

            var node = tree[0];

            Assert.Equal(this.M.Organization.Owner, node.RelationEndType);
            Assert.Single(node.Nodes);

            var childNode = node.Nodes[0];

            Assert.Equal(this.M.Person.FirstName, childNode.RelationEndType);
        }

        [Fact]
        public void ClassRoleTypeClassRoleTypes()
        {
            var m = this.M;

            var tree = new UserGroupTreeBuilder
            {
                Members = new UserGroupMembersNodeBuilder(m)
                {
                    User = new UserTreeBuilder
                    {
                        UniqueId = new UniquelyIdentifiableUniqueIdNodeBuilder(m),
                        SecurityTokens = new ObjectSecurityTokensNodeBuilder(m)
                    }
                }
            }.Build();

            var node = tree.First();

            Assert.Equal(m.UserGroup.Members, node.RelationEndType);
            Assert.Equal(2, node.Nodes.Length);

            var uniqueIdChild = node.Nodes.First(v => v.RelationEndType.Equals(m.User.UniqueId));

            Assert.NotNull(uniqueIdChild);
            Assert.Empty(uniqueIdChild.Nodes);

            var securityTokens = node.Nodes.First(v => v.RelationEndType.Equals(m.User.SecurityTokens));

            Assert.NotNull(securityTokens);
            Assert.Empty(securityTokens.Nodes);

        }
    }
}
