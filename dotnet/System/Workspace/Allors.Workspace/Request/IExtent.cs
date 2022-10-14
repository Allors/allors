// <copyright file="IExtent.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request.Visitor;

    public interface IExtent : IVisitable
    {
        IComposite ObjectType { get; }

        // TODO: move to Result
        Sort[] Sorting { get; set; }
    }
}
