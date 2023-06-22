namespace Allors.Embedded
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Meta;

    public class EmbeddedChangeSet
    {
        private static readonly IReadOnlyDictionary<EmbeddedObject, object> Empty = new ReadOnlyDictionary<EmbeddedObject, object>(new Dictionary<EmbeddedObject, object>());

        private readonly IReadOnlyDictionary<IEmbeddedRoleType, Dictionary<EmbeddedObject, object>> roleByAssociationByRoleType;
        private readonly IReadOnlyDictionary<IEmbeddedAssociationType, Dictionary<EmbeddedObject, object>> associationByRoleByRoleType;

        public EmbeddedChangeSet(EmbeddedMeta meta, IReadOnlyDictionary<IEmbeddedRoleType, Dictionary<EmbeddedObject, object>> roleByAssociationByRoleType, IReadOnlyDictionary<IEmbeddedAssociationType, Dictionary<EmbeddedObject, object>> associationByRoleByAssociationType)
        {
            this.Meta = meta;
            this.roleByAssociationByRoleType = roleByAssociationByRoleType;
            this.associationByRoleByRoleType = associationByRoleByAssociationType;
        }

        public EmbeddedMeta Meta { get; }

        public bool HasChanges =>
            this.roleByAssociationByRoleType.Any(v => v.Value.Count > 0) ||
            this.associationByRoleByRoleType.Any(v => v.Value.Count > 0);

        public IReadOnlyDictionary<EmbeddedObject, object> ChangedRoles<TRole>(string name)
        {
            var objectType = this.Meta.ObjectTypeByType[typeof(TRole)];
            var roleType = objectType.RoleTypeByName[name];
            return this.ChangedRoles(roleType) ?? Empty;
        }

        public IReadOnlyDictionary<EmbeddedObject, object> ChangedRoles(EmbeddedObjectType objectType, string name)
        {
            var roleType = objectType.RoleTypeByName[name];
            return this.ChangedRoles(roleType) ?? Empty;
        }

        public IReadOnlyDictionary<EmbeddedObject, object> ChangedRoles(IEmbeddedRoleType roleType)
        {
            this.roleByAssociationByRoleType.TryGetValue(roleType, out var changedRelations);
            return changedRelations ?? Empty;
        }
    }
}
