// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Signals
{
    public static class ISignalExtensions
    {
        public static T Track<T>(this T @this, ITracker tracker) where T : INotifyChanged
        {
            tracker.Track(@this);
            return @this;
        }
    }
}
