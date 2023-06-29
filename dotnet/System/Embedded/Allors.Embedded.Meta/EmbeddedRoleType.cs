namespace Allors.Embedded.Meta
{
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
    }
}
