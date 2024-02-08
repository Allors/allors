namespace Allors.Embedded.Meta
{
    public class EmbeddedRoleType
    {
        public EmbeddedAssociationType EmbeddedAssociationType { get; internal set; } = null!;

        public EmbeddedObjectType EmbeddedObjectType { get; }

        public string SingularName { get; }

        public string PluralName { get; }

        public string Name { get; }

        public bool IsOne { get; }

        public bool IsMany { get;}

        public bool IsUnit { get;  }

        internal EmbeddedRoleType(EmbeddedObjectType embeddedObjectType, string singularName, string pluralName, string name, bool isOne, bool isMany, bool isUnit)
        {
            this.EmbeddedObjectType = embeddedObjectType;
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

        internal string SingularNameForEmbeddedAssociationType(EmbeddedObjectType embeddedObjectType)
        {
            return $"{embeddedObjectType.Type.Name}Where{this.SingularName}";
        }

        internal string PluralNameForEmbeddedAssociationType(EmbeddedObjectType embeddedObjectType)
        {
            return $"{this.EmbeddedObjectType.EmbeddedMeta.Pluralize(embeddedObjectType.Type.Name)}Where{this.SingularName}";
        }
    }
}
