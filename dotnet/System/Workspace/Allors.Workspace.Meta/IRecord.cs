﻿// <copyright file="IRecord.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Workspace.Meta
{
    using System.Collections.Generic;

    public interface IRecord : IDataType
    {
        IReadOnlyList<Allors.Workspace.Meta.IFieldType> FieldTypes { get; }
    }
}
