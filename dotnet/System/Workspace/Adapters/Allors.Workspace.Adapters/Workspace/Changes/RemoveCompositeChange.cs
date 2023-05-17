﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    public class RemoveCompositeChange : Change
    {
        public RemoveCompositeChange(Strategy role)
        {
            this.Role = role;
        }

        public Strategy Role { get; }
    }
}
