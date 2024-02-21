﻿// <copyright file="TreeCache.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration
{
    using System.Collections.Generic;
    using Domain;
    using Meta;

    public class WorkspaceMask : IWorkspaceMask
    {
        private readonly Dictionary<IClass, IRoleType> masks;

        public WorkspaceMask(IMetaIndex m) =>
            this.masks = new Dictionary<IClass, IRoleType>
            {
                { m.TrimFrom, m.TrimFrom.Name },
                { m.TrimTo, m.TrimTo.Name },
            };

        public IDictionary<IClass, IRoleType> GetMasks(string workspaceName) => this.masks;
    }
}
