// <copyright file="IPullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Response
{
    using System.Collections.Generic;
    using Meta;

    public interface IPullResult : IResult
    {
        IDictionary<string, IObject[]> Collections { get; }

        IDictionary<string, IObject> Objects { get; }

        IDictionary<string, object> Values { get; }

        public IObject[] GetCollection(IComposite objectType);

        public IObject[] GetCollection(string key);

        public IObject GetObject(IComposite objectType);

        public IObject GetObject(string key);

        public object GetValue(string key);

        public T GetValue<T>(string key);
    }
}
