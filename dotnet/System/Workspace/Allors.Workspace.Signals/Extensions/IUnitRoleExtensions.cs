// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public static class ICompositeRoleExtensions
    {
        public static T TrackedValue<T>(this ICompositeRole<T> @this, ITracker tracker)
            where T : class, IObject
        {
            tracker.Track(@this);
            return @this.Value;
        }
    }
}
