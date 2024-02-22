// <copyright file="PullResponseBuilder.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Allors.Protocol.Json;
using Allors.Protocol.Json.Api.Pull;
using Allors.Database.Data;
using Allors.Database.Derivations;
using Allors.Database.Domain;
using Allors.Database.Meta;
using Allors.Database.Security;
using Allors.Database.Services;
using Allors.Shared.Ranges;

public class PullResponseBuilder
{
    private readonly Dictionary<string, ISet<IObject>> collectionsByName = new();

    private readonly HashSet<IObject> maskedObjects;
    private readonly Dictionary<string, IObject> objectByName = new();

    private readonly IPrefetchPolicyCache prefetchPolicyCache;
    private readonly IUnitConvert unitConvert;
    private readonly HashSet<IObject> unmaskedObjects;
    private readonly Dictionary<string, object> valueByName = new();

    private List<IValidation> errors;

    public PullResponseBuilder(
        ITransaction transaction,
        IAccessControl accessControl,
        IReadOnlySet<Class> allowedClasses,
        IPreparedSelects preparedSelects,
        IPreparedExtents preparedExtents,
        IUnitConvert unitConvert,
        IPrefetchPolicyCache prefetchPolicyCache,
        CancellationToken cancellationToken)
    {
        this.unitConvert = unitConvert;
        this.Transaction = transaction;

        this.AccessControl = accessControl;
        this.AllowedClasses = allowedClasses;
        this.PreparedSelects = preparedSelects;
        this.PreparedExtents = preparedExtents;
        this.CancellationToken = cancellationToken;

        this.prefetchPolicyCache = prefetchPolicyCache;

        this.maskedObjects = new HashSet<IObject>();
        this.unmaskedObjects = new HashSet<IObject>();
    }

    public ITransaction Transaction { get; }

    public IAccessControl AccessControl { get; }

    public IReadOnlySet<Class> AllowedClasses { get; }

    public IPreparedSelects PreparedSelects { get; }

    public IPreparedExtents PreparedExtents { get; }
    public CancellationToken CancellationToken { get; }

    public void AddError(IValidation validation)
    {
        this.errors ??= new List<IValidation>();
        this.errors.Add(validation);
    }

    public void AddCollection(string name, Composite objectType, in IEnumerable<IObject> collection) =>
        this.AddCollectionInternal(name, objectType, collection, null);

    public void AddCollection(string name, Composite objectType, in IEnumerable<IObject> collection, Node[] tree)
    {
        switch (collection)
        {
            case ICollection<IObject> list:
                this.AddCollectionInternal(name, objectType, list, tree);
                break;
            default:
            {
                this.AddCollectionInternal(name, objectType, collection.ToArray(), tree);
                break;
            }
        }
    }

    public void AddObject(string name, IObject @object) => this.AddObjectInternal(name, @object);

    public void AddObject(string name, IObject @object, Node[] tree) => this.AddObjectInternal(name, @object, tree);

    public void AddValue(string name, object value)
    {
        if (value != null)
        {
            this.valueByName.Add(name, value);
        }
    }

    private void AddObjectInternal(string name, IObject @object, Node[] tree = null)
    {
        if (@object == null || this.maskedObjects.Contains(@object))
        {
            return;
        }

        this.Transaction.Prefetch(this.prefetchPolicyCache.Security, @object);

        if (!this.Include(@object))
        {
            this.maskedObjects.Add(@object);
            return;
        }

        this.unmaskedObjects.Add(@object);
        this.objectByName[name] = @object;
        tree?.Resolve(@object, this.AccessControl, this.Add, this.prefetchPolicyCache, this.Transaction);
    }

