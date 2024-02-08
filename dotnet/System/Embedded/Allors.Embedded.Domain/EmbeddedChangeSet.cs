﻿namespace Allors.Embedded
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Meta;

    public class EmbeddedChangeSet : IEmbeddedChangeSet
    {
        private static readonly IReadOnlyDictionary<IEmbeddedObject, object> Empty = new ReadOnlyDictionary<IEmbeddedObject, object>(new Dictionary<IEmbeddedObject, object>());

        private readonly IReadOnlyDictionary<EmbeddedRoleType, Dictionary<IEmbeddedObject, object>> roleByAssociationByRoleType;
        private readonly IReadOnlyDictionary<EmbeddedAssociationType, Dictionary<IEmbeddedObject, object>> associationByRoleByRoleType;

        public EmbeddedChangeSet(EmbeddedMeta meta, 
            ISet<IEmbeddedObject> createdObjects,
            IReadOnlyDictionary<EmbeddedRoleType, Dictionary<IEmbeddedObject, object>> roleByAssociationByRoleType,
            IReadOnlyDictionary<EmbeddedAssociationType, Dictionary<IEmbeddedObject, object>> associationByRoleByAssociationType)
        {
            this.Meta = meta;
            this.CreatedObjects = createdObjects;
            this.roleByAssociationByRoleType = roleByAssociationByRoleType;
            this.associationByRoleByRoleType = associationByRoleByAssociationType;
        }

        public EmbeddedMeta Meta { get; }

        public bool HasChanges =>
            this.CreatedObjects.Any() ||
            this.roleByAssociationByRoleType.Any(v => v.Value.Count > 0) ||
            this.associationByRoleByRoleType.Any(v => v.Value.Count > 0);

        public ISet<IEmbeddedObject> CreatedObjects { get; }

        public IReadOnlyDictionary<IEmbeddedObject, object> ChangedRoles<T>(string name)
        {
            var objectType = this.Meta.ObjectTypeByType[typeof(T)];
            var roleType = objectType.RoleTypeByName[name];
            return this.ChangedRoles(roleType) ?? Empty;
        }

        public IReadOnlyDictionary<IEmbeddedObject, object> ChangedRoles(EmbeddedObjectType objectType, string name)
        {
            var roleType = objectType.RoleTypeByName[name];
            return this.ChangedRoles(roleType) ?? Empty;
        }

        public IReadOnlyDictionary<IEmbeddedObject, object> ChangedRoles(EmbeddedRoleType roleType)
        {
            this.roleByAssociationByRoleType.TryGetValue(roleType, out var changedRelations);
            return changedRelations ?? Empty;
        }
    }
}
