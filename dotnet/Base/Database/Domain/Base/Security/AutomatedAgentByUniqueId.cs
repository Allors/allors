// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class AutomatedAgentByUniqueId 
    {
        public AutomatedAgent Guest => this.cache[AutomatedAgent.GuestId];

        public AutomatedAgent System => this.cache[AutomatedAgent.SystemId];
    }
}
