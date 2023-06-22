﻿namespace Allors.Embedded.Meta
{
    using System;

    public class EmbeddedManyToManyRoleType : IEmbeddedToManyRoleType
    {
        public EmbeddedManyToManyRoleType(EmbeddedObjectType objectType, string singularName)
        {
            var pluralizer = objectType.Meta.Pluralizer;

            this.ObjectType = objectType;
            this.SingularName = singularName ?? objectType.Type.Name;
            this.PluralName = pluralizer.Pluralize(this.SingularName);
        }

        public EmbeddedObjectType ObjectType { get; }

        IEmbeddedAssociationType IEmbeddedRoleType.AssociationType => this.AssociationType;

        public EmbeddedManyToManyAssociationType AssociationType { get; internal set; }

        public string Name => this.PluralName;

        public string SingularName { get; }

        public string PluralName { get; }

        public bool IsOne => false;

        public bool IsMany => true;

        public bool IsUnit => false;

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Name;
        }

        public void Deconstruct(out EmbeddedManyToManyAssociationType associationType, out EmbeddedManyToManyRoleType roleType)
        {
            associationType = this.AssociationType;
            roleType = this;
        }

        public object Normalize(object value) => this.NormalizeToMany(value);
    }
}