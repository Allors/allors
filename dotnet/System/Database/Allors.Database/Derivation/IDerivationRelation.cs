// <copyright file="IDerivationError.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Derivations;

using Allors.Database.Meta;

public interface IDerivationRelation
{
    IRelationType RelationType { get; }

    IObject Association { get; }

    IObject Role { get; }
}
