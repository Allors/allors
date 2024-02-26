// <copyright file="MetaCache.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;
    using Meta.Extensions;
    using Services;

    public class MetaCache : IMetaCache
    {
        // TODO: Use EmptySet
        private static readonly IReadOnlySet<Interface> EmptyInterfaceSet = new HashSet<Interface>();
        private static readonly IReadOnlySet<AssociationType> EmptyAssociationTypeSet = new HashSet<AssociationType>();
        private static readonly IReadOnlySet<RoleType> EmptyRoleTypeSet = new HashSet<RoleType>();
        private static readonly IReadOnlySet<CompositeRoleType> EmptyCompositeRoleTypeSet = new HashSet<CompositeRoleType>();

        private readonly IDictionary<Composite, IReadOnlySet<Interface>> supertypesByComposite;
        private readonly IDictionary<Composite, IReadOnlySet<AssociationType>> associationTypesByComposite;
        private readonly IDictionary<Composite, IReadOnlySet<RoleType>> roleTypesByComposite;
        private readonly IDictionary<Composite, IReadOnlySet<RoleType>> requiredRoleTypesByComposite;
        private readonly IDictionary<Class, IReadOnlySet<CompositeRoleType>> requiredCompositeRoleTypesByClass;
        private readonly IDictionary<Class, Type> builderTypeByClass;
        private readonly IDictionary<string, IReadOnlySet<Class>> classesByWorkspaceName;
        private readonly IDictionary<string, IDictionary<Class, IReadOnlySet<RoleType>>> roleTypesByClassByWorkspaceName;

        public MetaCache(IDatabase database)
        {
            var metaPopulation = database.MetaPopulation;
            var assembly = database.ObjectFactory.Assembly;

            this.supertypesByComposite = metaPopulation.Composites
                .ToDictionary(v => v, v => (IReadOnlySet<Interface>)new HashSet<Interface>(v.Supertypes));

            this.associationTypesByComposite = metaPopulation.Composites
                .ToDictionary(v => v, v => (IReadOnlySet<AssociationType>)new HashSet<AssociationType>(v.AssociationTypes));

            this.roleTypesByComposite = metaPopulation.Composites
                .ToDictionary(v => v, v => (IReadOnlySet<RoleType>)new HashSet<RoleType>(v.RoleTypes));

            this.requiredRoleTypesByComposite = metaPopulation.Composites
                .ToDictionary(v => v, v => (IReadOnlySet<RoleType>)new HashSet<RoleType>(v.RoleTypes.Where(w => w.CompositeRoleType.IsRequired())));

            this.requiredCompositeRoleTypesByClass = metaPopulation.Classes
                .ToDictionary(v => v, v => (IReadOnlySet<CompositeRoleType>)new HashSet<CompositeRoleType>(v.CompositeRoleTypeByRoleType.Values.Where(w => w.IsRequired())));

            this.builderTypeByClass = metaPopulation.Classes.
                ToDictionary(
                    v => v,
                    v => assembly.GetType($"Allors.Database.Domain.{v.SingularName}Builder", false));

            this.classesByWorkspaceName = new Dictionary<string, IReadOnlySet<Class>>();
            this.roleTypesByClassByWorkspaceName = new Dictionary<string, IDictionary<Class, IReadOnlySet<RoleType>>>();

            foreach (var workspaceName in metaPopulation.WorkspaceNames)
            {
                IReadOnlySet<Class> classes = new HashSet<Class>(metaPopulation.Classes.Where(w => w.WorkspaceNames.Contains(workspaceName)));
                this.classesByWorkspaceName[workspaceName] = classes;

                Dictionary<Class, IReadOnlySet<RoleType>> roleTypesByClass = new Dictionary<Class, IReadOnlySet<RoleType>>();
                foreach (var @class in classes)
                {
                    var roleTypes = new HashSet<RoleType>(@class.RoleTypes.Where(v => v.RelationType.WorkspaceNames.Contains(workspaceName)));
                    roleTypesByClass[@class] = roleTypes;
                }

                this.roleTypesByClassByWorkspaceName[workspaceName] = roleTypesByClass;
            }
        }

        public Type GetBuilderType(Class @class) => this.builderTypeByClass[@class];

        public IReadOnlySet<Interface> GetSupertypesByComposite(Composite composite)
        {
            return this.supertypesByComposite.TryGetValue(composite, out var supertype) ? supertype : EmptyInterfaceSet;
        }

        public IReadOnlySet<AssociationType> GetAssociationTypesByComposite(Composite composite)
        {
            return this.associationTypesByComposite.TryGetValue(composite, out var associationTypes) ? associationTypes : EmptyAssociationTypeSet;
        }

        public IReadOnlySet<RoleType> GetRoleTypesByComposite(Composite composite)
        {
            return this.roleTypesByComposite.TryGetValue(composite, out var roleTypes) ? roleTypes : EmptyRoleTypeSet;
        }

        public IReadOnlySet<RoleType> GetRequiredRoleTypesByComposite(Composite composite)
        {
            return this.requiredRoleTypesByComposite.TryGetValue(composite, out var requiredRoleTypes) ? requiredRoleTypes : EmptyRoleTypeSet;
        }

        public IReadOnlySet<CompositeRoleType> GetRequiredCompositeRoleTypesByClass(Class @class)
        {
            return this.requiredCompositeRoleTypesByClass.TryGetValue(@class, out var requiredCompositeRoleTypes) ? requiredCompositeRoleTypes : EmptyCompositeRoleTypeSet;
        }

        public IReadOnlySet<Class> GetWorkspaceClasses(string workspaceName)
        {
            this.classesByWorkspaceName.TryGetValue(workspaceName, out var classes);
            return classes;
        }

        public IDictionary<Class, IReadOnlySet<RoleType>> GetWorkspaceRoleTypesByClass(string workspaceName)
        {
            this.roleTypesByClassByWorkspaceName.TryGetValue(workspaceName, out var rolesByClass);
            return rolesByClass;
        }
    }
}
