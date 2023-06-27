namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EmbeddedObjectType
    {
        private readonly IDictionary<string, EmbeddedAssociationType> assignedAssociationTypeByName;

        private readonly IDictionary<string, EmbeddedRoleType> assignedRoleTypeByName;

        private IDictionary<string, EmbeddedAssociationType>? derivedAssociationTypeByName;

        private IDictionary<string, EmbeddedRoleType>? derivedRoleTypeByName;

        internal EmbeddedObjectType(EmbeddedMeta meta, Type type)
        {
            this.Meta = meta;
            this.Type = type;
            this.SuperTypes = new HashSet<EmbeddedObjectType>();
            this.assignedAssociationTypeByName = new Dictionary<string, EmbeddedAssociationType>();
            this.assignedRoleTypeByName = new Dictionary<string, EmbeddedRoleType>();

            var hierarchyChanged = false;
            foreach (var other in meta.ObjectTypeByType.Values)
            {
                if (this.Type.IsAssignableFrom(other.Type))
                {
                    other.SuperTypes.Add(this);
                    hierarchyChanged = true;
                }

                if (other.Type.IsAssignableFrom(this.Type))
                {
                    this.SuperTypes.Add(other);
                    hierarchyChanged = true;
                }
            }

            if (hierarchyChanged)
            {
                this.Meta.ResetDerivations();
            }
        }

        public EmbeddedMeta Meta { get; }

        public ISet<EmbeddedObjectType> SuperTypes { get; }

        public Type Type { get; }

        public IDictionary<string, EmbeddedAssociationType> AssociationTypeByName
        {
            get
            {
                if (this.derivedAssociationTypeByName == null)
                {
                    this.derivedAssociationTypeByName = new Dictionary<string, EmbeddedAssociationType>(this.assignedAssociationTypeByName);
                    foreach (var item in this.SuperTypes.SelectMany(v => v.assignedAssociationTypeByName))
                    {
                        this.derivedAssociationTypeByName[item.Key] = item.Value;
                    }
                }

                return this.derivedAssociationTypeByName;
            }
        }

        public IDictionary<string, EmbeddedRoleType> RoleTypeByName
        {
            get
            {
                if (this.derivedRoleTypeByName == null)
                {
                    this.derivedRoleTypeByName = new Dictionary<string, EmbeddedRoleType>(this.assignedRoleTypeByName);
                    foreach (var item in this.SuperTypes.SelectMany(v => v.assignedRoleTypeByName))
                    {
                        this.derivedRoleTypeByName[item.Key] = item.Value;
                    }
                }

                return this.derivedRoleTypeByName;
            }
        }

        internal EmbeddedRoleType AddUnit(EmbeddedObjectType objectType, string? name)
        {
            var roleSingularName = name ?? objectType.Type.Name;
            string rolePluralName = this.Meta.Pluralize(roleSingularName);

            var roleType = new EmbeddedRoleType
            (
                objectType,
                roleSingularName,
                rolePluralName,
                roleSingularName,
                true,
                false,
                true
            );

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType(
                this,
                roleType,
                associationSingularName,
                associationPluralName,
                associationSingularName,
                true,
                false
            );

            this.AddRoleType(roleType);
            objectType.AddAssociationType(roleType.AssociationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        internal EmbeddedRoleType AddOneToOne(EmbeddedObjectType objectType, string? name)
        {
            var roleSingularName = name ?? objectType.Type.Name;
            string rolePluralName = this.Meta.Pluralize(roleSingularName);

            var roleType = new EmbeddedRoleType
            (
                objectType,
                roleSingularName,
                rolePluralName,
                roleSingularName,
                true,
                false,
                false
            );

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType(
                this,
                roleType,
                associationSingularName,
                associationPluralName,
                associationSingularName,
                true,
                false
            );

            this.AddRoleType(roleType);
            objectType.AddAssociationType(roleType.AssociationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        internal EmbeddedRoleType AddManyToOne(EmbeddedObjectType objectType, string? name)
        {
            var roleSingularName = name ?? objectType.Type.Name;
            string rolePluralName = this.Meta.Pluralize(roleSingularName);

            var roleType = new EmbeddedRoleType
            (
                objectType,
                roleSingularName,
                rolePluralName,
                roleSingularName,
                true,
                false,
                false
            );

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType
            (
                this,
                roleType,
                associationSingularName,
                associationPluralName,
                associationPluralName,
                false,
                true
            );

            this.AddRoleType(roleType);
            objectType.AddAssociationType(roleType.AssociationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        internal EmbeddedRoleType AddOneToMany(EmbeddedObjectType objectType, string? name)
        {
            var roleSingularName = name ?? objectType.Type.Name;
            string rolePluralName = this.Meta.Pluralize(roleSingularName);

            var roleType = new EmbeddedRoleType
            (
                objectType,
                roleSingularName,
                rolePluralName,
                rolePluralName,
                false,
                true,
                false
            );

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType(
                this,
                roleType,
                associationSingularName,
                associationPluralName,
                associationSingularName,
                true,
                false
            );

            this.AddRoleType(roleType);
            objectType.AddAssociationType(roleType.AssociationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        internal EmbeddedRoleType AddManyToMany(EmbeddedObjectType objectType, string? name)
        {
            var roleSingularName = name ?? objectType.Type.Name;
            string rolePluralName = this.Meta.Pluralize(roleSingularName);

            var roleType = new EmbeddedRoleType(
                objectType,
                roleSingularName,
                rolePluralName,
                rolePluralName,
                false,
                true,
                false
            );

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType
            (
                this,
                roleType,
                associationSingularName,
                associationPluralName,
                associationPluralName,
                false,
                true
            );

            this.AddRoleType(roleType);
            objectType.AddAssociationType(roleType.AssociationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        internal void ResetDerivations()
        {
            this.derivedAssociationTypeByName = null;
            this.derivedRoleTypeByName = null;
        }

        internal void AddAssociationType(EmbeddedAssociationType associationType)
        {
            this.CheckNames(associationType.SingularName, associationType.PluralName);

            this.assignedAssociationTypeByName.Add(associationType.SingularName, associationType);
            this.assignedAssociationTypeByName.Add(associationType.PluralName, associationType);
        }

        internal void AddRoleType(EmbeddedRoleType roleType)
        {
            this.CheckNames(roleType.SingularName, roleType.PluralName);

            this.assignedRoleTypeByName.Add(roleType.SingularName, roleType);
            this.assignedRoleTypeByName.Add(roleType.PluralName, roleType);
        }

        internal void CheckNames(string singularName, string pluralName)
        {
            if (this.RoleTypeByName.ContainsKey(singularName) ||
                this.AssociationTypeByName.ContainsKey(singularName))
            {
                throw new Exception($"{singularName} is not unique");
            }

            if (this.RoleTypeByName.ContainsKey(pluralName) ||
                this.AssociationTypeByName.ContainsKey(pluralName))
            {
                throw new Exception($"{pluralName} is not unique");
            }
        }
    }
}
