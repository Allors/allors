// <copyright file="ISessionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Linq;

    public static partial class IPullResultExtensions
    {
        public static T[] GetCollection<T>(this IPullResult @this) where T : class, IObject
        {
            var objectFactory = @this.Workspace.Services.Get<IObjectFactory>();
            var objectType = objectFactory.GetObjectTypeForObject<T>();
            var key = objectType.PluralName;
            return @this.GetCollection<T>(key);
        }

        public static T[] GetCollection<T>(this IPullResult @this, string key) where T : class, IObject
        {
            var objectFactory = @this.Workspace.Services.Get<IObjectFactory>();
            return @this.Collections.TryGetValue(key, out var collection) ? objectFactory.Object<T>(collection).ToArray() : null;
        }

        public static T GetObject<T>(this IPullResult @this) where T : class, IObject
        {
            var objectFactory = @this.Workspace.Services.Get<IObjectFactory>();
            var objectType = objectFactory.GetObjectTypeForObject<T>();
            var key = objectType.SingularName;
            return @this.GetObject<T>(key);
        }

        public static T GetObject<T>(this IPullResult @this, string key) where T : class, IObject
        {
            var objectFactory = @this.Workspace.Services.Get<IObjectFactory>();
            return @this.Objects.TryGetValue(key, out var @object) ? objectFactory.Object<T>(@object) : null;
        }
    }
}
