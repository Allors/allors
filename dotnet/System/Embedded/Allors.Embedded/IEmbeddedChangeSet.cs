namespace Allors.Embedded
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Meta;

    public interface IEmbeddedChangeSet
    {
        IEmbeddedMeta Meta { get; }

        bool HasChanges { get; }

        IReadOnlyDictionary<IEmbeddedObject, object> ChangedRoles<T>(string name);

        IReadOnlyDictionary<IEmbeddedObject, object> ChangedRoles(IEmbeddedObjectType objectType, string name);

        IReadOnlyDictionary<IEmbeddedObject, object> ChangedRoles(IEmbeddedRoleType roleType);
    }
}
