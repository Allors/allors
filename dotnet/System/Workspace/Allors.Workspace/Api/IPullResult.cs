// <copyright file="IPullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;

    public interface IPullResult : IResult
    {
        IEnumerable<IConflict> MergeErrors { get; }

        IDictionary<string, IObject[]> Collections { get; }

        IDictionary<string, IObject> Objects { get; }

        IDictionary<string, object> Values { get; }

        T[] GetCollection<T>() where T : class, IObject;

        T[] GetCollection<T>(string key) where T : class, IObject;

        T GetObject<T>() where T : class, IObject;

        T GetObject<T>(string key) where T : class, IObject;

        object GetValue(string key);

        T GetValue<T>(string key);
    }
}
