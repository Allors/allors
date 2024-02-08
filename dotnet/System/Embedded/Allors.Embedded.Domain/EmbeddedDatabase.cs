namespace Allors.Embedded
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    internal class EmbeddedDatabase
    {
        private readonly EmbeddedMeta meta;

        private readonly Dictionary<EmbeddedRoleType, Dictionary<IEmbeddedObject, object>> roleByAssociationByRoleType;
        private readonly Dictionary<EmbeddedAssociationType, Dictionary<IEmbeddedObject, object>> associationByRoleByAssociationType;

        private ISet<IEmbeddedObject> createdObjects;
        private Dictionary<EmbeddedRoleType, Dictionary<IEmbeddedObject, object>> changedRoleByAssociationByRoleType;
        private Dictionary<EmbeddedAssociationType, Dictionary<IEmbeddedObject, object>> changedAssociationByRoleByAssociationType;
        
        internal EmbeddedDatabase(EmbeddedMeta meta)
        {
            this.meta = meta;

            this.roleByAssociationByRoleType = new Dictionary<EmbeddedRoleType, Dictionary<IEmbeddedObject, object>>();
            this.associationByRoleByAssociationType = new Dictionary<EmbeddedAssociationType, Dictionary<IEmbeddedObject, object>>();

            this.createdObjects = new HashSet<IEmbeddedObject>();
            this.changedRoleByAssociationByRoleType = new Dictionary<EmbeddedRoleType, Dictionary<IEmbeddedObject, object>>();
            this.changedAssociationByRoleByAssociationType = new Dictionary<EmbeddedAssociationType, Dictionary<IEmbeddedObject, object>>();
        }

        internal IEmbeddedObject[] Objects { get; private set; }

        internal EmbeddedChangeSet Snapshot()
        {
            foreach (var roleType in this.changedRoleByAssociationByRoleType.Keys.ToArray())
            {
                var changedRoleByAssociation = this.changedRoleByAssociationByRoleType[roleType];
                var roleByAssociation = this.RoleByAssociation(roleType);

                foreach (var association in changedRoleByAssociation.Keys.ToArray())
                {
                    var role = changedRoleByAssociation[association];
                    roleByAssociation.TryGetValue(association, out var originalRole);

                    var areEqual = ReferenceEquals(originalRole, role) ||
                                   (roleType.IsOne && Equals(originalRole, role)) ||
                                   (roleType.IsMany && NullableArraySet.Same(originalRole, role));

                    if (areEqual)
                    {
                        changedRoleByAssociation.Remove(association);
                        continue;
                    }

                    roleByAssociation[association] = role;
                }

                if (roleByAssociation.Count == 0)
                {
                    this.changedRoleByAssociationByRoleType.Remove(roleType);
                }
            }

            foreach (var associationType in this.changedAssociationByRoleByAssociationType.Keys.ToArray())
            {
                var changedAssociationByRole = this.changedAssociationByRoleByAssociationType[associationType];
                var associationByRole = this.AssociationByRole(associationType);

                foreach (var role in changedAssociationByRole.Keys.ToArray())
                {
                    var changedAssociation = changedAssociationByRole[role];
                    associationByRole.TryGetValue(role, out var originalAssociation);

                    var areEqual = ReferenceEquals(originalAssociation, changedAssociation) ||
                                   (associationType.IsOne && Equals(originalAssociation, changedAssociation)) ||
                                   (associationType.IsMany && NullableArraySet.Same(originalAssociation, changedAssociation));

                    if (areEqual)
                    {
                        changedAssociationByRole.Remove(role);
                        continue;
                    }

                    associationByRole[role] = changedAssociation;
                }

                if (associationByRole.Count == 0)
                {
                    this.changedAssociationByRoleByAssociationType.Remove(associationType);
                }
            }

            var snapshot = new EmbeddedChangeSet(this.meta, this.createdObjects, this.changedRoleByAssociationByRoleType, this.changedAssociationByRoleByAssociationType);

            this.createdObjects = new HashSet<IEmbeddedObject>();

            foreach (var kvp in this.changedRoleByAssociationByRoleType)
            {
                var roleType = kvp.Key;
                var changedRoleByAssociation = kvp.Value;

                var roleByAssociation = this.RoleByAssociation(roleType);

                foreach (var kvp2 in changedRoleByAssociation)
                {
                    var association = kvp2.Key;
                    var changedRole = kvp2.Value;
                    roleByAssociation[association] = changedRole;
                }
            }

            foreach (var kvp in this.changedAssociationByRoleByAssociationType)
            {
                var associationType = kvp.Key;
                var changedAssociationByRole = kvp.Value;

                var associationByRole = this.AssociationByRole(associationType);

                foreach (var kvp2 in changedAssociationByRole)
                {
                    var role = kvp2.Key;
                    var changedAssociation = kvp2.Value;
                    associationByRole[role] = changedAssociation;
                }
            }

            this.changedRoleByAssociationByRoleType = new Dictionary<EmbeddedRoleType, Dictionary<IEmbeddedObject, object>>();
            this.changedAssociationByRoleByAssociationType = new Dictionary<EmbeddedAssociationType, Dictionary<IEmbeddedObject, object>>();

            return snapshot;
        }

        internal void AddObject(IEmbeddedObject newObject)
        {
            this.Objects = NullableArraySet.Add(this.Objects, newObject);
            this.createdObjects.Add(newObject);
        }

        internal void GetRoleValue(IEmbeddedObject association, EmbeddedRoleType roleType, out object? role)
        {
            if (this.changedRoleByAssociationByRoleType.TryGetValue(roleType, out var changedRoleByAssociation) &&
                changedRoleByAssociation.TryGetValue(association, out role))
            {
                return;
            }

            this.RoleByAssociation(roleType).TryGetValue(association, out role);
        }

        internal void SetRoleValue(IEmbeddedObject association, EmbeddedRoleType roleType, object? role)
        {
            if (role == null)
            {
                this.RemoveRoleValue(association, roleType);
                return;
            }

            var normalizedRole = roleType.Normalize(role);

            if (roleType.IsUnit)
            {
                // Role
                this.ChangedRoleByAssociation(roleType)[association] = normalizedRole;
            }
            else
            {
                var associationType = roleType.AssociationType;
                this.GetRoleValue(association, roleType, out object previousRole);
                if (roleType.IsOne)
                {
                    var roleObject = (IEmbeddedObject)normalizedRole;
                    this.GetAssociationValue(roleObject, associationType, out var previousAssociation);

                    // Role
                    var changedRoleByAssociation = this.ChangedRoleByAssociation(roleType);
                    changedRoleByAssociation[association] = roleObject;

                    // Association
                    var changedAssociationByRole = this.ChangedAssociationByRole(associationType);
                    if (associationType.IsOne)
                    {
                        // One to One
                        var previousAssociationObject = (IEmbeddedObject)previousAssociation;
                        if (previousAssociationObject != null)
                        {
                            changedRoleByAssociation[previousAssociationObject] = null;
                        }

                        if (previousRole != null)
                        {
                            var previousRoleObject = (IEmbeddedObject)previousRole;
                            changedAssociationByRole[previousRoleObject] = null;
                        }

                        changedAssociationByRole[roleObject] = association;
                    }
                    else
                    {
                        changedAssociationByRole[roleObject] = NullableArraySet.Remove(previousAssociation, roleObject);
                    }
                }
                else
                {
                    var roles = ((IEnumerable)normalizedRole)?.Cast<IEmbeddedObject>().ToArray() ?? Array.Empty<IEmbeddedObject>();
                    var previousRoles = (IEmbeddedObject[])previousRole ?? Array.Empty<IEmbeddedObject>();

                    // Use Diff (Add/Remove)
                    var addedRoles = roles.Except(previousRoles);
                    var removedRoles = previousRoles.Except(roles);

                    foreach (var addedRole in addedRoles)
                    {
                        this.AddRoleValue(association, roleType, addedRole);
                    }

                    foreach (var removeRole in removedRoles)
                    {
                        this.RemoveRoleValue(association, roleType, removeRole);
                    }
                }
            }
        }

        internal void AddRoleValue(IEmbeddedObject association, EmbeddedRoleType roleType, IEmbeddedObject role)
        {
            var associationType = roleType.AssociationType;
            this.GetAssociationValue(role, associationType, out var previousAssociation);

            // Role
            var changedRoleByAssociation = this.ChangedRoleByAssociation(roleType);
            this.GetRoleValue(association, roleType, out var previousRole);
            var roleArray = (IEmbeddedObject[])previousRole;
            roleArray = NullableArraySet.Add(roleArray, role);
            changedRoleByAssociation[association] = roleArray;

            // Association
            var changedAssociationByRole = this.ChangedAssociationByRole(associationType);
            if (associationType.IsOne)
            {
                // One to Many
                var previousAssociationObject = (IEmbeddedObject)previousAssociation;
                if (previousAssociationObject != null)
                {
                    this.GetRoleValue(previousAssociationObject, roleType, out var previousAssociationRole);
                    changedRoleByAssociation[previousAssociationObject] = NullableArraySet.Remove(previousAssociationRole, role);
                }

                changedAssociationByRole[role] = association;
            }
            else
            {
                // Many to Many
                changedAssociationByRole[role] = NullableArraySet.Add(previousAssociation, association);
            }
        }

        internal void RemoveRoleValue(IEmbeddedObject association, EmbeddedRoleType roleType, IEmbeddedObject role)
        {
            var associationType = roleType.AssociationType;
            this.GetAssociationValue(role, associationType, out var previousAssociation);

            this.GetRoleValue(association, roleType, out var previousRole);
            if (previousRole != null)
            {
                // Role
                var changedRoleByAssociation = this.ChangedRoleByAssociation(roleType);
                changedRoleByAssociation[association] = NullableArraySet.Remove(previousRole, role);

                // Association
                var changedAssociationByRole = this.ChangedAssociationByRole(associationType);
                if (associationType.IsOne)
                {
                    // One to Many
                    changedAssociationByRole[role] = null;
                }
                else
                {
                    // Many to Many
                    changedAssociationByRole[role] = NullableArraySet.Remove(previousAssociation, association);
                }
            }
        }

        internal void RemoveRoleValue(IEmbeddedObject association, EmbeddedRoleType roleType)
        {
            if (roleType.IsUnit || roleType.IsOne)
            {
                // Role
                this.ChangedRoleByAssociation(roleType)[association] = null;
            }
            else
            {
                var associationType = roleType.AssociationType;
                this.GetRoleValue(association, roleType, out object previousRole);

                var previousRoles = (IEmbeddedObject[])previousRole;

                if (previousRoles != null)
                {
                    foreach (var removeRole in previousRoles)
                    {
                        this.RemoveRoleValue(association, roleType, removeRole);
                    }
                }
            }
        }

        internal void GetAssociationValue(IEmbeddedObject role, EmbeddedAssociationType associationType, out object association)
        {
            if (this.changedAssociationByRoleByAssociationType.TryGetValue(associationType, out var changedAssociationByRole) &&
                changedAssociationByRole.TryGetValue(role, out association))
            {
                return;
            }

            this.AssociationByRole(associationType).TryGetValue(role, out association);
        }

        private Dictionary<IEmbeddedObject, object> AssociationByRole(EmbeddedAssociationType asscociationType)
        {
            if (!this.associationByRoleByAssociationType.TryGetValue(asscociationType, out var associationByRole))
            {
                associationByRole = new Dictionary<IEmbeddedObject, object>();
                this.associationByRoleByAssociationType[asscociationType] = associationByRole;
            }

            return associationByRole;
        }

        private Dictionary<IEmbeddedObject, object> RoleByAssociation(EmbeddedRoleType roleType)
        {
            if (!this.roleByAssociationByRoleType.TryGetValue(roleType, out var roleByAssociation))
            {
                roleByAssociation = new Dictionary<IEmbeddedObject, object>();
                this.roleByAssociationByRoleType[roleType] = roleByAssociation;
            }

            return roleByAssociation;
        }

        private Dictionary<IEmbeddedObject, object> ChangedAssociationByRole(EmbeddedAssociationType associationType)
        {
            if (!this.changedAssociationByRoleByAssociationType.TryGetValue(associationType, out var changedAssociationByRole))
            {
                changedAssociationByRole = new Dictionary<IEmbeddedObject, object>();
                this.changedAssociationByRoleByAssociationType[associationType] = changedAssociationByRole;
            }

            return changedAssociationByRole;
        }

        private Dictionary<IEmbeddedObject, object> ChangedRoleByAssociation(EmbeddedRoleType roleType)
        {
            if (!this.changedRoleByAssociationByRoleType.TryGetValue(roleType, out var changedRoleByAssociation))
            {
                changedRoleByAssociation = new Dictionary<IEmbeddedObject, object>();
                this.changedRoleByAssociationByRoleType[roleType] = changedRoleByAssociation;
            }

            return changedRoleByAssociation;
        }
    }
}
