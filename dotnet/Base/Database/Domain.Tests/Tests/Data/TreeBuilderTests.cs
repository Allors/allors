// <copyright file="FilterTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database.Data;
    using Meta;
    using Xunit;

    public interface ITreeBuilder
    {
        Node[] Build();
    }

    public interface INodeBuilder
    {
        Node Build();
    }

    public class OrganizationTreeBuilder(M m) : ITreeBuilder
    {
        private readonly List<INodeBuilder> nodeBuilders = [];

        public OrganizationTreeBuilder Owner(Action<OrganizationOwnerNodeBuilder> action = null)
        {
            var nodeBuilder = new OrganizationOwnerNodeBuilder(m);
            this.nodeBuilders.Add(nodeBuilder);
            action?.Invoke(nodeBuilder);
            return this;
        }

        public Node[] Build()
        {
            return this.nodeBuilders.Select(v => v.Build()).ToArray();
        }
    }

    public class PersonTreeBuilder(M m) : ITreeBuilder
    {
        private readonly List<INodeBuilder> nodeBuilders = [];

        public PersonTreeBuilder FirstName()
        {
            var nodeBuilder = new PersonFirstNameNodeBuilder(m);
            this.nodeBuilders.Add(nodeBuilder);
            return this;
        }

        public Node[] Build() => this.nodeBuilders.Select(v => v.Build()).ToArray();
    }

    public class OrganizationOwnerNodeBuilder(M m) : INodeBuilder
    {
        private PersonTreeBuilder treeBuilder;

        public OrganizationOwnerNodeBuilder Person(Action<PersonTreeBuilder> action = null)
        {
            if (action != null)
            {
                this.treeBuilder = new PersonTreeBuilder(m);
                action?.Invoke(this.treeBuilder);
            }

            return this;
        }

        public Node Build() => new(m.Organization.Owner, this.treeBuilder.Build());
    }

    public class PersonFirstNameNodeBuilder(M m) : INodeBuilder
    {
        public Node Build() => new(m.Person.FirstName);
    }

    public class TreeBuilderTests : DomainTest, IClassFixture<Fixture>
    {
        public TreeBuilderTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void Test()
        {
            var builder = new OrganizationTreeBuilder(this.M)
                .Owner(organization =>
                {
                    organization.Person(person =>
                    {
                        person.FirstName();
                    });
                });

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
