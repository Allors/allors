// <copyright file="IWorkspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;

    public class WorkspaceChangedEventArgs : EventArgs
    {
        public WorkspaceChanges WorkspaceChanges { get; }

        public WorkspaceChangedEventArgs(WorkspaceChanges workspaceChanges) => this.WorkspaceChanges = workspaceChanges;
    }
}
