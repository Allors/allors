namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;

    public class EmbeddedMeta
    {
        public EmbeddedMeta(IPluralizer pluralizer)
        {
            this.Pluralizer = pluralizer;
            this.ObjectTypeByType = new Dictionary<Type, EmbeddedObjectType>();
        }

        public IPluralizer Pluralizer { get; }

        public IDictionary<Type, EmbeddedObjectType> ObjectTypeByType { get; }

        public EmbeddedUnitRoleType AddUnit<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddUnit(this.GetOrAddObjectType(typeof(TRole)), roleName);

        public EmbeddedOneToOneRoleType AddOneToOne<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddOneToOne(this.GetOrAddObjectType(typeof(TRole)), roleName);

        public EmbeddedManyToOneRoleType AddManyToOne<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddManyToOne(this.GetOrAddObjectType(typeof(TRole)), roleName);

        public EmbeddedOneToManyRoleType AddOneToMany<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddOneToMany(this.GetOrAddObjectType(typeof(TRole)), roleName);

        public EmbeddedManyToManyRoleType AddManyToMany<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddManyToMany(this.GetOrAddObjectType(typeof(TRole)), roleName);

        internal EmbeddedObjectType GetOrAddObjectType(Type type)
        {
            if (!this.ObjectTypeByType.TryGetValue(type, out var objectType))
            {
                objectType = new EmbeddedObjectType(this, type);
                this.ObjectTypeByType.Add(type, objectType);
            }

            return objectType;
        }

        internal void ResetDerivations()
        {
            foreach (var kvp in this.ObjectTypeByType)
            {
                var objectType = kvp.Value;
                objectType.ResetDerivations();
            }
        }
    }
}