    private void AddCollectionInternal(string name, Composite objectType, in IEnumerable<IObject> enumerable, Node[] tree)
    {
        var collection = enumerable as ICollection<IObject> ?? enumerable.ToArray();

        if (collection.Count == 0)
        {
            return;
        }

        this.Transaction.Prefetch(this.prefetchPolicyCache.Security, collection);
        var trimmed = collection.Where(this.Include);

        this.collectionsByName.TryGetValue(name, out var existingCollection);

        if (tree != null)
        {
            ICollection<IObject> newCollection;

            if (existingCollection != null)
            {
                newCollection = trimmed.ToArray();
                existingCollection.UnionWith(newCollection);
            }
            else
            {
                var newSet = new HashSet<IObject>(trimmed);
                newCollection = newSet;
                this.collectionsByName.Add(name, newSet);
            }

            this.unmaskedObjects.UnionWith(newCollection);

            tree.Resolve(newCollection, this.AccessControl, this.Add, this.prefetchPolicyCache, this.Transaction);
        }
        else
        {
            if (existingCollection != null)
            {
                existingCollection.UnionWith(trimmed);
            }
            else
            {
                var newWorkspaceCollection = new HashSet<IObject>(trimmed);
                this.collectionsByName.Add(name, newWorkspaceCollection);
                this.unmaskedObjects.UnionWith(newWorkspaceCollection);
            }
        }
    }

    public PullResponse Build(PullRequest pullRequest = null)
    {
        if (pullRequest?.l != null)
        {
            var pulls = pullRequest.l.FromJson(this.Transaction, this.unitConvert);

            foreach (var pull in pulls)
            {
                this.ThrowIfCancellationRequested();

                if (pull.Object != null)
                {
                    var pullInstantiate = new PullInstantiate(this.Transaction, pull, this.AccessControl, this.PreparedSelects);
                    pullInstantiate.Execute(this);
                }
                else
                {
                    var pullExtent = new PullExtent(this.Transaction, pull, this.AccessControl, this.PreparedSelects, this.PreparedExtents,
                        this.prefetchPolicyCache);
                    pullExtent.Execute(this);
                }
            }
        }

        // Serialize
        var versionByGrant = new Dictionary<long, long>();
        var versionByRevocation = new Dictionary<long, long>();

        this.ThrowIfCancellationRequested();

        var pullResponse = new PullResponse
        {
            p = this.unmaskedObjects.Select(v =>
            {
                var accessControlList = this.AccessControl[v];
                var grants = accessControlList.Grants;
                var revocations = accessControlList.Revocations;

                foreach (var grant in grants)
                {
                    versionByGrant[grant.Id] = grant.Version;
                }

                foreach (var revocation in revocations)
                {
                    versionByRevocation[revocation.Id] = revocation.Version;
                }

                return new PullResponseObject
                {
                    i = v.Strategy.ObjectId,
                    v = v.Strategy.ObjectVersion,
                    g = ValueRange<long>.Import(grants.Select(w => w.Id)).Save(),
                    r = ValueRange<long>.Import(revocations.Select(w => w.Id))
                        .Save(),
                };
            }).ToArray(),
            o = this.objectByName.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Id),
            c = this.collectionsByName.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(obj => obj.Id).ToArray()),
            v = this.valueByName,
        };

        pullResponse.g = versionByGrant.Count > 0 ? versionByGrant.Select(v => new[] { v.Key, v.Value }).ToArray() : null;
        pullResponse.r = versionByRevocation.Count > 0 ? versionByRevocation.Select(v => new[] { v.Key, v.Value }).ToArray() : null;

        return pullResponse;
    }

    public bool Include(IObject @object)
    {
        if (@object == null || this.AllowedClasses?.Contains(@object.Strategy.Class) != true ||
            this.maskedObjects.Contains(@object))
        {
            return false;
        }

        if (this.AccessControl[@object].IsMasked())
        {
            this.maskedObjects.Add(@object);
            return false;
        }

        return true;
    }

    private void Add(IObject @object)
    {
        if (this.Include(@object))
        {
            this.unmaskedObjects.Add(@object);
        }
    }

    private void ThrowIfCancellationRequested() =>
        //if (this.CancellationToken.IsCancellationRequested)
        //{
        //    Debugger.Break();
        //}
        this.CancellationToken.ThrowIfCancellationRequested();
}
