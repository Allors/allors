// <copyright file="Singletons.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class SecurityTokens
    {
        protected override void CorePrepare(Setup setup) => setup.AddDependency(this.ObjectType, this.M.Grant);

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Transaction.Caches().SecurityTokenByUniqueId().Merger().Action();

            var grants = new Grants(this.Transaction);

            merge(SecurityToken.InitialSecurityTokenId, v =>
              {
                  if (setup.Config.SetupSecurity)
                  {
                      v.AddGrant(grants.Creators);
                      v.AddGrant(grants.GuestCreator);
                      v.AddGrant(grants.Administrator);
                  }
              });

            merge(SecurityToken.DefaultSecurityTokenId, v =>
              {
                  if (setup.Config.SetupSecurity)
                  {
                      v.AddGrant(grants.Administrator);
                      v.AddGrant(grants.Guest);
                  }
              });

            merge(SecurityToken.AdministratorSecurityTokenId, v =>
              {
                  if (setup.Config.SetupSecurity)
                  {
                      v.AddGrant(grants.Administrator);
                  }
              });
        }
    }
}
