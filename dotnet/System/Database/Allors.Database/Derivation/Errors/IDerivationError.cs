// <copyright file="IDerivationError.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Derivations;

using Allors.Database.Meta;

public interface IDerivationError
{
    IDerivationRelation[] Relations { get; }

    RoleType[] RoleTypes { get; }

    string ErrorCode { get; }
}
