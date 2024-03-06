// <copyright file="Extent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsExtentMemory type.
// </summary>

namespace Allors.Database.Adapters.Memory;

using Meta;

public interface IInternalExtent 
{
    void Invalidate();

    void CheckForRoleType(RoleType roleType);

    void CheckForAssociationType(AssociationType associationType);
}
