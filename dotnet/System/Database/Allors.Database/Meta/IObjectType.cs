// <copyright file="IObjectType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;

    public interface IObjectType : IFieldObjectType, IComparable
    {
        bool IsUnit { get; }

        bool IsComposite { get; }

        bool IsInterface { get; }

        bool IsClass { get; }
    }
}
