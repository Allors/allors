// <copyright file="ParametrizedTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using System.Collections.Generic;
    using Allors.Database.Data;
    using Xunit;

    public class ParametrizedTests : DomainTest, IClassFixture<Fixture>
    {
        public ParametrizedTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void EqualsWithArguments()
        {
            var filter = new Extent(this.M.Person)
            {
                Predicate = new Equals { RelationEndType = this.M.Person.FirstName, Parameter = "firstName" },
            };

            var arguments = new Arguments(new Dictionary<string, object> { { "firstName", "John" } });
            var queryExtent = filter.Build(this.Transaction, arguments);

            var extent = this.Transaction.Extent(this.M.Person);
            extent.Filter.AddEquals(this.M.Person.FirstName, "John");

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }
        
        [Fact]
        public void EqualsWithoutArguments()
        {
            var filter = new Extent(this.M.Person)
            {
                Predicate = new Equals { RelationEndType = this.M.Person.FirstName, Parameter = "firstName" },
            };

            var queryExtent = filter.Build(this.Transaction);

            var extent = this.Transaction.Extent(this.M.Person);

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }

        [Fact]
        public void AndWithArguments()
        {
            // select from Person where FirstName='John' and LastName='Doe'
            var filter = new Extent(this.M.Person)
            {
                Predicate = new And
                {
                    Operands =
                                [
                                    new Equals
                                        {
                                            RelationEndType = this.M.Person.FirstName,
                                            Parameter = "firstName",
                                        },
                                    new Equals
                                        {
                                            RelationEndType = this.M.Person.LastName,
                                            Parameter = "lastName",
                                        },
                                ],
                },
            };

            var arguments = new Arguments(new Dictionary<string, object>
                                {
                                    { "firstName", "John" },
                                    { "lastName", "Doe" },
                                });
            var queryExtent = filter.Build(this.Transaction, arguments);

            var extent = this.Transaction.Extent(this.M.Person);
            var and = extent.Filter.AddAnd();
            and.AddEquals(this.M.Person.FirstName, "John");
            and.AddEquals(this.M.Person.LastName, "Doe");

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }

        [Fact]
        public void AndWithoutArguments()
        {
            // select from Person where FirstName='John' and LastName='Doe'
            var filter = new Extent(this.M.Person)
            {
                Predicate = new And
                {
                    Operands =
                        [
                            new Equals
                                {
                                    RelationEndType = this.M.Person.FirstName,
                                    Parameter = "firstName",
                                },
                            new Equals
                                {
                                    RelationEndType = this.M.Person.LastName,
                                    Parameter = "lastName",
                                },
                        ],
                },
            };
            {
                var arguments = new Arguments(new Dictionary<string, object>
                                    {
                                        { "firstName", "John" },
                                    });
                var queryExtent = filter.Build(this.Transaction, arguments);

                var extent = this.Transaction.Extent(this.M.Person);
                extent.Filter.AddEquals(this.M.Person.FirstName, "John");

                Assert.Equal(extent.ToArray(), [.. queryExtent]);
            }

            {
                var queryExtent = filter.Build(this.Transaction);

                var extent = this.Transaction.Extent(this.M.Person);

                Assert.Equal(extent.ToArray(), [.. queryExtent]);
            }
        }

        [Fact]
        public void NestedWithArguments()
        {
            var filter = new Extent(this.M.C1)
            {
                Predicate = new ContainedIn
                {
                    RelationEndType = this.M.C1.C1C2One2One,
                    Extent = new Extent(this.M.C2)
                    {
                        Predicate = new Equals
                        {
                            RelationEndType = this.M.C2.C2AllorsString,
                            Parameter = "nested",
                        },
                    },
                },
            };

            var arguments = new Arguments(new Dictionary<string, object> { { "nested", "c2B" } });
            var queryExtent = filter.Build(this.Transaction, arguments);

            var c2s = this.Transaction.Extent(this.M.C2);
            c2s.Filter.AddEquals(this.M.C2.C2AllorsString, "c2B");
            var extent = this.Transaction.Extent(this.M.C1);
            extent.Filter.AddContainedIn(this.M.C1.C1C2One2One, c2s);

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }

        [Fact]
        public void NestedWithoutArguments()
        {
            var filter = new Extent(this.M.C1)
            {
                Predicate = new ContainedIn
                {
                    RelationEndType = this.M.C1.C1C2One2One,
                    Extent = new Extent(this.M.C2)
                    {
                        Predicate = new Equals
                        {
                            RelationEndType = this.M.C2.C2AllorsString,
                            Parameter = "nested",
                        },
                    },
                },
            };

            var arguments = new Arguments(new Dictionary<string, object>());
            var queryExtent = filter.Build(this.Transaction, arguments);

            var extent = this.Transaction.Extent(this.M.C1);

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }

        [Fact]
        public void AndNestedContainedInWithoutArguments()
        {
            var filter = new Extent(this.M.C1)
            {
                Predicate = new And
                {
                    Operands =
                    [
                        new ContainedIn
                        {
                            RelationEndType = this.M.C1.C1C2One2One,
                            Extent = new Extent(this.M.C2)
                            {
                                Predicate = new Equals
                                {
                                    RelationEndType = this.M.C2.C2AllorsString,
                                    Parameter = "nested",
                                },
                            },
                        },
                    ],
                },
            };

            var parameters = new Arguments(new Dictionary<string, object>());
            var queryExtent = filter.Build(this.Transaction, parameters);

            var extent = this.Transaction.Extent(this.M.C1);

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }

        [Fact]
        public void AndNestedContainsWithoutArguments()
        {
            var filter = new Extent(this.M.C1)
            {
                Predicate = new And
                {
                    Operands =
                    [
                        new ContainedIn
                            {
                                RelationEndType = this.M.C1.C1C2One2One,
                                Extent = new Extent(this.M.C2)
                                             {
                                                 Predicate = new Contains
                                                                 {
                                                                     RelationEndType = this.M.C2.C1WhereC1C2One2One,
                                                                     Parameter = "nested",
                                                                 },
                                             },
                            },
                    ],
                },
            };

            var arguments = new Arguments(new Dictionary<string, object>());
            var queryExtent = filter.Build(this.Transaction, arguments);

            var extent = this.Transaction.Extent(this.M.C1);

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }
    }
}
