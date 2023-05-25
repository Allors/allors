// <copyright file="ISessionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using Meta;

    public static partial class IWorkspaceExtensions
    {
        public static T Create<T>(this IWorkspace workspace) where T : class, IObject
        {
            var objectFactory = workspace.Services.Get<IObjectFactory>();
            var @class = objectFactory.GetObjectType<T>();
            var strategy = workspace.Create((IClass)@class);
            return objectFactory.Instantiate<T>(strategy);
        }

        public static T Instantiate<T>(this IWorkspace workspace, IStrategy strategy) where T : class, IObject
        {
            var objectFactory = workspace.Services.Get<IObjectFactory>();
            return objectFactory.Instantiate<T>(strategy);
        }

        public static DateTime Now(this IWorkspace workspace)
        {
            var now = DateTime.UtcNow;

            var timeService = workspace.Services.Get<ITime>();
            var timeShift = timeService.Shift;
            if (timeShift != null)
            {
                now = now.Add((TimeSpan)timeShift);
            }

            return now;
        }
    }
}
