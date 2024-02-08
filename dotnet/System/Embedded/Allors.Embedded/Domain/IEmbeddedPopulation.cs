namespace Allors.Embedded
{
    using System;
    using System.Collections.Generic;
    using Meta;

    public delegate T Create<out T>(params Action<T>[] builders);

    public interface IEmbeddedPopulation
    {
        EmbeddedMeta EmbeddedMeta { get; }

        Dictionary<string, IEmbeddedDerivation> EmbeddedDerivationById { get; }

        IEnumerable<IEmbeddedObject> EmbeddedObjects { get; }

        IEmbeddedObject EmbeddedCreateObject(Type t, params Action<IEmbeddedObject>[] builders);

        T EmbeddedCreateObject<T>(params Action<T>[] builders)
            where T : IEmbeddedObject;

        void EmbeddedDerive();

        IEmbeddedUnitRole<T> EmbeddedGetUnitRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType);

        IEmbeddedCompositeRole<T> EmbeddedGetCompositeRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType)
            where T : IEmbeddedObject;

        IEmbeddedCompositesRole<T> EmbeddedGetCompositesRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType)
            where T : IEmbeddedObject;

        IEmbeddedCompositeAssociation<T> EmbeddedGetCompositeAssociation<T>(IEmbeddedObject obj, EmbeddedAssociationType associationType)
            where T : IEmbeddedObject;

        IEmbeddedCompositesAssociation<T> EmbeddedGetCompositesAssociation<T>(IEmbeddedObject obj, EmbeddedAssociationType associationType)
            where T : IEmbeddedObject;

        object EmbeddedGetRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType);

        void EmbeddedSetRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, object value);

        void EmbeddedAddRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, IEmbeddedObject role);

        void EmbeddedRemoveRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, IEmbeddedObject role);

        object EmbeddedGetAssociationValue(IEmbeddedObject obj, EmbeddedAssociationType associationType);
    }
}
