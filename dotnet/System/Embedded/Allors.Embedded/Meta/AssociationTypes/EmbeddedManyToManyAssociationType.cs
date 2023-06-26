namespace Allors.Embedded.Meta
{
    public class EmbeddedManyToManyAssociationType : IEmbeddedAssociationType
    {
        public EmbeddedManyToManyAssociationType(EmbeddedObjectType objectType, EmbeddedManyToManyRoleType roleType)
        {
            this.ObjectType = objectType;
            roleType.AssociationType = this;
            this.RoleType = roleType;
            this.SingularName = roleType.SingularNameForAssociation(objectType);
            this.PluralName = roleType.PluralNameForAssociation(objectType);
        }

        public EmbeddedObjectType ObjectType { get; }

        public EmbeddedManyToManyRoleType RoleType { get; }

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
