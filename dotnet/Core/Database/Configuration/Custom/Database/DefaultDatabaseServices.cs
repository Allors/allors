// <copyright file="DefaultDatabaseScope.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors.Database.Configuration
{
    using Allors.Database.Derivations;
    using Allors.Database.Configuration.Derivations.Default;
    using Allors.Database.Domain;
    using Microsoft.AspNetCore.Http;

    public class DefaultDatabaseServices : DatabaseServices
    {
        public DefaultDatabaseServices(Engine engine, IHttpContextAccessor httpContextAccessor = null) : base(engine, httpContextAccessor) { }

        protected override IPasswordHasher CreatePasswordHasher() => new PasswordHasher();

        protected override IDerivationService CreateDerivationFactory() => new DerivationService(this.Engine);
    }
}
