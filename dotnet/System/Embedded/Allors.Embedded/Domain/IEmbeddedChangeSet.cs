namespace Allors.Embedded
{
    using System.Collections.Generic;
    using Meta;

    public interface IEmbeddedChangeSet
    {
        EmbeddedMeta EmbeddedMeta { get; }

        bool HashEmbeddedChanges { get; }

        ISet<IEmbeddedObject> EmbeddedCreatedObjects { get; }

        IReadOnlyDictionary<IEmbeddedObject, object> EmbeddedChangedRoles<T>(string name);

        IReadOnlyDictionary<IEmbeddedObject, object> EmbeddedChangedRoles(EmbeddedObjectType objectType, string name);

        IReadOnlyDictionary<IEmbeddedObject, object> EmbeddedChangedRoles(EmbeddedRoleType roleType);
    }
}
