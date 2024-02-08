namespace Allors.Embedded
{
    using System;
    using System.Collections.Generic;
    using Meta;

    public class EmbeddedPopulation : IEmbeddedPopulation
    {
        private readonly EmbeddedDatabase database;

        public EmbeddedPopulation()
        {
            this.Meta = new EmbeddedMeta();
            this.DerivationById = new Dictionary<string, IEmbeddedDerivation>();
            this.database = new EmbeddedDatabase(this.Meta);
        }

        EmbeddedMeta IEmbeddedPopulation.Meta => this.Meta;

        public EmbeddedMeta Meta { get; }

        public Dictionary<string, IEmbeddedDerivation> DerivationById { get; }

        public IEnumerable<IEmbeddedObject> Objects => this.database.Objects;

        public IEmbeddedObject Create(Type type, params Action<IEmbeddedObject>[] builders)
        {
            var @new = (IEmbeddedObject)Activator.CreateInstance(type, new object[] { this, this.Meta.GetOrAddObjectType(type) });
            this.database.AddObject(@new);

            foreach (var builder in builders)
            {
                builder(@new);
            }

            return @new;
        }

        public T Create<T>(params Action<T>[] builders)
              where T : IEmbeddedObject
        {
            var @new = (T)Activator.CreateInstance(typeof(T), new object[] { this, this.Meta.GetOrAddObjectType(typeof(T)) });
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

        public void Derive()
        {
            var changeSet = this.Snapshot();

            while (changeSet.HasChanges)
            {
                foreach (var kvp in this.DerivationById)
                {
                    var derivation = kvp.Value;
                    derivation.Derive(changeSet);
                }

                changeSet = this.Snapshot();
            }
        }

        public IUnitRole<T> GetUnitRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType)
        {
            return new UnitRole<T>(obj, roleType);
        }

        public ICompositeRole<T> GetCompositeRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType) where T : IEmbeddedObject
        {
            return new CompositeRole<T>(obj, roleType);
        }

        public ICompositesRole<T> GetCompositesRole<T>(IEmbeddedObject obj, EmbeddedRoleType roleType) where T : IEmbeddedObject
        {
            return new CompositesRole<T>(obj, roleType);
        }

        public ICompositeAssociation<T> GetCompositeAssociation<T>(IEmbeddedObject obj, EmbeddedAssociationType associationType) where T : IEmbeddedObject
        {
            return new CompositeAssociation<T>(obj, associationType);
        }
        
        public ICompositesAssociation<T> GetCompositesAssociation<T>(IEmbeddedObject obj, EmbeddedAssociationType associationType) where T : IEmbeddedObject
        {
            return new CompositesAssociation<T>(obj, associationType);
        }

        public object GetRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType)
        {
            this.database.GetRoleValue(obj, roleType, out var result);
            return result;
        }

        public void SetRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, object value)
        {
            this.database.SetRoleValue(obj, roleType, value);
        }

        public void AddRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, IEmbeddedObject role)
        {
            this.database.AddRoleValue(obj, roleType, (IEmbeddedObject)role);
        }

        public void RemoveRoleValue(IEmbeddedObject obj, EmbeddedRoleType roleType, IEmbeddedObject role)
        {
            this.database.RemoveRoleValue(obj, roleType, (IEmbeddedObject)role);
        }

        public object GetAssociationValue(IEmbeddedObject obj, EmbeddedAssociationType associationType)
        {
            this.database.GetAssociationValue(obj, associationType, out var result);
            return result;
        }
    }
}
