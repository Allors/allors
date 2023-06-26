namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;

    public class EmbeddedMeta 
    {
        public EmbeddedMeta()
        {
            this.ObjectTypeByType = new Dictionary<Type, EmbeddedObjectType>();
        }

        public IDictionary<Type, EmbeddedObjectType> ObjectTypeByType { get; }

        public string Pluralize(string singular)
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

        public EmbeddedRoleType AddUnit<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddUnit(this.GetOrAddObjectType(typeof(TRole)), roleName);

        public EmbeddedRoleType AddOneToOne<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddOneToOne(this.GetOrAddObjectType(typeof(TRole)), roleName);

        public EmbeddedRoleType AddManyToOne<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddManyToOne(this.GetOrAddObjectType(typeof(TRole)), roleName);

        public EmbeddedRoleType AddOneToMany<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddOneToMany(this.GetOrAddObjectType(typeof(TRole)), roleName);

        public EmbeddedRoleType AddManyToMany<TAssociation, TRole>(string roleName) => this.GetOrAddObjectType(typeof(TAssociation)).AddManyToMany(this.GetOrAddObjectType(typeof(TRole)), roleName);

        public EmbeddedObjectType GetOrAddObjectType(Type type)
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
