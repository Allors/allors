// <copyright file="ChangeLog.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;

internal sealed class ChangeLog
{
    private readonly Dictionary<Strategy, ISet<AssociationType>> associationTypesByRole;
    private readonly HashSet<Strategy> created;
    private readonly HashSet<IStrategy> deleted;

    private readonly Dictionary<Strategy, Original> originalByStrategy;

    private readonly Dictionary<Strategy, ISet<RoleType>> roleTypesByAssociation;

    internal ChangeLog()
    {
        this.created = new HashSet<Strategy>();
        this.deleted = new HashSet<IStrategy>();

        this.roleTypesByAssociation = new Dictionary<Strategy, ISet<RoleType>>();
        this.associationTypesByRole = new Dictionary<Strategy, ISet<AssociationType>>();

        this.originalByStrategy = new Dictionary<Strategy, Original>();
    }

    internal void OnCreated(Strategy strategy) => this.created.Add(strategy);

    internal void OnDeleted(Strategy strategy) => this.deleted.Add(strategy);

    internal void OnChangingUnitRole(Strategy association, RoleType roleType, object previousRole)
    {
        this.Original(association).OnChangingUnitRole(roleType, previousRole);

        this.RoleTypes(association).Add(roleType);
    }

    internal void OnChangingCompositeRole(Strategy association, RoleType roleType, Strategy newRole, Strategy previousRole)
    {
        this.Original(association).OnChangingCompositeRole(roleType, previousRole);

        if (previousRole != null)
        {
            this.AssociationTypes(previousRole).Add(roleType.AssociationType);
        }

        if (newRole != null)
        {
            this.AssociationTypes(newRole).Add(roleType.AssociationType);
        }

        this.RoleTypes(association).Add(roleType);
    }

    internal void OnChangingCompositesRole(Strategy association, RoleType roleType, Strategy changedRole,
        IEnumerable<Strategy> previousRole)
    {
        this.Original(association).OnChangingCompositesRole(roleType, previousRole);

        if (changedRole != null)
        {
            this.AssociationTypes(changedRole).Add(roleType.AssociationType);
        }

        this.RoleTypes(association).Add(roleType);
    }

    internal void OnChangingCompositeAssociation(Strategy role, AssociationType associationType, Strategy previousAssociation)
        => this.Original(role).OnChangingCompositeAssociation(associationType, previousAssociation);

    internal void OnChangingCompositesAssociation(Strategy role, AssociationType roleType, IEnumerable<Strategy> previousAssociation)
        => this.Original(role).OnChangingCompositesAssociation(roleType, previousAssociation);

    internal ChangeSet Checkpoint() =>
        new(
            this.created != null ? new HashSet<IObject>(this.created.Select(v => v.GetObject())) : null,
            this.deleted,
            this.RoleTypesByAssociation().ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            this.AssociationTypesByRole().ToDictionary(kvp => kvp.Key, kvp => kvp.Value));

    private ISet<RoleType> RoleTypes(Strategy associationId)
    {
        if (!this.roleTypesByAssociation.TryGetValue(associationId, out var roleTypes))
        {
            roleTypes = new HashSet<RoleType>();
            this.roleTypesByAssociation[associationId] = roleTypes;
        }

        return roleTypes;
    }

    private ISet<AssociationType> AssociationTypes(Strategy roleId)
    {
        if (!this.associationTypesByRole.TryGetValue(roleId, out var associationTypes))
        {
            associationTypes = new HashSet<AssociationType>();
            this.associationTypesByRole[roleId] = associationTypes;
        }

        return associationTypes;
    }

    private Original Original(Strategy association)
    {
        if (this.originalByStrategy.TryGetValue(association, out var original))
        {
            return original;
        }

        original = new Original(association);
        this.originalByStrategy.Add(association, original);
        return original;
    }

    private IEnumerable<KeyValuePair<IObject, ISet<RoleType>>> RoleTypesByAssociation()
    {
        foreach ((Strategy strategy, ISet<RoleType> roleTypes) in this.roleTypesByAssociation)
        {
            if (strategy.IsDeleted)
            {
                continue;
            }

            var original = this.Original(strategy);
            original.Trim(roleTypes);

            if (roleTypes.Count <= 0)
            {
                continue;
            }

            var @object = strategy.GetObject();
            yield return new KeyValuePair<IObject, ISet<RoleType>>(@object, roleTypes);
        }
    }

    private IEnumerable<KeyValuePair<IObject, ISet<AssociationType>>> AssociationTypesByRole()
    {
        foreach ((Strategy strategy, ISet<AssociationType> associationTypes) in this.associationTypesByRole)
        {
            if (strategy.IsDeleted)
            {
                continue;
            }

            var original = this.Original(strategy);
            original.Trim(associationTypes);

            if (associationTypes.Count <= 0)
            {
                continue;
            }

            var @object = strategy.GetObject();
            yield return new KeyValuePair<IObject, ISet<AssociationType>>(@object, associationTypes);
        }
    }
}
