// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    public class SetUnitChange : Change
    {
        public SetUnitChange(object role) => this.Role = role;

        public object Role { get; }
    }
}
