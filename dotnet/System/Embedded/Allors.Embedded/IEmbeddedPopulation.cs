namespace Allors.Embedded
{
    using System;
    using System.Collections.Generic;
    using Meta;

    public delegate T New<out T>(params Action<T>[] builders);

    public interface IEmbeddedPopulation
    {
        EmbeddedMeta Meta { get; }

        Dictionary<string, IEmbeddedDerivation> DerivationById { get; }

        IEnumerable<IEmbeddedObject> Objects { get; }

        IEmbeddedObject New(Type t, params Action<IEmbeddedObject>[] builders);

        T New<T>(params Action<T>[] builders)
            where T : IEmbeddedObject;

        IEmbeddedChangeSet Snapshot();

        void Derive();

        IUnitRole<T> GetUnitRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType);

        ICompositeRole<T> GetCompositeRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType)
            where T : IEmbeddedObject;

        ICompositesRole<T> GetCompositesRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType)
            where T : IEmbeddedObject;

        ICompositeAssociation<T> GetCompositeAssociation<T>(IEmbeddedObject obj, EmbeddedAssociationType associationType) 
            where T : IEmbeddedObject;

        ICompositesAssociation<T> GetCompositesAssociation<T>(IEmbeddedObject obj, EmbeddedAssociationType associationType)
            where T : IEmbeddedObject;

        object GetRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType);

        void SetRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, object value);

        void AddRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, IEmbeddedObject role);

        void RemoveRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, IEmbeddedObject role);

        object GetAssociationValue(IEmbeddedObject obj, EmbeddedAssociationType associationType);
    }
}
