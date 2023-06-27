namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class EmbeddedRoleType
    {
        public EmbeddedAssociationType AssociationType { get; internal set; } = null!;

        public EmbeddedObjectType ObjectType { get; }

        public string SingularName { get; }

        public string PluralName { get; }

        public string Name { get; }

        public bool IsOne { get; }

        public bool IsMany { get;}

        public bool IsUnit { get;  }

        internal EmbeddedRoleType(EmbeddedObjectType objectType, string singularName, string pluralName, string name, bool isOne, bool isMany, bool isUnit)
        {
            this.ObjectType = objectType;
            this.SingularName = singularName;
            this.PluralName = pluralName;
            this.Name = name;
            this.IsOne = isOne;
            this.IsMany = isMany;
            this.IsUnit = isUnit;
        }

        public override string ToString()
        {
            return this.Name;
        }

        internal string SingularNameForAssociation(EmbeddedObjectType objectType)
        {
            return $"{objectType.Type.Name}Where{this.SingularName}";
        }

        internal string PluralNameForAssociation(EmbeddedObjectType objectType)
        {
            return $"{this.ObjectType.Meta.Pluralize(objectType.Type.Name)}Where{this.SingularName}";
        }

        public object? Normalize(object? value) =>
            this.IsOne switch
            {
                true => this.NormalizeToOne(value),
                _ => this.NormalizeToMany(value)
            };

        private object? NormalizeToOne(object? value)
        {
            if (value != null)
            {
                var type = this.ObjectType.Type;
                if (!type.IsInstanceOfType(value))
                {
                    throw new ArgumentException($"{this.Name} should be a {type.Name} but was a {value.GetType()}");
                }
            }

            return value;
        }

        private object? NormalizeToMany(object? value)
        {
            return value switch
            {
                null => null,
                ICollection collection => this.NormalizeToMany(collection).ToArray(),
                _ => throw new ArgumentException($"{value.GetType()} is not a collection Type")
            };
        }

        private IEnumerable<IEmbeddedObject> NormalizeToMany(ICollection role)
        {
            foreach (IEmbeddedObject @object in role)
            {
                if (@object != null)
                {
                    var type = this.ObjectType.Type;

                    if (!type.IsInstanceOfType(@object))
                    {
                        throw new ArgumentException($"{this.Name} should be a {type.Name} but was a {@object.GetType()}");
                    }

                    yield return @object;
                }
            }
        }
    }
}
