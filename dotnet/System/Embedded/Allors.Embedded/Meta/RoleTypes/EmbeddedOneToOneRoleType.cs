namespace Allors.Embedded.Meta
{
    public class EmbeddedOneToOneRoleType : IEmbeddedRoleType
    {
        public EmbeddedOneToOneRoleType(EmbeddedObjectType objectType, string singularName)
        {
            var meta = objectType.Meta;

            this.ObjectType = objectType;
            this.SingularName = singularName ?? objectType.Type.Name;
            this.PluralName = meta.Pluralize(this.SingularName);
        }

        public EmbeddedObjectType ObjectType { get; }

        IEmbeddedAssociationType IEmbeddedRoleType.AssociationType => this.AssociationType;

        public EmbeddedOneToOneAssociationType AssociationType { get; internal set; }

        public string Name => this.SingularName;

        public string SingularName { get; }

        public string PluralName { get; }

        public bool IsOne => true;

        public bool IsMany => false;

        public bool IsUnit => false;

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }

        public void Deconstruct(out EmbeddedOneToOneAssociationType associationType, out EmbeddedOneToOneRoleType roleType)
        {
            associationType = this.AssociationType;
            roleType = this;
        }

        public object Normalize(object value) => this.NormalizeToOne(value);
    }
}
