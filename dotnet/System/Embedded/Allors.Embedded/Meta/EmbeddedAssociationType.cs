namespace Allors.Embedded.Meta
{
    public class EmbeddedAssociationType
    {
        public EmbeddedObjectType ObjectType { get; internal set; }

        public EmbeddedRoleType RoleType { get; internal set; }

        public string Name { get; internal set; }

        public string SingularName { get; internal set; }

        public string PluralName { get; internal set; }

        public bool IsOne { get; internal set; }

        public bool IsMany { get; internal set; }
    }
}
