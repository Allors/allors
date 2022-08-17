// <copyright file="ChangeSet.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the AllorsChangeSetMemory type.
// </summary>

namespace Allors.Workspace.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Collections;
    using Meta;
    using Shared.Ranges;

    public sealed class ChangeSet : IChangeSet
    {
        public ChangeSet(Session session, ISet<IStrategy> created, ISet<IStrategy> instantiated)
        {
            this.Session = session;
            this.Created = created ?? EmptySet<IStrategy>.Instance;
            this.Instantiated = instantiated ?? EmptySet<IStrategy>.Instance;
            this.AssociationsByRoleType = new Dictionary<RoleType, ISet<IStrategy>>();
            this.RolesByAssociationType = new Dictionary<AssociationType, ISet<IStrategy>>();
        }

        private Session Session { get; }

        ISession IChangeSet.Session => this.Session;

        public ISet<IStrategy> Created { get; }

        public ISet<IStrategy> Instantiated { get; }

        public IDictionary<RoleType, ISet<IStrategy>> AssociationsByRoleType { get; }

        public IDictionary<AssociationType, ISet<IStrategy>> RolesByAssociationType { get; }

        public void AddSessionStateChanges(IDictionary<IPropertyType, IDictionary<Strategy, object>> sessionStateChangeSet)
        {
            foreach (var kvp in sessionStateChangeSet)
            {
                var strategies = new HashSet<IStrategy>(kvp.Value.Keys);

                switch (kvp.Key)
                {
                    case AssociationType associationType:
                        this.RolesByAssociationType.Add(associationType, strategies);
                        break;
                    case RoleType roleType:
                        this.AssociationsByRoleType.Add(roleType, strategies);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"PropertyType {kvp.Key}");
                }
            }
        }

        public void DiffUnit(Strategy association, RelationType relationType, object current, object previous)
        {
            if (!Equals(current, previous))
            {
                this.AddAssociation(relationType, association);
            }
        }

        public void DiffComposite(Strategy association, RelationType relationType, Strategy current, long? previous)
        {
            if (Equals(current?.Id, previous))
            {
                return;
            }

            if (previous != null)
            {
                this.AddRole(relationType, this.Session.GetStrategy((long)previous));
            }

            if (current != null)
            {
                this.AddRole(relationType, current);
            }

            this.AddAssociation(relationType, association);
        }

        public void DiffComposite(Strategy association, RelationType relationType, long? current, long? previous)
        {
            if (Equals(current, previous))
            {
                return;
            }

            if (previous != null)
            {
                this.AddRole(relationType, this.Session.GetStrategy((long)previous));
            }

            if (current != null)
            {
                this.AddRole(relationType, this.Session.GetStrategy((long)current));
            }

            this.AddAssociation(relationType, association);
        }

        public void DiffComposite(Strategy association, RelationType relationType, Strategy current, Strategy previous)
        {
            if (Equals(current, previous))
            {
                return;
            }

            if (previous != null)
            {
                this.AddRole(relationType, previous);
            }

            if (current != null)
            {
                this.AddRole(relationType, current);
            }

            this.AddAssociation(relationType, association);
        }

        public void DiffComposites(Strategy association, RelationType relationType, RefRange<Strategy> current, ValueRange<long> previousRange)
        {
            var previous = RefRange<Strategy>.Load(previousRange.Select(v => this.Session.GetStrategy(v)));
            this.DiffComposites(association, relationType, current, previous);
        }

        public void DiffComposites(Strategy association, RelationType relationType, ValueRange<long> currentRange, ValueRange<long> previous)
        {
            var current = RefRange<Strategy>.Load(currentRange.Select(v => this.Session.GetStrategy(v)));
            this.DiffComposites(association, relationType, current, previous);
        }

        public void DiffComposites(Strategy association, RelationType relationType, RefRange<Strategy> current, RefRange<Strategy> previous)
        {
            var hasChange = false;

            foreach (var v in current.Except(previous))
            {
                this.AddRole(relationType, v);
                hasChange = true;
            }

            foreach (var v in previous.Except(current))
            {
                this.AddRole(relationType, v);
                hasChange = true;
            }

            if (hasChange)
            {
                this.AddAssociation(relationType, association);
            }
        }

        private void AddAssociation(RelationType relationType, Strategy association)
        {
            var roleType = relationType.RoleType;

            if (!this.AssociationsByRoleType.TryGetValue(roleType, out var associations))
            {
                associations = new HashSet<IStrategy>();
                this.AssociationsByRoleType.Add(roleType, associations);
            }

            associations.Add(association);
        }

        private void AddRole(RelationType relationType, Strategy role)
        {
            var associationType = relationType.AssociationType;

            if (!this.RolesByAssociationType.TryGetValue(associationType, out var roles))
            {
                roles = new HashSet<IStrategy>();
                this.RolesByAssociationType.Add(associationType, roles);
            }

            roles.Add(role);
        }
    }
}
