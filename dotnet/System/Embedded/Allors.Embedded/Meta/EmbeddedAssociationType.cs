namespace Allors.Embedded.Meta
{
    public class EmbeddedAssociationType
    {
        public EmbeddedObjectType ObjectType { get; }

        public EmbeddedRoleType RoleType { get; }

        public string SingularName { get; }

        public string PluralName { get; }

        public string Name { get; }

        public bool IsOne { get; }

        public bool IsMany { get; }

        internal EmbeddedAssociationType(EmbeddedObjectType objectType, EmbeddedRoleType roleType, string singularName, string pluralName, string name, bool isOne, bool isMany)
        {
            this.ObjectType = objectType;
            this.RoleType = roleType;
            this.SingularName = singularName;
            this.PluralName = pluralName;
            this.Name = name;
            this.IsOne = isOne;
            this.IsMany = isMany;
        }
    }
}
