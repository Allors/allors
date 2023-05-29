// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Meta;

    public interface IRole : IRelationEnd
    {
        IRoleType RoleType { get; }

        new object Value { get; set; }

        bool CanRead { get; }

        bool CanWrite { get; }

        bool Exist { get; }

        bool IsModified { get; }

        void Restore();
    }
}
