// <copyright file="FilterTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using Allors.Database.Data;
    using Xunit;

    public class FilterTests : DomainTest, IClassFixture<Fixture>
    {
        public FilterTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void Type()
        {
            var query = new Extent(this.M.Person.Composite);
            var queryExtent = query.Build(this.Transaction);

            var extent = this.Transaction.Extent(this.M.Person.Composite);

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }

        [Fact]
        public void RoleEquals()
        {
            var filter = new Extent(this.M.Person.Composite)
            {
                Predicate = new Equals
                {
                    RelationEndType = this.M.Person.FirstName,
                    Value = "John",
                },
            };

            var queryExtent = filter.Build(this.Transaction);

            var extent = this.Transaction.Extent(this.M.Person.Composite);
            extent.Filter().AddEquals(this.M.Person.FirstName, "John");

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }

        [Fact]
        public void And()
        {
            // select from Person where FirstName='John' and LastName='Doe'
            var filter = new Extent(this.M.Person.Composite)
            {
                Predicate = new And
                {
                    Operands =
                    [
                        new Equals
                            {
                                RelationEndType = this.M.Person.FirstName,
                                Value = "John",
                            },
                        new Equals
                            {
                                RelationEndType = this.M.Person.LastName,
                                Value = "Doe",
                            },
                    ],
                },
            };

            var queryExtent = filter.Build(this.Transaction);

            var extent = this.Transaction.Extent(this.M.Person.Composite);
            extent.Filter().AddAnd(v =>
            {
                v.AddEquals(this.M.Person.FirstName, "John");
                v.AddEquals(this.M.Person.LastName, "Doe");
            });

            Assert.Equal(extent.ToArray(), [.. queryExtent]);
        }
    }
}
