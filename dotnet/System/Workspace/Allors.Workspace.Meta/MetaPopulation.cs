// <copyright file="IMetaPopulation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class MetaPopulation
    {
        // class
        public Unit[] Units { get; private set; }
        public Interface[] Interfaces { get; private set; }
        public Class[] Classes { get; private set; }
        public RelationType[] RelationTypes { get; private set; }
        public MethodType[] MethodTypes { get; private set; }

        public Dictionary<string, IMetaObject> MetaObjectByTag { get; private set; }
        public IComposite[] Composites { get; private set; }
        public Dictionary<string, IComposite> CompositeByLowercaseName { get; private set; }

        public IMetaObject FindByTag(string tag)
        {
            this.MetaObjectByTag.TryGetValue(tag, out var metaObject);
            return metaObject;
        }

        public IComposite FindByName(string name)
        {
            this.CompositeByLowercaseName.TryGetValue(name.ToLowerInvariant(), out var composite);
            return composite;
        }

        public void Bind(Type[] types)
        {
            var typeByName = types.ToDictionary(type => type.Name, type => type);

            foreach (var unit in this.Units)
            {
                unit.Bind();
            }

            foreach (var @interface in this.Interfaces)
            {
                @interface.Bind(typeByName);
            }

            foreach (var @class in this.Classes)
            {
                @class.Bind(typeByName);
            }
        }

        public void Init(Unit[] units, Interface[] interfaces, Class[] classes, Inheritance[] inheritances, RelationType[] relationTypes, MethodType[] methodTypes)
        {
            this.Units = units;
            this.Interfaces = interfaces;
            this.Classes = classes;
            this.RelationTypes = relationTypes;
            this.MethodTypes = methodTypes;

            this.MetaObjectByTag =
                this.Units.Cast<IMetaObject>()
                .Union(this.Classes)
                .Union(this.Interfaces)
                .Union(this.RelationTypes)
                .Union(this.MethodTypes)
                .ToDictionary(v => v.Tag, v => v);

            this.Composites = this.Interfaces.Cast<IComposite>().Union(this.Classes).ToArray();
            this.CompositeByLowercaseName = this.Composites.ToDictionary(v => v.SingularName.ToLowerInvariant());

            foreach (var composite in this.Composites)
            {
                composite.MetaPopulation = this;
            }

            foreach (var unit in this.Units)
            {
                unit.MetaPopulation = this;
            }

            foreach (var methodType in this.MethodTypes)
            {
                methodType.MetaPopulation = this;
            }

            // DirectSupertypes
            foreach (var grouping in inheritances.GroupBy(v => v.Subtype, v => v.Supertype))
            {
                var composite = grouping.Key;
                composite.DirectSupertypes = new HashSet<Interface>(grouping);
            }

            // DirectSubtypes
            foreach (var grouping in inheritances.GroupBy(v => v.Supertype, v => v.Subtype))
            {
                var @interface = grouping.Key;
                @interface.DirectSubtypes = new HashSet<IComposite>(grouping);
            }

            // Supertypes
            foreach (var composite in this.Composites)
            {
                static IEnumerable<Interface> RecurseDirectSupertypes(IComposite composite)
                {
                    if (composite.DirectSupertypes != null)
                    {
                        foreach (var directSupertype in composite.DirectSupertypes)
                        {
                            yield return directSupertype;

                            foreach (var directSuperSupertype in RecurseDirectSupertypes(directSupertype))
                            {
                                yield return directSuperSupertype;
                            }
                        }
                    }
                }

                composite.Supertypes = new HashSet<Interface>(RecurseDirectSupertypes(composite));
            }

            // Subtypes
            foreach (var @interface in this.Interfaces)
            {
                static IEnumerable<IComposite> RecurseDirectSubtypes(Interface @interface)
                {
                    if (@interface.DirectSubtypes != null)
                    {
                        foreach (var directSubtype in @interface.DirectSubtypes)
                        {
                            yield return directSubtype;

                            if (directSubtype is Interface directSubinterface)
                            {
                                foreach (var directSubSubtype in RecurseDirectSubtypes(directSubinterface))
                                {
                                    yield return directSubSubtype;
                                }
                            }
                        }
                    }
                }

                @interface.Subtypes = new HashSet<IComposite>(RecurseDirectSubtypes(@interface));
                @interface.Classes = new HashSet<Class>(@interface.Subtypes.Where(v => v.IsClass).Cast<Class>());
            }

            // RoleTypes
            {
                var exclusiveRoleTypesObjectType = this.RelationTypes
                    .GroupBy(v => v.AssociationType.ObjectType)
                    .ToDictionary(g => g.Key, g => g.Select(v => v.RoleType).ToArray());

                foreach (var objectType in this.Composites)
                {
                    exclusiveRoleTypesObjectType.TryGetValue(objectType, out var exclusiveRoleTypes);
                    objectType.ExclusiveRoleTypes = exclusiveRoleTypes ?? Array.Empty<RoleType>();
                }
            }

            // AssociationTypes
            {
                var exclusiveAssociationTypesByObjectType = this.RelationTypes
                   .GroupBy(v => v.RoleType.ObjectType)
                   .ToDictionary(g => g.Key, g => g.Select(v => v.AssociationType).ToArray());

                foreach (var objectType in this.Composites)
                {
                    exclusiveAssociationTypesByObjectType.TryGetValue(objectType, out var exclusiveAssociationTypes);
                    objectType.ExclusiveAssociationTypes = exclusiveAssociationTypes ?? Array.Empty<AssociationType>();
                }
            }

            // MethodTypes
            {
                var exclusiveMethodTypeByObjectType = this.MethodTypes
                    .GroupBy(v => v.ObjectType)
                    .ToDictionary(g => g.Key, g => g.ToArray());

                foreach (var objectType in this.Composites)
                {
                    exclusiveMethodTypeByObjectType.TryGetValue(objectType, out var exclusiveMethodTypes);
                    objectType.ExclusiveMethodTypes = exclusiveMethodTypes ?? Array.Empty<MethodType>();
                }
            }
        }
    }
}
