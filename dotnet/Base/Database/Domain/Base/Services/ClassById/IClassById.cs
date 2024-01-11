// <copyright file="IDatabaseCaches.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Allors.Database.Meta;

    public interface IClassById
    {
        IClass Get(long id);

        void Set(long id, IClass @class);
    }
}
