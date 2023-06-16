// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
