// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.ComponentModel;

    public static class IOperandExtensions
    {
        public static T Track<T>(this T @this, ITracker tracker) where T : IOperand
        {
            tracker.Track(@this);
            return @this;
        }
    }
}
