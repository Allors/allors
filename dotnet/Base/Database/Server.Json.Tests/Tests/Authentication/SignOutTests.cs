// <copyright file="SignOutTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Server.Tests
{
    using System;
    using Allors.Database.Domain;
    using Allors.Protocol.Json.Auth;
    using Xunit;

    [Collection("Api")]
    public class SignOutTests : ApiTest
    {
        public SignOutTests()
        {
            this.Transaction.Build<Person>(v =>
            {
                v.UserName = "user";
                v.SetPassword("p@ssw0rd");
            });

            this.Transaction.Derive();
            this.Transaction.Commit();
        }

        [Fact]
        public async void Successful()
        {
            var args = new AuthenticationTokenRequest
            {
                l = "user",
                p = "p@ssw0rd",
            };

            var signInUri = new Uri("Authentication/Token", UriKind.Relative);
            await this.PostAsJsonAsync(signInUri, args);

            var signOutUri = new Uri("Authentication/SignOut", UriKind.Relative);
            await this.PostAsJsonAsync(signOutUri, null);
        }
    }
}
