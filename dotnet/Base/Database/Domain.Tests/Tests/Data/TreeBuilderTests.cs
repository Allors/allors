// <copyright file="TreeBuilderTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Database.Data;
    using Meta;
    using Xunit;

    public record OrganizationTreeBuilder : ITreeBuilder
    {
        public OrganizationOwnerNodeBuilder Owner { get; init; }

        public Node[] Build()
        {
            return new[] { this.Owner?.Build() }.Where(v => v != null).ToArray();
        }
    }

    public record PersonTreeBuilder : ITreeBuilder
    {
        public PersonFirstNameNodeBuilder FirstName { get; init; }

        public Node[] Build()
        {
            IEnumerable<Node> nodes = [
                this.FirstName?.Build()
            ];
            return nodes.Where(v => v != null).ToArray();
        }
    }

    public record OrganizationOwnerNodeBuilder(M M) : INodeBuilder
    {
        public PersonTreeBuilder Person { get; init; }

        public Node Build()
        {
            IEnumerable<Node>[] nodes = [
                this.Person?.Build()
            ];
            return new Node(this.M.Organization.Owner, nodes.Where(v => v != null).SelectMany(v => v));
        }
    }

    public record PersonFirstNameNodeBuilder(M M) : INodeBuilder
    {
        public Node Build()
        {
            return new Node(this.M.Person.FirstName);
        }
    }

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
