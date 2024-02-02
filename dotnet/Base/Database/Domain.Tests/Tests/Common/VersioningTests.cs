// <copyright file="VersioningTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using System.Linq;
    using Domain;
    using Xunit;

    public class VersioningTests : DomainTest, IClassFixture<Fixture>
    {
        public VersioningTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void InitialNothing()
        {
            var order = this.Transaction.Build<Order>();

            this.Transaction.Derive();

            Assert.True(order.ExistCurrentVersion);
            Assert.True(order.ExistAllVersions);
            Assert.Single(order.AllVersions);

            var version = order.CurrentVersion;

            Assert.Equal(order.Amount, version.Amount);
        }

        [Fact]
        public void VersionedUnitRole()
        {
            var order = this.Transaction.Build<Order>(v =>
                {
                    v.Amount = 10m;

                });

            this.Transaction.Derive();

            Assert.True(order.ExistCurrentVersion);
            Assert.True(order.ExistAllVersions);
            Assert.Single(order.AllVersions);

            var version = order.CurrentVersion;

            Assert.Equal(10m, version.Amount);
            Assert.False(version.ExistOrderState);
            Assert.False(version.ExistOrderLines);
        }

        [Fact]
        public void NonVersionedUnitRole()
        {
            var order = this.Transaction.Build<Order>(v =>
            {
                v.Amount = 10m;

            });
            this.Transaction.Derive();

            var currentVersion = order.CurrentVersion;

            order.NonVersionedAmount = 20m;

            this.Transaction.Derive();

            Assert.True(order.ExistAllVersions);
            Assert.Single(order.AllVersions);
            Assert.Equal(currentVersion, order.CurrentVersion);
        }

        [Fact]
        public void InitialCompositeRole()
        {
            var orderStates = this.Transaction.Scoped<OrderStateByKey>();

            var initialObjectState = orderStates.Initial;

            var order = this.Transaction.Build<Order>(v =>
            {
                v.OrderState = initialObjectState;
            });

            this.Transaction.Derive();

            Assert.True(order.ExistCurrentVersion);
            Assert.True(order.ExistAllVersions);
            Assert.Single(order.AllVersions);

            var version = order.CurrentVersion;

            Assert.False(version.ExistAmount);
            Assert.Equal(initialObjectState, version.OrderState);
            Assert.False(version.ExistOrderLines);
        }

        [Fact]
        public void InitialCompositeRoles()
        {
            var orderLine = this.Transaction.Build<OrderLine>();

            var order = this.Transaction.Build<Order>(v =>
            {
                v.AddOrderLine(orderLine);
            });

            this.Transaction.Derive();

            Assert.True(order.ExistCurrentVersion);
            Assert.True(order.ExistAllVersions);
            Assert.Single(order.AllVersions);

            var version = order.CurrentVersion;

            Assert.False(version.ExistAmount);
            Assert.False(version.ExistOrderState);
            Assert.Single(version.OrderLines);
            Assert.Equal(orderLine, version.OrderLines.ElementAt(0));
        }
    }
}
