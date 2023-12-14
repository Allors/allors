// <copyright file="PushTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System;
    using System.Threading;
    using Allors.Database.Domain;
    using Allors.Database.Protocol.Json;
    using Allors.Protocol.Json.Api.Push;
    using Xunit;

    public class PushDeletedObjectsTests : ApiTest, IClassFixture<Fixture>
    {
        public PushDeletedObjectsTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void SameWorkspace()
        {
            this.SetUser("jane@example.com");

            var organization = this.Transaction.Build<Organization>();
            this.Transaction.Commit();

            var organizationId = organization.Id;
            var organizationVersion = organization.Strategy.ObjectVersion;

            organization.Delete();
            this.Transaction.Commit();

            var uri = new Uri(@"allors/push", UriKind.Relative);

            var pushRequest = new PushRequest
            {
                o = new[]
                {
                    new PushRequestObject
                    {
                        d = organizationId,
                        v = organizationVersion,
                        r = new[]
                        {
                            new PushRequestRole
                            {
                              t = this.M.Organization.Name.RelationType.Tag,
                              u = "Acme",
                            },
                        },
                    },
                },
            };

            var api = new Api(this.Transaction, "Default", CancellationToken.None);
            var pushResponse = api.Push(pushRequest);

            Assert.True(pushResponse.HasErrors);
            Assert.Single(pushResponse._m);
            Assert.Contains(organizationId, pushResponse._m);
        }
    }
}
