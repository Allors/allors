// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public static class ISignalExtensions
    {
        public static T TrackedValue<T>(this ISignal<T> @this, ITracker tracker)
{
            tracker.Track(@this);
            return @this.Value;
        }
    }
}
