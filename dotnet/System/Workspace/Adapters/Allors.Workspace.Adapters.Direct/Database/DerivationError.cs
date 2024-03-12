// <copyright file="RemoteDerivationError.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public class DerivationError : IDerivationError
    {
        private readonly Database.Derivations.IDerivationError derivationError;
        private readonly Workspace workspace;

        public DerivationError(Workspace workspace, Database.Derivations.IDerivationError derivationError)
        {
            this.workspace = workspace;
            this.derivationError = derivationError;
        }

        public IEnumerable<IRole> Roles => this.derivationError.Relations
            .Select(v =>
            {
                var metaPopulation = this.workspace.MetaPopulation;
                var relationType = (IRelationType)metaPopulation.FindByTag(v.RoleType.Tag);
                return this.workspace.Instantiate(v.Association.Id).Role(relationType.RoleType);
            });

        public string Message => this.derivationError.ErrorCode;
    }
}
