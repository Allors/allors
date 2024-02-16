namespace Allors.Embedded.Domain.Memory
{
    using System;
    using System.Collections.Generic;
    using Embedded.Meta;

    public class EmbeddedPopulation : IEmbeddedPopulation
    {
        private readonly EmbeddedDatabase database;

        public EmbeddedPopulation()
        {
            this.EmbeddedMeta = new EmbeddedMeta();
            this.EmbeddedDerivationById = new Dictionary<string, IEmbeddedDerivation>();
            this.database = new EmbeddedDatabase(this.EmbeddedMeta);
        }

        EmbeddedMeta IEmbeddedPopulation.EmbeddedMeta => this.EmbeddedMeta;

        public EmbeddedMeta EmbeddedMeta { get; }

        public Dictionary<string, IEmbeddedDerivation> EmbeddedDerivationById { get; }

        public IEnumerable<IEmbeddedObject> EmbeddedObjects => this.database.Objects;

        public IEmbeddedObject EmbeddedCreateObject(Type type, params Action<IEmbeddedObject>[] builders)
        {
            var created = (IEmbeddedObject)Activator.CreateInstance(type, [this, this.EmbeddedMeta.GetOrAddEmbeddedObjectType(type)]);
            this.database.AddObject(created);

            foreach (var builder in builders)
            {
                builder(created);
            }

            return created;
        }

        public T EmbeddedCreateObject<T>(params Action<T>[] builders)
              where T : IEmbeddedObject
        {
            var @new = (T)Activator.CreateInstance(typeof(T), [this, this.EmbeddedMeta.GetOrAddEmbeddedObjectType(typeof(T))]);
            this.database.AddObject(@new);

            foreach (var builder in builders)
            {
                builder(@new);
            }

            return @new;
        }

        public IEmbeddedChangeSet Snapshot()
        {
            return this.database.Snapshot();
        }

        public void EmbeddedDerive()
        {
            var changeSet = this.Snapshot();

            while (changeSet.HashEmbeddedChanges)
            {
                foreach (var kvp in this.EmbeddedDerivationById)
                {
                    var derivation = kvp.Value;
                    derivation.EmbeddedDerive(changeSet);
                }

                changeSet = this.Snapshot();
            }
        }

        public IEmbeddedUnitRole<T> EmbeddedGetUnitRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType)
        {
            return new EmbeddedUnitRole<T>(obj, roleType);
        }

        public IEmbeddedCompositeRole<T> EmbeddedGetCompositeRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType) where T : IEmbeddedObject
        {
            return new EmbeddedCompositeRole<T>(obj, roleType);
        }

        public IEmbeddedCompositesRole<T> EmbeddedGetCompositesRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType) where T : IEmbeddedObject
        {
            return new EmbeddedCompositesRole<T>(obj, roleType);
        }

        public IEmbeddedCompositeAssociation<T> EmbeddedGetCompositeAssociation<T>(IEmbeddedObject obj, EmbeddedAssociationType associationType) where T : IEmbeddedObject
        {
            return new EmbeddedCompositeAssociation<T>(obj, associationType);
        }
        
        public IEmbeddedCompositesAssociation<T> EmbeddedGetCompositesAssociation<T>(IEmbeddedObject obj, EmbeddedAssociationType associationType) where T : IEmbeddedObject
        {
            return new EmbeddedCompositesAssociation<T>(obj, associationType);
        }

        public object EmbeddedGetRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType)
        {
            this.database.GetRoleValue(obj, roleType, out var result);
            return result;
        }

        public void EmbeddedSetRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, object value)
        {
            this.database.SetRoleValue(obj, roleType, value);
        }

        public void EmbeddedAddRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, IEmbeddedObject role)
        {
            this.database.AddRoleValue(obj, roleType, (IEmbeddedObject)role);
        }

        public void EmbeddedRemoveRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, IEmbeddedObject role)
        {
            this.database.RemoveRoleValue(obj, roleType, (IEmbeddedObject)role);
        }

        public object EmbeddedGetAssociationValue(IEmbeddedObject obj, EmbeddedAssociationType associationType)
        {
            this.database.GetAssociationValue(obj, associationType, out var result);
            return result;
        }
    }
}
