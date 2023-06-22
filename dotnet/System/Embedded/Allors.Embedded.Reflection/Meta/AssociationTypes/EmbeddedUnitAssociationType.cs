﻿namespace Allors.Embedded.Meta
{
    using System;

    public class EmbeddedUnitAssociationType : IEmbeddedOneToAssociationType
    {
        public EmbeddedUnitAssociationType(EmbeddedObjectType objectType, EmbeddedUnitRoleType roleType)
        {
            this.ObjectType = objectType;
            roleType.AssociationType = this;
            this.RoleType = roleType;
            this.SingularName = roleType.SingularNameForAssociation(objectType);
            this.PluralName = roleType.PluralNameForAssociation(objectType);
        }

        public EmbeddedObjectType ObjectType { get; }

        IEmbeddedRoleType IEmbeddedAssociationType.RoleType => this.RoleType;

        public EmbeddedUnitRoleType RoleType { get; }

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
