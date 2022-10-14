// <copyright file="RemoteDerivationError.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Response;

    public class DerivationError : IDerivationError
    {
        private readonly ResponseDerivationError responseDerivationError;
        private readonly Workspace workspace;

        public DerivationError(Workspace workspace, ResponseDerivationError responseDerivationError)
        {
            this.workspace = workspace;
            this.responseDerivationError = responseDerivationError;
        }

        public string Message => this.responseDerivationError.m;

        public IEnumerable<Role> Roles =>
            from r in this.responseDerivationError.r
            let association = this.workspace.GetObject(r.i)
            let relationType = (RelationType)this.workspace.Connection.MetaPopulation.FindByTag(r.r)
            select new Role(association, relationType);
    }
}
