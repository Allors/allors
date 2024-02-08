namespace Allors.Embedded
{
    using System.Collections.Generic;
    using Meta;

    public interface IEmbeddedChangeSet
    {
        EmbeddedMeta Meta { get; }

        bool HasChanges { get; }

        ISet<IEmbeddedObject> CreatedObjects { get; }

        IReadOnlyDictionary<IEmbeddedObject, object> ChangedRoles<T>(string name);

        IReadOnlyDictionary<IEmbeddedObject, object> ChangedRoles(EmbeddedObjectType objectType, string name);

        IReadOnlyDictionary<IEmbeddedObject, object> ChangedRoles(EmbeddedRoleType roleType);
    }
}
