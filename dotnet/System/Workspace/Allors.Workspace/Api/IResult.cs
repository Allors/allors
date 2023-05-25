// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;

    public interface IResult
    {
        IWorkspace Workspace { get; }

        bool HasErrors { get; }

        string ErrorMessage { get; }

        IEnumerable<IStrategy> VersionErrors { get; }

        IEnumerable<IStrategy> AccessErrors { get; }

        IEnumerable<IStrategy> MissingErrors { get; }

        IEnumerable<IDerivationError> DerivationErrors { get; }
    }
}
