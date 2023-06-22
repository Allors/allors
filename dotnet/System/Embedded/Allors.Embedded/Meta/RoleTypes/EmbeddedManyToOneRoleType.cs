namespace Allors.Embedded.Meta
{
    using System;

    public class EmbeddedManyToOneRoleType : IEmbeddedToOneRoleType
    {
        public EmbeddedManyToOneRoleType(EmbeddedObjectType objectType, string singularName)
        {
            var pluralizer = objectType.Meta.Pluralizer;

            this.ObjectType = objectType;
            this.SingularName = singularName ?? objectType.Type.Name;
            this.PluralName = pluralizer.Pluralize(this.SingularName);
        }

        public EmbeddedObjectType ObjectType { get; }

        IEmbeddedAssociationType IEmbeddedRoleType.AssociationType => this.AssociationType;

        public EmbeddedManyToOneAssociationType AssociationType { get; internal set; }

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

        public void Deconstruct(out EmbeddedManyToOneAssociationType associationType, out EmbeddedManyToOneRoleType roleType)
        {
            associationType = this.AssociationType;
            roleType = this;
        }

        public object Normalize(object value) => this.NormalizeToOne(value);
    }
}