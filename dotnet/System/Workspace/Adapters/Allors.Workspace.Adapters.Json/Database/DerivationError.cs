// <copyright file="RemoteDerivationError.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api;
    using Meta;

    public class DerivationError : IDerivationError
    {
        private readonly IWorkspace session;
        private readonly ResponseDerivationError responseDerivationError;

        public DerivationError(IWorkspace session, ResponseDerivationError responseDerivationError)
        {
            this.session = session;
            this.responseDerivationError = responseDerivationError;
        }

        public string Message => this.responseDerivationError.m;

        public IEnumerable<Role> Roles =>
            from r in this.responseDerivationError.r
            let association = this.session.Instantiate<IObject>(r.i)
            let relationType = (RelationType)this.session.WorkspaceConnection.Configuration.MetaPopulation.FindByTag(r.r)
            select new Role(association, relationType);
    }
}
