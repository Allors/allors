namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class EmbeddedMeta
    {
        private readonly IDictionary<Type, EmbeddedObjectType> embeddedObjectTypeByType;

        public EmbeddedMeta()
        {
            this.embeddedObjectTypeByType = new Dictionary<Type, EmbeddedObjectType>();
        }

        public IReadOnlyDictionary<Type, EmbeddedObjectType> EmbeddedObjectTypeByType => new ReadOnlyDictionary<Type, EmbeddedObjectType>(this.embeddedObjectTypeByType);

        public EmbeddedRoleType AddUnit<TAssociation, TRole>(string roleName, string? associationName = null) => this.GetOrAddEmbeddedObjectType(typeof(TAssociation)).AddUnit(this.GetOrAddEmbeddedObjectType(typeof(TRole)), roleName, associationName);

        public EmbeddedRoleType AddOneToOne<TAssociation, TRole>(string roleName, string? associationName = null) => this.GetOrAddEmbeddedObjectType(typeof(TAssociation)).AddOneToOne(this.GetOrAddEmbeddedObjectType(typeof(TRole)), roleName, associationName);

        public EmbeddedRoleType AddManyToOne<TAssociation, TRole>(string roleName, string? associationName = null) => this.GetOrAddEmbeddedObjectType(typeof(TAssociation)).AddManyToOne(this.GetOrAddEmbeddedObjectType(typeof(TRole)), roleName, associationName);

        public EmbeddedRoleType AddOneToMany<TAssociation, TRole>(string roleName, string? associationName = null) => this.GetOrAddEmbeddedObjectType(typeof(TAssociation)).AddOneToMany(this.GetOrAddEmbeddedObjectType(typeof(TRole)), roleName, associationName);

        public EmbeddedRoleType AddManyToMany<TAssociation, TRole>(string roleName, string? associationName = null) => this.GetOrAddEmbeddedObjectType(typeof(TAssociation)).AddManyToMany(this.GetOrAddEmbeddedObjectType(typeof(TRole)), roleName, associationName);

        public EmbeddedObjectType GetOrAddEmbeddedObjectType(Type type)
        {
            if (!this.EmbeddedObjectTypeByType.TryGetValue(type, out var objectType))
            {
                objectType = new EmbeddedObjectType(this, type);
                this.embeddedObjectTypeByType.Add(type, objectType);
            }

            return objectType;
        }

        internal string Pluralize(string singular)
        {
            static bool EndsWith(string word, string ending) => word.EndsWith(ending, StringComparison.InvariantCultureIgnoreCase);

            if (EndsWith(singular, "y") &&
                !EndsWith(singular, "ay") &&
                !EndsWith(singular, "ey") &&
                !EndsWith(singular, "iy") &&
                !EndsWith(singular, "oy") &&
                !EndsWith(singular, "uy"))
            {
                return singular.Substring(0, singular.Length - 1) + "ies";
            }

            if (EndsWith(singular, "us"))
            {
                return singular + "es";
            }

            if (EndsWith(singular, "ss"))
            {
                return singular + "es";
            }

            if (EndsWith(singular, "x") ||
                EndsWith(singular, "ch") ||
                EndsWith(singular, "sh"))
            {
                return singular + "es";
            }

            if (EndsWith(singular, "f") && singular.Length > 1)
            {
                return singular.Substring(0, singular.Length - 1) + "ves";
            }

            if (EndsWith(singular, "fe") && singular.Length > 2)
            {
                return singular.Substring(0, singular.Length - 2) + "ves";
            }

            return singular + "s";
        }

        internal void ResetDerivations()
        {
            foreach ((_, EmbeddedObjectType? objectType) in this.EmbeddedObjectTypeByType)
            {
                objectType.ResetDerivations();
            }
        }
    }
}
