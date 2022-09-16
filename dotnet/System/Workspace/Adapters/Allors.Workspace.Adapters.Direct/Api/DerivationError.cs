// <copyright file="RemoteDerivationError.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct;

using System.Collections.Generic;
using System.Linq;
using Meta;
using Response;

public class DerivationError : IDerivationError
{
    private readonly Database.Derivations.IDerivationError derivationError;
    private readonly Workspace workspace;

    public DerivationError(Workspace workspace, Database.Derivations.IDerivationError derivationError)
    {
        this.workspace = workspace;
        this.derivationError = derivationError;
    }

    public IEnumerable<Role> Roles => this.derivationError.Relations
        .Select(v =>
            new Role(
                this.workspace.GetObject(v.Association.Id),
                (RelationType)this.workspace.Connection.MetaPopulation.FindByTag(v.RelationType.Tag)));

    public string Message => this.derivationError.Message;
}
