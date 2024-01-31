// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
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

    public interface IRole<T> : IRole, IRelationEnd<T>, IOperand<IRole<T>>
    {
        new T Value { get; set; }
    }
}
