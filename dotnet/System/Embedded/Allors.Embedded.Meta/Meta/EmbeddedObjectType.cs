namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EmbeddedObjectType
    {
        private readonly Dictionary<string, EmbeddedAssociationType> assignedEmbeddedAssociationTypeByName;

        private readonly Dictionary<string, EmbeddedRoleType> assignedEmbeddedRoleTypeByName;

        private IDictionary<string, EmbeddedAssociationType>? derivedEmbeddedAssociationTypeByName;

        private IDictionary<string, EmbeddedRoleType>? derivedEmbeddedRoleTypeByName;

        internal EmbeddedObjectType(EmbeddedMeta embeddedMeta, Type type)
        {
            this.EmbeddedMeta = embeddedMeta;
            this.Type = type;
            this.SuperTypes = new HashSet<EmbeddedObjectType>();
            this.assignedEmbeddedAssociationTypeByName = new Dictionary<string, EmbeddedAssociationType>();
            this.assignedEmbeddedRoleTypeByName = new Dictionary<string, EmbeddedRoleType>();

            var hierarchyChanged = false;
            foreach (var other in embeddedMeta.EmbeddedObjectTypeByType.Values)
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
                this.EmbeddedMeta.ResetDerivations();
            }
        }

        public EmbeddedMeta EmbeddedMeta { get; }

        public ISet<EmbeddedObjectType> SuperTypes { get; }

        public Type Type { get; }

        public IDictionary<string, EmbeddedAssociationType> EmbeddedAssociationTypeByName
        {
            get
            {
                if (this.derivedEmbeddedAssociationTypeByName == null)
                {
                    this.derivedEmbeddedAssociationTypeByName = new Dictionary<string, EmbeddedAssociationType>(this.assignedEmbeddedAssociationTypeByName);
                    foreach (var item in this.SuperTypes.SelectMany(v => v.assignedEmbeddedAssociationTypeByName))
                    {
                        this.derivedEmbeddedAssociationTypeByName[item.Key] = item.Value;
                    }
                }

                return this.derivedEmbeddedAssociationTypeByName;
            }
        }

        public IDictionary<string, EmbeddedRoleType> EmbeddedRoleTypeByName
        {
            get
            {
                if (this.derivedEmbeddedRoleTypeByName == null)
                {
                    this.derivedEmbeddedRoleTypeByName = new Dictionary<string, EmbeddedRoleType>(this.assignedEmbeddedRoleTypeByName);
                    foreach (var item in this.SuperTypes.SelectMany(v => v.assignedEmbeddedRoleTypeByName))
                    {
                        this.derivedEmbeddedRoleTypeByName[item.Key] = item.Value;
                    }
                }

                return this.derivedEmbeddedRoleTypeByName;
            }
        }

        internal EmbeddedRoleType AddUnit(EmbeddedObjectType objectType, string? roleSingularName, string? associationSingularName)
        {
            roleSingularName ??= objectType.Type.Name;
            string rolePluralName = this.EmbeddedMeta.Pluralize(roleSingularName);

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

            string associationPluralName;
            if (associationSingularName != null)
            {
                associationPluralName = this.EmbeddedMeta.Pluralize(associationSingularName);
            }
            else
            {
                associationSingularName = roleType.SingularNameForEmbeddedAssociationType(this);
                associationPluralName = roleType.PluralNameForEmbeddedAssociationType(this);
            }

            roleType.EmbeddedAssociationType = new EmbeddedAssociationType(
                this,
                roleType,
                associationSingularName,
                associationPluralName,
                associationSingularName,
                true,
                false
            );

            this.AddRoleType(roleType);
            objectType.AddAssociationType(roleType.EmbeddedAssociationType);

            this.EmbeddedMeta.ResetDerivations();

            return roleType;
        }

        internal EmbeddedRoleType AddOneToOne(EmbeddedObjectType objectType, string? roleSingularName, string? associationSingularName)
        {
            roleSingularName ??= objectType.Type.Name;
            string rolePluralName = this.EmbeddedMeta.Pluralize(roleSingularName);

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

            string associationPluralName;
            if (associationSingularName != null)
            {
                associationPluralName = this.EmbeddedMeta.Pluralize(associationSingularName);
            }
            else
            {
                associationSingularName = roleType.SingularNameForEmbeddedAssociationType(this);
                associationPluralName = roleType.PluralNameForEmbeddedAssociationType(this);
            }

            roleType.EmbeddedAssociationType = new EmbeddedAssociationType(
                this,
                roleType,
                associationSingularName,
                associationPluralName,
                associationSingularName,
                true,
                false
            );

            this.AddRoleType(roleType);
            objectType.AddAssociationType(roleType.EmbeddedAssociationType);

            this.EmbeddedMeta.ResetDerivations();

            return roleType;
        }

        internal EmbeddedRoleType AddManyToOne(EmbeddedObjectType objectType, string? roleSingularName, string? associationSingularName)
        {
            roleSingularName ??= objectType.Type.Name;
            string rolePluralName = this.EmbeddedMeta.Pluralize(roleSingularName);

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

            string associationPluralName;
            if (associationSingularName != null)
            {
                associationPluralName = this.EmbeddedMeta.Pluralize(associationSingularName);
            }
            else
            {
                associationSingularName = roleType.SingularNameForEmbeddedAssociationType(this);
                associationPluralName = roleType.PluralNameForEmbeddedAssociationType(this);
            }

            roleType.EmbeddedAssociationType = new EmbeddedAssociationType
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
            objectType.AddAssociationType(roleType.EmbeddedAssociationType);

            this.EmbeddedMeta.ResetDerivations();

            return roleType;
        }

        internal EmbeddedRoleType AddOneToMany(EmbeddedObjectType objectType, string? roleSingularName, string? associationSingularName)
        {
            roleSingularName ??= objectType.Type.Name;
            string rolePluralName = this.EmbeddedMeta.Pluralize(roleSingularName);

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

            string associationPluralName;
            if (associationSingularName != null)
            {
                associationPluralName = this.EmbeddedMeta.Pluralize(associationSingularName);
            }
            else
            {
                associationSingularName = roleType.SingularNameForEmbeddedAssociationType(this);
                associationPluralName = roleType.PluralNameForEmbeddedAssociationType(this);
            }

            roleType.EmbeddedAssociationType = new EmbeddedAssociationType(
                this,
                roleType,
                associationSingularName,
                associationPluralName,
                associationSingularName,
                true,
                false
            );

            this.AddRoleType(roleType);
            objectType.AddAssociationType(roleType.EmbeddedAssociationType);

            this.EmbeddedMeta.ResetDerivations();

            return roleType;
        }

        internal EmbeddedRoleType AddManyToMany(EmbeddedObjectType objectType, string? roleSingularName, string? associationSingularName)
        {
            roleSingularName ??= objectType.Type.Name;
            string rolePluralName = this.EmbeddedMeta.Pluralize(roleSingularName);

            var roleType = new EmbeddedRoleType(
                objectType,
                roleSingularName,
                rolePluralName,
                rolePluralName,
                false,
                true,
                false
            );
            
            string associationPluralName;
            if (associationSingularName != null)
            {
                associationPluralName = this.EmbeddedMeta.Pluralize(associationSingularName);
            }
            else
            {
                associationSingularName = roleType.SingularNameForEmbeddedAssociationType(this);
                associationPluralName = roleType.PluralNameForEmbeddedAssociationType(this);
            }

            roleType.EmbeddedAssociationType = new EmbeddedAssociationType
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
            objectType.AddAssociationType(roleType.EmbeddedAssociationType);

            this.EmbeddedMeta.ResetDerivations();

            return roleType;
        }

        internal void ResetDerivations()
        {
            this.derivedEmbeddedAssociationTypeByName = null;
            this.derivedEmbeddedRoleTypeByName = null;
        }

        private void AddAssociationType(EmbeddedAssociationType associationType)
        {
            this.CheckNames(associationType.SingularName, associationType.PluralName);

            this.assignedEmbeddedAssociationTypeByName.Add(associationType.SingularName, associationType);
            this.assignedEmbeddedAssociationTypeByName.Add(associationType.PluralName, associationType);
        }

        private void AddRoleType(EmbeddedRoleType roleType)
        {
            this.CheckNames(roleType.SingularName, roleType.PluralName);

            this.assignedEmbeddedRoleTypeByName.Add(roleType.SingularName, roleType);
            this.assignedEmbeddedRoleTypeByName.Add(roleType.PluralName, roleType);
        }

        private void CheckNames(string singularName, string pluralName)
        {
            if (this.EmbeddedRoleTypeByName.ContainsKey(singularName) ||
                this.EmbeddedAssociationTypeByName.ContainsKey(singularName))
            {
                throw new ArgumentException($"{singularName} is not unique");
            }

            if (this.EmbeddedRoleTypeByName.ContainsKey(pluralName) ||
                this.EmbeddedAssociationTypeByName.ContainsKey(pluralName))
            {
                throw new ArgumentException($"{pluralName} is not unique");
            }
        }
    }
}
