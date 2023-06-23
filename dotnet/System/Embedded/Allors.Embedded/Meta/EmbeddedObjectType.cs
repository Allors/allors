﻿namespace Allors.Embedded.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EmbeddedObjectType
    {
        private readonly IDictionary<string, IEmbeddedAssociationType> assignedAssociationTypeByName;

        private readonly IDictionary<string, IEmbeddedRoleType> assignedRoleTypeByName;

        private IDictionary<string, IEmbeddedAssociationType> derivedAssociationTypeByName;

        private IDictionary<string, IEmbeddedRoleType> derivedRoleTypeByName;

        public EmbeddedObjectType(IEmbeddedMeta meta, Type type)
        {
            this.Meta = meta;
            this.Type = type;
            this.TypeCode = Type.GetTypeCode(type);
            this.SuperTypes = new HashSet<EmbeddedObjectType>();
            this.assignedAssociationTypeByName = new Dictionary<string, IEmbeddedAssociationType>();
            this.assignedRoleTypeByName = new Dictionary<string, IEmbeddedRoleType>();

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

        public IEmbeddedMeta Meta { get; }

        public Type Type { get; }

        public TypeCode TypeCode { get; }

        public ISet<EmbeddedObjectType> SuperTypes { get; }

        public IDictionary<string, IEmbeddedAssociationType> AssociationTypeByName
        {
            get
            {
                if (this.derivedAssociationTypeByName == null)
                {
                    this.derivedAssociationTypeByName = new Dictionary<string, IEmbeddedAssociationType>(this.assignedAssociationTypeByName);
                    foreach (var item in this.SuperTypes.SelectMany(v => v.assignedAssociationTypeByName))
                    {
                        this.derivedAssociationTypeByName[item.Key] = item.Value;
                    }
                }

                return this.derivedAssociationTypeByName;
            }
        }

        public IDictionary<string, IEmbeddedRoleType> RoleTypeByName
        {
            get
            {
                if (this.derivedRoleTypeByName == null)
                {
                    this.derivedRoleTypeByName = new Dictionary<string, IEmbeddedRoleType>(this.assignedRoleTypeByName);
                    foreach (var item in this.SuperTypes.SelectMany(v => v.assignedRoleTypeByName))
                    {
                        this.derivedRoleTypeByName[item.Key] = item.Value;
                    }
                }

                return this.derivedRoleTypeByName;
            }
        }

        internal object EmptyArray { get; }

        public EmbeddedUnitRoleType AddUnit(EmbeddedObjectType roleObjectType, string roleName)
        {
            var roleType = new EmbeddedUnitRoleType(roleObjectType, roleName);
            this.AddRoleType(roleType);

            var associationType = new EmbeddedUnitAssociationType(this, roleType);
            roleObjectType.AddAssociationType(associationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        public EmbeddedOneToOneRoleType AddOneToOne(EmbeddedObjectType roleObjectType, string roleName)
        {
            var roleType = new EmbeddedOneToOneRoleType(roleObjectType, roleName);
            this.AddRoleType(roleType);

            var associationType = new EmbeddedOneToOneAssociationType(this, roleType);
            roleObjectType.AddAssociationType(associationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        public EmbeddedManyToOneRoleType AddManyToOne(EmbeddedObjectType roleObjectType, string roleName)
        {
            var roleType = new EmbeddedManyToOneRoleType(roleObjectType, roleName);
            this.AddRoleType(roleType);

            var associationType = new EmbeddedManyToOneAssociationType(this, roleType);
            roleObjectType.AddAssociationType(associationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        public EmbeddedOneToManyRoleType AddOneToMany(EmbeddedObjectType roleObjectType, string roleName)
        {
            var roleType = new EmbeddedOneToManyRoleType(roleObjectType, roleName);
            this.AddRoleType(roleType);

            var associationType = new EmbeddedOneToManyAssociationType(this, roleType);
            roleObjectType.AddAssociationType(associationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        public EmbeddedManyToManyRoleType AddManyToMany(EmbeddedObjectType roleObjectType, string roleName)
        {
            var roleType = new EmbeddedManyToManyRoleType(roleObjectType, roleName);
            this.AddRoleType(roleType);

            var associationType = new EmbeddedManyToManyAssociationType(this, roleType);
            roleObjectType.AddAssociationType(associationType);

            this.Meta.ResetDerivations();

            return roleType;
        }

        public void ResetDerivations()
        {
            this.derivedAssociationTypeByName = null;
            this.derivedRoleTypeByName = null;
        }

        private void AddAssociationType(IEmbeddedAssociationType associationType)
        {
            this.CheckNames(associationType.SingularName, associationType.PluralName);

            this.assignedAssociationTypeByName.Add(associationType.SingularName, associationType);
            this.assignedAssociationTypeByName.Add(associationType.PluralName, associationType);
        }

        private void AddRoleType(IEmbeddedRoleType roleType)
        {
            this.CheckNames(roleType.SingularName, roleType.PluralName);

            this.assignedRoleTypeByName.Add(roleType.SingularName, roleType);
            this.assignedRoleTypeByName.Add(roleType.PluralName, roleType);
        }

        private void CheckNames(string singularName, string pluralName)
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
