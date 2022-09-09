// <copyright file="Composite.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Composite : ObjectType, IComposite
    {

        private HashSet<Interface> structuralDerivedDirectSupertypes;
        private HashSet<Interface> structuralDerivedSupertypes;

        private HashSet<AssociationType> structuralDerivedAssociationTypes;
        private HashSet<RoleType> structuralDerivedRoleTypes;

        private HashSet<MethodType> structuralDerivedMethodTypes;

        protected Composite(MetaPopulation metaPopulation, Guid id, string tag) : base(metaPopulation, id, tag) { }

        public abstract IEnumerable<Class> Classes { get; }

        public abstract bool ExistClass { get; }

        public bool ExistExclusiveClass
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.ExclusiveClass != null;
            }
        }

        IClass IComposite.ExclusiveClass => this.ExclusiveClass;
        public abstract Class ExclusiveClass { get; }

        public IEnumerable<Interface> DirectSupertypes => this.structuralDerivedDirectSupertypes;

        IEnumerable<IInterface> IComposite.Supertypes => this.Supertypes;
        public IEnumerable<Interface> Supertypes => this.structuralDerivedSupertypes;

        IEnumerable<IAssociationType> IComposite.AssociationTypes => this.AssociationTypes;
        public IEnumerable<AssociationType> AssociationTypes => this.structuralDerivedAssociationTypes;

        public IEnumerable<AssociationType> ExclusiveAssociationTypes => this.AssociationTypes.Where(associationType => this.Equals(associationType.RoleType.ObjectType)).ToArray();

        IEnumerable<IAssociationType> IComposite.ExclusiveAssociationTypes => this.ExclusiveDatabaseAssociationTypes;
        public IEnumerable<AssociationType> ExclusiveDatabaseAssociationTypes => this.ExclusiveAssociationTypes.ToArray();

        IEnumerable<IAssociationType> IComposite.InheritedAssociationTypes => this.InheritedAssociationTypes;
        public IEnumerable<AssociationType> InheritedAssociationTypes => this.AssociationTypes.Except(this.ExclusiveAssociationTypes);

        IEnumerable<IRoleType> IComposite.RoleTypes => this.RoleTypes;
        public IEnumerable<RoleType> RoleTypes => this.structuralDerivedRoleTypes;

        public IEnumerable<RoleType> ExclusiveRoleTypes => this.RoleTypes.Where(roleType => this.Equals(roleType.AssociationType.ObjectType)).ToArray();

        IEnumerable<IRoleType> IComposite.ExclusiveRoleTypes => this.ExclusiveDatabaseRoleTypes;
        public IEnumerable<RoleType> ExclusiveDatabaseRoleTypes => this.ExclusiveRoleTypes.ToArray();

        IEnumerable<IMethodType> IComposite.MethodTypes => this.MethodTypes;
        public IEnumerable<MethodType> MethodTypes => this.structuralDerivedMethodTypes;

        IEnumerable<IMethodType> IComposite.ExclusiveMethodTypes => this.ExclusiveMethodTypes;
        public IEnumerable<MethodType> ExclusiveMethodTypes => this.MethodTypes.Where(methodType => this.Equals(methodType.ObjectType)).ToArray();

        IEnumerable<IMethodType> IComposite.InheritedMethodTypes => this.InheritedMethodTypes;
        public IEnumerable<MethodType> InheritedMethodTypes => this.MethodTypes.Except(this.ExclusiveMethodTypes);

        IEnumerable<IRoleType> IComposite.InheritedRoleTypes => this.InheritedRoleTypes;
        public IEnumerable<RoleType> InheritedRoleTypes => this.RoleTypes.Except(this.ExclusiveRoleTypes);

        public IEnumerable<RoleType> ExclusiveCompositeRoleTypes
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.ExclusiveRoleTypes.Where(roleType => roleType.ObjectType.IsComposite);
            }
        }

        IEnumerable<IComposite> IComposite.Subtypes => this.Subtypes;
        public abstract IEnumerable<Composite> Subtypes { get; }

        IEnumerable<IClass> IComposite.Classes => this.Classes;

        public bool ExistSupertype(IInterface @interface) => this.structuralDerivedSupertypes.Contains(@interface);

        public bool ExistAssociationType(IAssociationType associationType) => this.structuralDerivedAssociationTypes.Contains(associationType);

        public bool ExistRoleType(IRoleType roleType) => this.structuralDerivedRoleTypes.Contains(roleType);

        public abstract bool IsAssignableFrom(IComposite objectType);

        public abstract void Bind(Dictionary<string, Type> typeByName);

        internal void StructuralDeriveDirectSupertypes(HashSet<Interface> directSupertypes)
        {
            directSupertypes.Clear();
            foreach (var inheritance in this.MetaPopulation.Inheritances.Where(inheritance => this.Equals(inheritance.Subtype)))
            {
                directSupertypes.Add(inheritance.Supertype);
            }

            this.structuralDerivedDirectSupertypes = new HashSet<Interface>(directSupertypes);
        }

        internal void StructuralDeriveSupertypes(HashSet<Interface> superTypes)
        {
            superTypes.Clear();

            this.StructuralDeriveSupertypesRecursively(this, superTypes);

            this.structuralDerivedSupertypes = new HashSet<Interface>(superTypes);
        }

        internal void StructuralDeriveRoleTypes(HashSet<RoleType> roleTypes, Dictionary<Composite, HashSet<RoleType>> roleTypesByAssociationObjectType)
        {
            roleTypes.Clear();

            if (roleTypesByAssociationObjectType.TryGetValue(this, out var directRoleTypes))
            {
                roleTypes.UnionWith(directRoleTypes);
            }

            foreach (var superType in this.Supertypes)
            {
                if (roleTypesByAssociationObjectType.TryGetValue(superType, out var inheritedRoleTypes))
                {
                    roleTypes.UnionWith(inheritedRoleTypes);
                }
            }

            this.structuralDerivedRoleTypes = new HashSet<RoleType>(roleTypes);
        }

        internal void StructuralDeriveAssociationTypes(HashSet<AssociationType> associationTypes, Dictionary<ObjectType, HashSet<AssociationType>> relationTypesByRoleObjectType)
        {
            associationTypes.Clear();

            if (relationTypesByRoleObjectType.TryGetValue(this, out var classAssociationTypes))
            {
                associationTypes.UnionWith(classAssociationTypes);
            }

            foreach (var superType in this.Supertypes)
            {
                if (relationTypesByRoleObjectType.TryGetValue(superType, out var interfaceAssociationTypes))
                {
                    associationTypes.UnionWith(interfaceAssociationTypes);
                }
            }

            this.structuralDerivedAssociationTypes = new HashSet<AssociationType>(associationTypes);
        }

        internal void StructuralDeriveMethodTypes(HashSet<MethodType> methodTypes, Dictionary<Composite, HashSet<MethodType>> methodTypeByClass)
        {
            methodTypes.Clear();

            if (methodTypeByClass.TryGetValue(this, out var directMethodTypes))
            {
                methodTypes.UnionWith(directMethodTypes);
            }

            foreach (var superType in this.Supertypes)
            {
                if (methodTypeByClass.TryGetValue(superType, out var inheritedMethodTypes))
                {
                    methodTypes.UnionWith(inheritedMethodTypes);
                }
            }

            this.structuralDerivedMethodTypes = new HashSet<MethodType>(methodTypes);
        }

        internal void StructuralDeriveSupertypesRecursively(ObjectType type, HashSet<Interface> superTypes)
        {
            foreach (var directSupertype in this.DirectSupertypes)
            {
                if (!Equals(directSupertype, type))
                {
                    superTypes.Add(directSupertype);
                    directSupertype.StructuralDeriveSupertypesRecursively(type, superTypes);
                }
            }
        }
    }
}
