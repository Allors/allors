// <copyright file="LocalPullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct;

using System.Collections.Generic;
using System.Linq;
using Allors.Database;
using Allors.Database.Data;
using Allors.Database.Domain;
using Allors.Database.Meta;
using Allors.Database.Security;
using Allors.Database.Services;
using Allors.Workspace.Protocol.Direct;
using Allors.Workspace.Request;
using Allors.Workspace.Response;
using IComposite = Allors.Workspace.Meta.IComposite;
using IObject = Allors.Workspace.Response.IObject;
using Node = Allors.Database.Data.Node;

public class Pull : Result, IPullResult
{
    private IDictionary<string, IObject[]> collections;
    private IDictionary<string, IObject> objects;

    public Pull(Workspace workspace) : base(workspace)
    {
        this.Workspace = workspace.Connection;
        var database = this.Workspace.Database;
        this.Transaction = database.CreateTransaction();

        this.AllowedClasses = database.Services.Get<IMetaCache>()
            .GetWorkspaceClasses(this.Workspace.Name);
        this.PreparedSelects = database.Services.Get<IPreparedSelects>();
        this.PreparedExtents = database.Services.Get<IPreparedExtents>();
        this.PrefetchPolicyCache = database.Services.Get<IPrefetchPolicyCache>();

        this.AccessControl = this.Transaction.Services.Get<IWorkspaceAclsService>()
            .Create(this.Workspace.Name);

        this.DatabaseObjects = new HashSet<Database.IObject>();
    }

    public IAccessControl AccessControl { get; }

    public HashSet<Database.IObject> DatabaseObjects { get; }

    private Dictionary<string, ISet<Database.IObject>> DatabaseCollectionsByName { get; } = new();

    private Dictionary<string, Database.IObject> DatabaseObjectByName { get; } = new();

    private Dictionary<string, object> ValueByName { get; } = new();

    private Connection Workspace { get; }

    private ITransaction Transaction { get; }

    private IReadOnlySet<IClass> AllowedClasses { get; }

    private IPreparedSelects PreparedSelects { get; }

    private IPreparedExtents PreparedExtents { get; }

    public IPrefetchPolicyCache PrefetchPolicyCache { get; }

    public IDictionary<string, IObject[]> Collections =>
        this.collections ??= this.DatabaseCollectionsByName.ToDictionary(v => v.Key,
            v => v.Value.Select(w => (IObject)base.Workspace.GetObject(w.Id)).ToArray());

    public IDictionary<string, IObject> Objects =>
        this.objects ??= this.DatabaseObjectByName.ToDictionary(v => v.Key,
            v => (IObject)base.Workspace.GetObject(v.Value.Id));

    public IDictionary<string, object> Values => this.ValueByName;

    public IObject[] GetCollection(IComposite objectType)
    {
        var key = objectType.PluralName;
        return this.GetCollection(key);
    }

    public IObject[] GetCollection(string key) =>
        this.Collections.TryGetValue(key, out var collection) ? collection?.ToArray() : null;

    public IObject GetObject(IComposite objectType)
    {
        var key = objectType.SingularName;
        return this.GetObject(key);
    }

    public IObject GetObject(string key) => this.Objects.TryGetValue(key, out var @object) ? @object : null;

    public object GetValue(string key) => this.Values[key];

    public T GetValue<T>(string key) => (T)this.GetValue(key);

    public void AddCollection(string name, Database.Meta.IComposite objectType, in IEnumerable<Database.IObject> collection, Node[] tree)
    {
        switch (collection)
        {
            case ICollection<Database.IObject> list:
                this.AddCollectionInternal(name, list, tree);
                break;
            default:
                this.AddCollectionInternal(name, collection.ToArray(), tree);
                break;
        }
    }

    public void AddObject(string name, Database.IObject @object, Node[] tree) => this.AddObjectInternal(name, @object, tree);

    public void AddValue(string name, object value)
    {
        if (value != null)
        {
            this.ValueByName.Add(name, value);
        }
    }

    public void Execute(IEnumerable<PullRequest> workspacePulls)
    {
        var visitor = new ToDatabaseVisitor(this.Transaction);
        foreach (var pull in workspacePulls.Select(v => visitor.Visit(v)))
        {
            if (pull.Object != null)
            {
                var pullInstantiate = new PullInstantiate(this.Transaction, pull, this.AccessControl, this.PreparedSelects);
                pullInstantiate.Execute(this);
            }
            else
            {
                var pullExtent = new PullExtent(this.Transaction, pull, this.AccessControl, this.PreparedSelects, this.PreparedExtents);
                pullExtent.Execute(this);
            }
        }
    }

    public void AddCollection(string name, in ICollection<Database.IObject> collection) =>
        this.AddCollectionInternal(name, collection, null);

    public void AddObject(string name, Database.IObject @object) => this.AddObjectInternal(name, @object, null);

    private void AddObjectInternal(string name, Database.IObject @object, Node[] tree)
    {
        if (@object == null || this.AllowedClasses?.Contains(@object.Strategy.Class) != true || this.AccessControl[@object].IsMasked())
        {
            return;
        }


        this.DatabaseObjects.Add(@object);
        this.DatabaseObjectByName[name] = @object;
        tree?.Resolve(@object, this.AccessControl, this.Add, this.PrefetchPolicyCache, this.Transaction);
    }

    private void AddCollectionInternal(string name, in ICollection<Database.IObject> collection, Node[] tree)
    {
        if (collection?.Count > 0)
        {
            this.DatabaseCollectionsByName.TryGetValue(name, out var existingCollection);

            var filteredCollection = collection.Where(v =>
                this.AllowedClasses != null && this.AllowedClasses.Contains(v.Strategy.Class) && !this.AccessControl[v].IsMasked());

            if (tree != null)
            {
                // TODO: 
                //var prefetchPolicy = tree.BuildPrefetchPolicy();

                ICollection<Database.IObject> newCollection;

                if (existingCollection != null)
                {
                    newCollection = filteredCollection.ToArray();
                    //this.Transaction.Prefetch(prefetchPolicy, newCollection);
                    existingCollection.UnionWith(newCollection);
                }
                else
                {
                    var newSet = new HashSet<Database.IObject>(filteredCollection);
                    newCollection = newSet;
                    //this.Transaction.Prefetch(prefetchPolicy, newCollection);
                    this.DatabaseCollectionsByName.Add(name, newSet);
                }

                this.DatabaseObjects.UnionWith(newCollection);

                tree.Resolve(newCollection, this.AccessControl, this.Add, this.PrefetchPolicyCache, this.Transaction);
            }
            else if (existingCollection != null)
            {
                existingCollection.UnionWith(filteredCollection);
            }
            else
            {
                var newWorkspaceCollection = new HashSet<Database.IObject>(filteredCollection);
                this.DatabaseCollectionsByName.Add(name, newWorkspaceCollection);
                this.DatabaseObjects.UnionWith(newWorkspaceCollection);
            }
        }
    }

    private void Add(Database.IObject @object)
    {
        if (this.AccessControl[@object].IsMasked())
        {
            return;
        }

        this.DatabaseObjects.Add(@object);
    }
}
