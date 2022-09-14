// <copyright file="Repository.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>


namespace Allors.Repository.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public class Repository
    {
        public Repository()
        {
            this.DomainByName = new Dictionary<string, Domain>();
            this.RecordByName = new Dictionary<string, Record>();
            this.StructuralTypeBySingularName = new Dictionary<string, StructuralType>();
        }

        public Dictionary<string, Domain> DomainByName { get; }

        public Dictionary<string, Record> RecordByName { get; }

        public Dictionary<string, StructuralType> StructuralTypeBySingularName { get; }

        public Domain[] Domains => this.DomainByName.Values.ToArray();

        public StructuralType[] StructuralTypes => this.StructuralTypeBySingularName.Values.ToArray();

        public Unit[] Units => this.StructuralTypes.OfType<Unit>().ToArray();

        public Interface[] Interfaces => this.StructuralTypes.OfType<Interface>().ToArray();

        public Class[] Classes => this.StructuralTypes.OfType<Class>().ToArray();

        public Composite[] Composites => this.StructuralTypes.OfType<Composite>().ToArray();

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
