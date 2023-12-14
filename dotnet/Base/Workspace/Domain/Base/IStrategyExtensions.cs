// <copyright file="ISessionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    public static partial class IStrategyExtensions
    {
        public static T Cast<T>(this IStrategy @this) where T : class, IObject
        {
            var objectFactory = @this?.Workspace.Services.Get<IObjectFactory>();
            return objectFactory?.Object<T>(@this);
        }
    }
}
