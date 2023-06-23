namespace Allors.Embedded.Meta
{
    public class EmbeddedManyToOneAssociationType : IEmbeddedManyToAssociationType
    {
        public EmbeddedManyToOneAssociationType(EmbeddedObjectType objectType, EmbeddedManyToOneRoleType roleType)
        {
            this.ObjectType = objectType;
            roleType.AssociationType = this;
            this.RoleType = roleType;
            this.SingularName = roleType.SingularNameForAssociation(objectType);
            this.PluralName = roleType.PluralNameForAssociation(objectType);
        }

        public EmbeddedObjectType ObjectType { get; }

        IEmbeddedRoleType IEmbeddedAssociationType.RoleType => this.RoleType;

        public EmbeddedManyToOneRoleType RoleType { get; }

        public string Name => this.PluralName;

        public string SingularName { get; }

        public string PluralName { get; }

        public bool IsOne => false;

        public bool IsMany => true;

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
