// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    public class AddCompositeChange : CompositeChange
    {
        public AddCompositeChange(Strategy role, Strategy source) : base(role, source)
        {
        }
    }
}
