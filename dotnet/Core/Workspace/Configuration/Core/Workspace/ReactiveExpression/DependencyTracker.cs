﻿// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class DependencyTracker : IDependencyTracker
    {
        public ISet<INotifyPropertyChanged> Dependencies { get; } = new HashSet<INotifyPropertyChanged>();


        public void Track(INotifyPropertyChanged dependency)
        {
            this.Dependencies.Add(dependency);
        }
    }
}
