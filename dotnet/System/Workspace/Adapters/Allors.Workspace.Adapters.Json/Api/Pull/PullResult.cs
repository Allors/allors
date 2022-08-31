// <copyright file="RemotePullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api.Pull;
    using Meta;

    public class PullResult : Result, IPullResult
    {
        private IDictionary<string, IObject> objects;

        private IDictionary<string, IObject[]> collections;

        private IDictionary<string, object> values;

        private readonly PullResponse pullResponse;

        public PullResult(Workspace workspace, PullResponse response) : base(workspace, response) => this.pullResponse = response;

        public IDictionary<string, IObject> Objects => this.objects ??= this.pullResponse.o.ToDictionary(pair => pair.Key.ToUpperInvariant(), pair => (IObject)this.Workspace.GetObject(pair.Value));

        public IDictionary<string, IObject[]> Collections => this.collections ??= this.pullResponse.c.ToDictionary(pair => pair.Key.ToUpperInvariant(), pair => pair.Value.Select(this.Workspace.GetObject).Cast<IObject>().ToArray());

        public IDictionary<string, object> Values => this.values ??= this.pullResponse.v.ToDictionary(pair => pair.Key.ToUpperInvariant(), pair => pair.Value);

        public IObject[] GetCollection(IComposite objectType)
        {
            var key = objectType.PluralName.ToUpperInvariant();
            return this.GetCollection(key);
        }

        public IObject[] GetCollection(string key) => this.Collections.TryGetValue(key.ToUpperInvariant(), out var collection) ? collection?.ToArray() : null;

        public IObject GetObject(IComposite objectType)
        {
            var key = objectType.SingularName.ToUpperInvariant();
            return this.GetObject(key);
        }

        public IObject GetObject(string key) => this.Objects.TryGetValue(key.ToUpperInvariant(), out var @object) ? @object : null;

        public object GetValue(string key) => this.Values[key.ToUpperInvariant()];

        public T GetValue<T>(string key) => (T)this.GetValue(key.ToUpperInvariant());
    }
}
