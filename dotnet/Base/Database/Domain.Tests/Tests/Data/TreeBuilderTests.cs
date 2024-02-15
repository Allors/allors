// <copyright file="TreeBuilderTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using Xunit;
   
    public class TreeBuilderTests(Fixture fixture) : DomainTest(fixture), IClassFixture<Fixture>
    {
        [Fact]
        public void Test()
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
    }
}
