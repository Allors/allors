// <copyright file="Composite.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta;

using System;

public interface IStaticObjectType : IObjectType, IMetaIdentifiableObject
{
    new Type BoundType { get; internal set; }
}
