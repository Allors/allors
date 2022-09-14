// <copyright file="Repository.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>


namespace Allors.Repository.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public class Repository : RepositoryObject
    {
        public Repository() => this.Objects = new HashSet<RepositoryObject>();

        public ISet<RepositoryObject> Objects { get; }

        public Domain[] Domains => this.Objects.OfType<Domain>().ToArray();

        public StructuralType[] StructuralTypes => this.Objects.OfType<StructuralType>().ToArray();

        public Unit[] Units => this.Objects.OfType<Unit>().ToArray();

        public Interface[] Interfaces => this.Objects.OfType<Interface>().ToArray();

        public Class[] Classes => this.Objects.OfType<Class>().ToArray();

        public Composite[] Composites => this.Objects.OfType<Composite>().ToArray();

        public Domain[] SortedDomains
        {
            get
            {
                var assemblies = this.Domains.ToList();
                assemblies.Sort((x, y) => x.Base == y ? 1 : -1);
                return assemblies.ToArray();
            }
        }
    }
}
