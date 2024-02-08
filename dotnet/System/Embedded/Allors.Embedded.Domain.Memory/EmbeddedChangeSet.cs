namespace Allors.Embedded.Domain.Memory
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Embedded.Meta;

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
            this.EmbeddedMeta = meta;
            this.EmbeddedCreatedObjects = createdObjects;
            this.roleByAssociationByRoleType = roleByAssociationByRoleType;
            this.associationByRoleByRoleType = associationByRoleByAssociationType;
        }

        public EmbeddedMeta EmbeddedMeta { get; }

        public bool HashEmbeddedChanges =>
            this.EmbeddedCreatedObjects.Any() ||
            this.roleByAssociationByRoleType.Any(v => v.Value.Count > 0) ||
            this.associationByRoleByRoleType.Any(v => v.Value.Count > 0);

        public ISet<IEmbeddedObject> EmbeddedCreatedObjects { get; }

        public IReadOnlyDictionary<IEmbeddedObject, object> EmbeddedChangedRoles<T>(string name)
        {
            var objectType = this.EmbeddedMeta.EmbeddedObjectTypeByType[typeof(T)];
            var roleType = objectType.EmbeddedRoleTypeByName[name];
            return this.EmbeddedChangedRoles(roleType) ?? Empty;
        }

        public IReadOnlyDictionary<IEmbeddedObject, object> EmbeddedChangedRoles(EmbeddedObjectType objectType, string name)
        {
            var roleType = objectType.EmbeddedRoleTypeByName[name];
            return this.EmbeddedChangedRoles(roleType) ?? Empty;
        }

        public IReadOnlyDictionary<IEmbeddedObject, object> EmbeddedChangedRoles(EmbeddedRoleType roleType)
        {
            this.roleByAssociationByRoleType.TryGetValue(roleType, out var changedRelations);
            return changedRelations ?? Empty;
        }
    }
}
