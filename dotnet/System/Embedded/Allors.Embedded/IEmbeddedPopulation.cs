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

        IUnitRole<T> GetUnitRole<T>(IEmbeddedObject obj, IEmbeddedRoleType roleType);

        ICompositeRole<T> GetCompositeRole<T>(IEmbeddedObject obj, IEmbeddedRoleType roleType)
            where T : IEmbeddedObject;

        ICompositesRole<T> GetCompositesRole<T>(IEmbeddedObject obj, IEmbeddedRoleType roleType)
            where T : IEmbeddedObject;

        ICompositeAssociation<T> GetCompositeAssociation<T>(IEmbeddedObject obj, IEmbeddedAssociationType associationType) 
            where T : IEmbeddedObject;

        ICompositesAssociation<T> GetCompositesAssociation<T>(IEmbeddedObject obj, IEmbeddedAssociationType associationType)
            where T : IEmbeddedObject;

        object GetRoleValue(IEmbeddedObject obj, IEmbeddedRoleType roleType);

        void SetRoleValue(IEmbeddedObject obj, IEmbeddedRoleType roleType, object value);

        void AddRoleValue(IEmbeddedObject obj, IEmbeddedRoleType roleType, IEmbeddedObject role);

        void RemoveRoleValue(IEmbeddedObject obj, IEmbeddedRoleType roleType, IEmbeddedObject role);

        object GetAssociationValue(IEmbeddedObject obj, IEmbeddedAssociationType associationType);
    }
}
