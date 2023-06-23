namespace Allors.Embedded
{
    using System;
    using System.Collections.Generic;
    using Meta;

    public delegate T New<out T>(params Action<T>[] builders);

    public class EmbeddedPopulation
    {
        private readonly EmbeddedDatabase database;

        public EmbeddedPopulation(params Action<EmbeddedMeta>[] builders)
        {
            this.Meta = new EmbeddedMeta();
            this.DerivationById = new Dictionary<string, IEmbeddedDerivation>();
            this.database = new EmbeddedDatabase(this.Meta);

            foreach (var builder in builders)
            {
                builder?.Invoke(this.Meta);
            }
        }

        public EmbeddedMeta Meta { get; }

        public Dictionary<string, IEmbeddedDerivation> DerivationById { get; }

        public IEnumerable<IEmbeddedObject> Objects => this.database.Objects;

        public EmbeddedObject New(Type t, params Action<EmbeddedObject>[] builders)
        {
            var @new = (EmbeddedObject)Activator.CreateInstance(t, new object[] { this, this.Meta.GetOrAddObjectType(t) });
            this.database.AddObject(@new);

            foreach (var builder in builders)
            {
                builder(@new);
            }

            return @new;
        }

        public T New<T>(params Action<T>[] builders)
              where T : EmbeddedObject
        {
            var @new = (T)Activator.CreateInstance(typeof(T), new object[] { this, this.Meta.GetOrAddObjectType(typeof(T)) });
            this.database.AddObject(@new);

            foreach (var builder in builders)
            {
                builder(@new);
            }

            return @new;
        }

        public EmbeddedChangeSet Snapshot()
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

        public UnitRole<T> GetUnitRole<T>(IEmbeddedObject obj, IEmbeddedRoleType roleType)
        {
            return new UnitRole<T>(obj, roleType);
        }

        public CompositeRole<T> GetCompositeRole<T>(IEmbeddedObject obj, IEmbeddedRoleType roleType) where T : IEmbeddedObject
        {
            return new CompositeRole<T>(obj, roleType);
        }

        public CompositesRole<T> GetCompositesRole<T>(IEmbeddedObject obj, IEmbeddedRoleType roleType) where T : IEmbeddedObject
        {
            return new CompositesRole<T>(obj, roleType);
        }

        public CompositeAssociation<T> GetCompositeAssociation<T>(IEmbeddedObject obj, IEmbeddedAssociationType associationType) where T : IEmbeddedObject
        {
            return new CompositeAssociation<T>(obj, associationType);
        }
        
        public CompositesAssociation<T> GetCompositesAssociation<T>(IEmbeddedObject obj, IEmbeddedAssociationType associationType) where T : IEmbeddedObject
        {
            return new CompositesAssociation<T>(obj, associationType);
        }

        public object GetRoleValue(IEmbeddedObject obj, IEmbeddedRoleType roleType)
        {
            this.database.GetRoleValue(obj, roleType, out var result);
            return result;
        }

        public void SetRoleValue(IEmbeddedObject obj, IEmbeddedRoleType roleType, object value)
        {
            this.database.SetRoleValue(obj, roleType, value);
        }

        public void AddRoleValue(IEmbeddedObject obj, IEmbeddedRoleType roleType, IEmbeddedObject role)
        {
            this.database.AddRoleValue(obj, roleType, (IEmbeddedObject)role);
        }

        public void RemoveRoleValue(IEmbeddedObject obj, IEmbeddedRoleType roleType, IEmbeddedObject role)
        {
            this.database.RemoveRoleValue(obj, roleType, (IEmbeddedObject)role);
        }

        public object GetAssociationValue(IEmbeddedObject obj, IEmbeddedAssociationType associationType)
        {
            this.database.GetAssociationValue(obj, associationType, out var result);
            return result;
        }
    }
}
