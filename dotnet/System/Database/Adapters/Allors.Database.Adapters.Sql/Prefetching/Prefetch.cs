﻿// <copyright file="Prefetch.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;

internal class Prefetch
{
    private readonly Prefetcher prefetcher;
    private readonly PrefetchPolicy prefetchPolicy;
    private readonly HashSet<Reference> references;

    public Prefetch(Prefetcher prefetcher, PrefetchPolicy prefetchPolicy, HashSet<Reference> references)
    {
        this.prefetcher = prefetcher;
        this.references = references;
        this.prefetchPolicy = prefetchPolicy;
    }

    public void Execute()
    {
        var leafs = new HashSet<long>();

        var nestedObjectIdsByRoleType = new Dictionary<RoleType, HashSet<long>>();

        // Phase 1
        var unitRoles = false;
        foreach (var prefetchRule in this.prefetchPolicy)
        {
            var relationEndType = prefetchRule.RelationEndType;
            if (relationEndType is RoleType)
            {
                var roleType = (RoleType)relationEndType;
                var objectType = roleType.ObjectType;
                if (objectType.IsUnit)
                {
                    if (!unitRoles)
                    {
                        unitRoles = true;

                        var referencesByClass = new Dictionary<Class, List<Reference>>();
                        foreach (var reference in this.references)
                        {
                            if (!referencesByClass.TryGetValue(reference.Class, out var classedReferences))
                            {
                                classedReferences = new List<Reference>();
                                referencesByClass.Add(reference.Class, classedReferences);
                            }

                            classedReferences.Add(reference);
                        }

                        foreach (var dictionaryEntry in referencesByClass)
                        {
                            var @class = dictionaryEntry.Key;
                            var classedReferences = dictionaryEntry.Value;

                            var referencesWithoutCachedRole = new HashSet<Reference>();
                            foreach (var reference in classedReferences)
                            {
                                var roles = this.prefetcher.Transaction.State.GetOrCreateRoles(reference);
                                if (!roles.PrefetchTryGetUnitRole(roleType))
                                {
                                    referencesWithoutCachedRole.Add(reference);
                                }
                            }

                            this.prefetcher.PrefetchUnitRoles(@class, referencesWithoutCachedRole, roleType);
                        }
                    }
                }
                else
                {
                    var nestedPrefetchPolicy = prefetchRule.PrefetchPolicy;
                    var existNestedPrefetchPolicy = nestedPrefetchPolicy != null;
                    var nestedObjectIds = existNestedPrefetchPolicy ? new HashSet<long>() : null;
                    if (existNestedPrefetchPolicy)
                    {
                        nestedObjectIdsByRoleType[roleType] = nestedObjectIds;
                    }

                    if (roleType.IsOne)
                    {
                        if (roleType.ExistExclusiveClasses)
                        {
                            this.prefetcher.PrefetchCompositeRoleObjectTable(this.references, roleType, nestedObjectIds, leafs);
                        }
                        else
                        {
                            this.prefetcher.PrefetchCompositeRoleRelationTable(this.references, roleType, nestedObjectIds, leafs);
                        }
                    }
                    else
                    {
                        var associationType = roleType.AssociationType;
                        if (associationType.IsOne && roleType.ExistExclusiveClasses)
                        {
                            this.prefetcher.PrefetchCompositesRoleObjectTable(this.references, roleType, nestedObjectIds, leafs);
                        }
                        else
                        {
                            this.prefetcher.PrefetchCompositesRoleRelationTable(this.references, roleType, nestedObjectIds, leafs);
                        }
                    }
                }
            }
            else
            {
                var associationType = (AssociationType)relationEndType;
                var roleType = associationType.RoleType;

                var nestedPrefetchPolicy = prefetchRule.PrefetchPolicy;
                var existNestedPrefetchPolicy = nestedPrefetchPolicy != null;
                var nestedObjectIds = existNestedPrefetchPolicy ? new HashSet<long>() : null;
                if (existNestedPrefetchPolicy)
                {
                    nestedObjectIdsByRoleType[roleType] = nestedObjectIds;
                }

                if (!(associationType.IsMany && roleType.IsMany) && associationType.RoleType.ExistExclusiveClasses)
                {
                    if (associationType.IsOne)
                    {
                        this.prefetcher.PrefetchCompositeAssociationObjectTable(this.references, associationType, nestedObjectIds, leafs);
                    }
                    else
                    {
                        this.prefetcher.PrefetchCompositesAssociationObjectTable(this.references, associationType, nestedObjectIds, leafs);
                    }
                }
                else if (associationType.IsOne)
                {
                    this.prefetcher.PrefetchCompositeAssociationRelationTable(this.references, associationType, nestedObjectIds, leafs);
                }
                else
                {
                    this.prefetcher.PrefetchCompositesAssociationRelationTable(this.references, associationType, nestedObjectIds, leafs);
                }
            }
        }

        var objectIds = new HashSet<long>();
        if (leafs.Count > 0)
        {
            objectIds.UnionWith(leafs);
        }

        foreach (var nestedObjectIds in nestedObjectIdsByRoleType.Values)
        {
            objectIds.UnionWith(nestedObjectIds);
        }

        var referenceByObjectId = this.prefetcher.GetReferencesForPrefetching(objectIds).ToDictionary(v => v.ObjectId);

        foreach (var prefetchRule in this.prefetchPolicy)
        {
            var relationEndType = prefetchRule.RelationEndType;
            if (relationEndType is RoleType)
            {
                var roleType = (RoleType)relationEndType;
                var objectType = roleType.ObjectType;
                if (!objectType.IsUnit)
                {
                    var nestedPrefetchPolicy = prefetchRule.PrefetchPolicy;
                    var existNestedPrefetchPolicy = nestedPrefetchPolicy != null;
                    if (existNestedPrefetchPolicy)
                    {
                        var nestedObjectIds = nestedObjectIdsByRoleType[roleType];
                        var nestedReferenceIds = new HashSet<Reference>(nestedObjectIds.Where(v => referenceByObjectId.ContainsKey(v))
                            .Select(v => referenceByObjectId[v]));
                        new Prefetch(this.prefetcher, nestedPrefetchPolicy, nestedReferenceIds).Execute();
                    }
                }
            }
            else
            {
                var associationType = (AssociationType)relationEndType;
                var roleType = associationType.RoleType;

                var nestedPrefetchPolicy = prefetchRule.PrefetchPolicy;
                var existNestedPrefetchPolicy = nestedPrefetchPolicy != null;
                if (existNestedPrefetchPolicy)
                {
                    var nestedObjectIds = nestedObjectIdsByRoleType[roleType];
                    var nestedReferenceIds = new HashSet<Reference>(nestedObjectIds.Where(v => referenceByObjectId.ContainsKey(v))
                        .Select(v => referenceByObjectId[v]));
                    new Prefetch(this.prefetcher, nestedPrefetchPolicy, nestedReferenceIds).Execute();
                }
            }
        }
    }
}
