// <copyright file="MetaCache.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Collections;
    using Allors.Database.Meta;
    using Allors.Database.Meta.Extensions;
    using Allors.Database.Services;

    public class MetaCache : IMetaCache
    {
        // TODO: Use EmptySet
        private static readonly IReadOnlySet<IRoleType> EmptyRoleTypeSet = new HashSet<IRoleType>();
        private static readonly IReadOnlySet<ICompositeRoleType> EmptyCompositeRoleTypeSet = new HashSet<ICompositeRoleType>();

        private readonly IDictionary<IComposite, IReadOnlySet<IRoleType>> requiredRoleTypesByComposite;
        private readonly IDictionary<IClass, IReadOnlySet<ICompositeRoleType>> requiredCompositeRoleTypesByClass;
        private readonly IDictionary<IClass, Type> builderTypeByClass;
        private readonly IDictionary<string, IReadOnlySet<IClass>> classesByWorkspaceName;
        private readonly IDictionary<string, IDictionary<IClass, IReadOnlySet<IRoleType>>> roleTypesByClassByWorkspaceName;

        public MetaCache(IDatabase database)
        {
            var metaPopulation = (MetaPopulation)database.MetaPopulation;
            var assembly = database.ObjectFactory.Assembly;

            this.requiredRoleTypesByComposite = metaPopulation.Composites
                .ToDictionary(v => (IComposite)v, v => (IReadOnlySet<IRoleType>)new HashSet<IRoleType>(v.RoleTypes.Where(w => w.CompositeRoleType.IsRequired())));

            this.requiredCompositeRoleTypesByClass = metaPopulation.Classes
                .ToDictionary(v => (IClass)v, v => (IReadOnlySet<ICompositeRoleType>)new HashSet<ICompositeRoleType>(v.CompositeRoleTypeByRoleType.Values.Where(w => w.IsRequired())));

            this.builderTypeByClass = metaPopulation.Classes.
                ToDictionary(
                    v => (IClass)v,
                    v => assembly.GetType($"Allors.Database.Domain.{v.Name}Builder", false));

            this.classesByWorkspaceName = new Dictionary<string, IReadOnlySet<IClass>>();
            this.roleTypesByClassByWorkspaceName = new Dictionary<string, IDictionary<IClass, IReadOnlySet<IRoleType>>>();

            foreach (var workspaceName in metaPopulation.WorkspaceNames)
            {
                IReadOnlySet<IClass> classes = new HashSet<IClass>(metaPopulation.Classes.Where(w => w.WorkspaceNames.Contains(workspaceName)));
                this.classesByWorkspaceName[workspaceName] = classes;

                Dictionary<IClass, IReadOnlySet<IRoleType>> roleTypesByClass = new Dictionary<IClass, IReadOnlySet<IRoleType>>();
                foreach (var @class in classes)
                {
                    var roleTypes = new HashSet<IRoleType>(@class.RoleTypes.Where(v => v.RelationType.WorkspaceNames.Contains(workspaceName)));
                    roleTypesByClass[@class] = roleTypes;
                }

                this.roleTypesByClassByWorkspaceName[workspaceName] = roleTypesByClass;
            }
        }

        public Type GetBuilderType(IClass @class) => this.builderTypeByClass[@class];

        public IReadOnlySet<IRoleType> GetRequiredRoleTypesByComposite(IComposite composite)
        {
            return this.requiredRoleTypesByComposite.TryGetValue(composite, out var requiredRoleTypes) ? requiredRoleTypes : EmptyRoleTypeSet;
        }

        public IReadOnlySet<ICompositeRoleType> GetRequiredCompositeRoleTypesByClass(IClass @class)
        {
            return this.requiredCompositeRoleTypesByClass.TryGetValue(@class, out var requiredCompositeRoleTypes) ? requiredCompositeRoleTypes : EmptyCompositeRoleTypeSet;
        }

        public IReadOnlySet<IClass> GetWorkspaceClasses(string workspaceName)
        {
            this.classesByWorkspaceName.TryGetValue(workspaceName, out var classes);
            return classes;
        }

        public IDictionary<IClass, IReadOnlySet<IRoleType>> GetWorkspaceRoleTypesByClass(string workspaceName)
        {
            this.roleTypesByClassByWorkspaceName.TryGetValue(workspaceName, out var rolesByClass);
            return rolesByClass;
        }
    }
}
