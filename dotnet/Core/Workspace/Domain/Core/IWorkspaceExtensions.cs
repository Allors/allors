// <copyright file="ISessionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public static partial class IWorkspaceExtensions
    {
        public static T Create<T>(this IWorkspace workspace) where T : class, IObject
        {
            var objectFactory = workspace.Services.Get<IObjectFactory>();
            var @class = objectFactory.GetObjectTypeForObject<T>();
            var strategy = workspace.Create((IClass)@class);
            return objectFactory.Object<T>(strategy);
        }

        public static T Instantiate<T>(this IWorkspace workspace, long id) where T : class, IObject
        {
            var objectFactory = workspace.Services.Get<IObjectFactory>();
            return objectFactory.Object<T>(workspace.Instantiate(id));
        }

        public static T Instantiate<T>(this IWorkspace workspace, IStrategy strategy) where T : class, IObject
        {
            var objectFactory = workspace.Services.Get<IObjectFactory>();
            return objectFactory.Object<T>(workspace.Instantiate(strategy));
        }

        public static IEnumerable<T> Instantiate<T>(this IWorkspace workspace, IEnumerable<long> ids) where T : class, IObject
        {
            if (ids == null)
            {
                return Array.Empty<T>();
            }

            var objectFactory = workspace.Services.Get<IObjectFactory>();
            return objectFactory.Object<T>(ids.Select(workspace.Instantiate));
        }


        public static IEnumerable<T> Instantiate<T>(this IWorkspace workspace, IEnumerable<IStrategy> strategies) where T : class, IObject
        {
            if (strategies == null)
            {
                return Array.Empty<T>();
            }

            var objectFactory = workspace.Services.Get<IObjectFactory>();
            return objectFactory.Object<T>(strategies.Select(workspace.Instantiate));
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
