// <copyright file="IPattern.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Domain.Derivations.Rules
{
    using System.Collections.Generic;
    using Allors.Database.Data;
    using Allors.Database.Derivations;
    using Allors.Database.Meta;

    public abstract class Pattern : IPattern
    {
        IEnumerable<Node> IPattern.Tree => this.Tree;
        public IEnumerable<Node> Tree { get; set; }

        public IComposite OfType { get; set; }
    }
}
