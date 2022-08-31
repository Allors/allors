// <copyright file="Method.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Response
{
    using Meta;

    public struct Role
    {
        public Role(IObject @object, RelationType relationType)
        {
            this.Object = @object;
            this.RelationType = relationType;
        }

        public IObject Object { get; }

        public RelationType RelationType { get; }
    }
}
