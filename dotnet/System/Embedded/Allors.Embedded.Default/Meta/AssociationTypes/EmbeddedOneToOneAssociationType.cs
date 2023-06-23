namespace Allors.Embedded.Meta
{
    public class EmbeddedOneToOneAssociationType : IEmbeddedOneToAssociationType
    {
        public EmbeddedOneToOneAssociationType(EmbeddedObjectType objectType, EmbeddedOneToOneRoleType roleType)
        {
            this.ObjectType = objectType;
            roleType.AssociationType = this;
            this.RoleType = roleType;
            this.SingularName = roleType.SingularNameForAssociation(objectType);
            this.PluralName = roleType.PluralNameForAssociation(objectType);
        }

        public IEmbeddedObjectType ObjectType { get; }

        IEmbeddedRoleType IEmbeddedAssociationType.RoleType => this.RoleType;

        public EmbeddedOneToOneRoleType RoleType { get; }

        public string Name => this.SingularName;

        public string SingularName { get; }

        public string PluralName { get; }

        public bool IsOne => true;

        public bool IsMany => false;

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
