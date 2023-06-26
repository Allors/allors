namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EmbeddedObjectType
    {
        private readonly IDictionary<string, EmbeddedAssociationType> assignedAssociationTypeByName;

        private readonly IDictionary<string, EmbeddedRoleType> assignedRoleTypeByName;

        private IDictionary<string, EmbeddedAssociationType> derivedAssociationTypeByName;

        private IDictionary<string, EmbeddedRoleType> derivedRoleTypeByName;

        public EmbeddedObjectType(EmbeddedMeta meta, Type type)
        {
            this.Meta = meta;
            this.Type = type;
            this.TypeCode = Type.GetTypeCode(type);
            this.SuperTypes = new HashSet<EmbeddedObjectType>();
            this.assignedAssociationTypeByName = new Dictionary<string, EmbeddedAssociationType>();
            this.assignedRoleTypeByName = new Dictionary<string, EmbeddedRoleType>();

            this.EmptyArray = Array.CreateInstance(type, 0);

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

        public Type Type { get; }

        public TypeCode TypeCode { get; }

        public ISet<EmbeddedObjectType> SuperTypes { get; }

        public IDictionary<string, EmbeddedAssociationType> AssociationTypeByName
        {
            get
            {
                if (this.derivedAssociationTypeByName == null)
                {
                    this.derivedAssociationTypeByName = new Dictionary<string, EmbeddedAssociationType>(this.assignedAssociationTypeByName);
                    // TODO: Remove cast
                    foreach (var item in this.SuperTypes.SelectMany(v => ((EmbeddedObjectType)v).assignedAssociationTypeByName))
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
                    // TODO: Remove cast
                    foreach (var item in this.SuperTypes.SelectMany(v => ((EmbeddedObjectType)v).assignedRoleTypeByName))
                    {
                        this.derivedRoleTypeByName[item.Key] = item.Value;
                    }
                }

                return this.derivedRoleTypeByName;
            }
        }

        internal object EmptyArray { get; }

        internal EmbeddedRoleType AddUnit(EmbeddedObjectType objectType, string? name)
        {
            var roleSingularName = name ?? objectType.Type.Name;
            string rolePluralName = this.Meta.Pluralize(roleSingularName);

            var roleType = new EmbeddedRoleType
            {
                ObjectType = objectType,
                SingularName = roleSingularName,
                PluralName = rolePluralName,
                Name = roleSingularName,
                IsOne = true,
                IsMany = false,
                IsUnit = true
            };

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType
            {
                ObjectType = this,
                RoleType = roleType,
                SingularName = associationSingularName,
                PluralName = associationPluralName,
                Name = associationSingularName,
                IsOne = true,
                IsMany = false,
            };

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
            {
                ObjectType = objectType,
                SingularName = roleSingularName,
                PluralName = rolePluralName,
                Name = roleSingularName,
                IsOne = true,
                IsMany = false,
                IsUnit = false
            };

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType
            {
                ObjectType = this,
                RoleType = roleType,
                SingularName = associationSingularName,
                PluralName = associationPluralName,
                Name = associationSingularName,
                IsOne = true,
                IsMany = false,
            };

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
            {
                ObjectType = objectType,
                SingularName = roleSingularName,
                PluralName = rolePluralName,
                Name = roleSingularName,
                IsOne = true,
                IsMany = false,
                IsUnit = false
            };

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType
            {
                ObjectType = this,
                RoleType = roleType,
                SingularName = associationSingularName,
                PluralName = associationPluralName,
                Name = associationPluralName,
                IsOne = false,
                IsMany = true,
            };

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
            {
                ObjectType = objectType,
                SingularName = roleSingularName,
                PluralName = rolePluralName,
                Name = rolePluralName,
                IsOne = false,
                IsMany = true,
                IsUnit = false
            };

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType
            {
                ObjectType = this,
                RoleType = roleType,
                SingularName = associationSingularName,
                PluralName = associationPluralName,
                Name = associationSingularName,
                IsOne = true,
                IsMany = false,
            };

            this.AddRoleType(roleType);
            objectType.AddAssociationType(roleType.AssociationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        internal EmbeddedRoleType AddManyToMany(EmbeddedObjectType objectType, string? name)
        {
            var roleSingularName = name ?? objectType.Type.Name;
            string rolePluralName = this.Meta.Pluralize(roleSingularName);

            var roleType = new EmbeddedRoleType
            {
                ObjectType = objectType,
                SingularName = roleSingularName,
                PluralName = rolePluralName,
                Name = rolePluralName,
                IsOne = false,
                IsMany = true,
                IsUnit = false
            };

            var associationSingularName = roleType.SingularNameForAssociation(this);
            var associationPluralName = roleType.PluralNameForAssociation(this);

            roleType.AssociationType = new EmbeddedAssociationType
            {
                ObjectType = this,
                RoleType = roleType,
                SingularName = associationSingularName,
                PluralName = associationPluralName,
                Name = associationPluralName,
                IsOne = false,
                IsMany = true,
            };

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
