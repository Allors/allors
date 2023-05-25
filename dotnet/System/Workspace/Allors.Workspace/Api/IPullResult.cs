// <copyright file="IPullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System.Collections.Generic;
    using Meta;

    public interface IPullResult : IResult
    {
        IEnumerable<IConflict> MergeErrors { get; }

        IDictionary<string, IStrategy[]> Collections { get; }

        IDictionary<string, IStrategy> Objects { get; }

        IDictionary<string, object> Values { get; }

        IStrategy[] GetCollection(IComposite composite);

        IStrategy[] GetCollection(string key);

        IStrategy GetObject(IComposite composite);

        IStrategy GetObject(string key);

        object GetValue(string key);

        T GetValue<T>(string key);
    }
}
