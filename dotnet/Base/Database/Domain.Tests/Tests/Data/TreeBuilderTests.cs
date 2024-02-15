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
        M M { get; }

        void Add(INodeBuilder nodeBuilder);

        Node[] Build();
    }

    public class TreeBuilder(M m) : ITreeBuilder
    {
        private readonly List<INodeBuilder> nodeBuilders = [];

        public M M => m;

        public void Add(INodeBuilder nodeBuilder)
        {
            this.nodeBuilders.Add(nodeBuilder);
        }

        public Node[] Build()
        {
            return this.nodeBuilders.Select(v => v.Build()).ToArray();
        }
    }

    public static class OrganizationTreeBuilderExtensions
    {
        public static OrganizationOwnerNodeBuilder Owner(this OrganizationTreeBuilder @this, Action<OrganizationOwnerNodeBuilder> action = null)
        {
            var nodeBuilder = new OrganizationOwnerNodeBuilder(@this.M);
            @this.Add(nodeBuilder);
            action?.Invoke(nodeBuilder);
            return nodeBuilder;
        }
    }

    public static class PersonTreeBuilderExtensions
    {
        public static PersonTreeBuilder FirstName(this PersonTreeBuilder @this)
        {
            var nodeBuilder = new PersonFirstNameNodeBuilder(@this.M);
            @this.Add(nodeBuilder);
            return @this;
        }
    }

    public class OrganizationTreeBuilder : TreeBuilder
    {
        public OrganizationTreeBuilder(M m, Action<OrganizationTreeBuilder> init = null) : base(m)
        {
            init?.Invoke(this);
        }
    }

    public class PersonTreeBuilder : TreeBuilder
    {
        public PersonTreeBuilder(M m, Action<PersonTreeBuilder> init = null) : base(m)
        {
            init?.Invoke(this);
        }
    }

    public interface INodeBuilder
    {
        M M { get; }

        void SetTreeBuilder(ITreeBuilder treeBuilder);

        Node Build();
    }

    public class NodeBuilder(M m, IRelationEndType relationEndType) : INodeBuilder
    {
        private ITreeBuilder treeBuilder;

        public M M => m;

        public void SetTreeBuilder(ITreeBuilder treeBuilder) => this.treeBuilder = treeBuilder;

        public Node Build() => new(relationEndType, this.treeBuilder?.Build());
    }

    public static class OrganizationOwnerNodeBuilderExtensions
    {
        public static PersonTreeBuilder Person(this OrganizationOwnerNodeBuilder @this, Action<PersonTreeBuilder> action = null)
        {
            var treeBuilder = new PersonTreeBuilder(@this.M);
            @this.SetTreeBuilder(treeBuilder);

            if (action != null)
            {
                action?.Invoke(treeBuilder);
            }

            return treeBuilder;
        }
    }

    public class OrganizationOwnerNodeBuilder(M m) : NodeBuilder(m, m.Organization.Owner)
    {
    }

    public class PersonFirstNameNodeBuilder(M m) : NodeBuilder(m, m.Person.FirstName)
    {
    }

    public class TreeBuilderTests(Fixture fixture) : DomainTest(fixture), IClassFixture<Fixture>
    {
        [Fact]
        public void TestConstructor()
        {
            var builder = new OrganizationTreeBuilder(this.M,
                organization =>
                {
                    organization.Owner(owner =>
                    {
                        owner.Person(person =>
                        {
                            person.FirstName();
                        });
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

        [Fact]
        public void TestProperty()
        {
            var builder = new OrganizationTreeBuilder(this.M);
            var owner = builder.Owner();
            var person = owner.Person();
            person.FirstName();

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
