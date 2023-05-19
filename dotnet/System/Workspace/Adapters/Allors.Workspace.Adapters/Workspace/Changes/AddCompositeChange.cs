// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    public class AddCompositeChange : Change
    {
        public AddCompositeChange(Strategy role, bool isDirect)
        {
            this.Role = role;
            this.IsDirect = isDirect;
        }

        public Strategy Role { get; }
    }
}
