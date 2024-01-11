// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    public abstract class CompositeChange : Change
    {
        protected CompositeChange(Strategy role, Strategy dependee)
        {
            this.Role = role;
            this.Dependee = dependee;
        }

        public Strategy Role { get; }
    }
}
