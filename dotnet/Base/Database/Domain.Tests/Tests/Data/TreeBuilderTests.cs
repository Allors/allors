// <copyright file="FilterTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using Xunit;

    public class TreeBuilderTests : DomainTest, IClassFixture<Fixture>
    {
        public TreeBuilderTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void Test()
        {
            var nodeOrganisation = new NodeOrganization
            {
                Owner = new NodePerson
                {
                    FirstName = new NodeString()
                }
            };

            var m = this.M;

            var nodeBuilder = new TreeBuilder(m);

            var tree = nodeBuilder.Build(nodeOrganisation);

            Assert.Single(tree);

            var node = tree[0];

            Assert.Equal(m.Organization.Owner, node.RelationEndType);
            Assert.Single(node.Nodes);

            var childNode = node.Nodes[0];

            Assert.Equal(m.Person.FirstName, childNode.RelationEndType);
        }
    }
}
