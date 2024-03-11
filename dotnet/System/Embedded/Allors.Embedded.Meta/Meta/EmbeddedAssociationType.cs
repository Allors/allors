namespace Allors.Embedded.Meta
{
    public class EmbeddedAssociationType
    {
        public EmbeddedObjectType EmbeddedObjectType { get; }

        public EmbeddedRoleType EmbeddedRoleType { get; }

        public string SingularName { get; }

        public string PluralName { get; }

        public string Name { get; }

        public bool IsOne { get; }

        public bool IsMany { get; }

        internal EmbeddedAssociationType(EmbeddedObjectType embeddedObjectType, EmbeddedRoleType embeddedRoleType, string singularName, string pluralName, string name, bool isOne, bool isMany)
        {
            this.EmbeddedObjectType = embeddedObjectType;
            this.EmbeddedRoleType = embeddedRoleType;
            this.SingularName = singularName;
            this.PluralName = pluralName;
            this.Name = name;
            this.IsOne = isOne;
            this.IsMany = isMany;
        }
    }
}
