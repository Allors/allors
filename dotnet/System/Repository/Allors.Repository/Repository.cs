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
            this.UnitBySingularName = new Dictionary<string, Unit>();
            this.InterfaceBySingularName = new Dictionary<string, Interface>();
            this.ClassBySingularName = new Dictionary<string, Class>();
            this.CompositeByName = new Dictionary<string, Composite>();
            this.TypeBySingularName = new Dictionary<string, Type>();
            this.RecordByName = new Dictionary<string, Record>();
        }
        public Dictionary<string, Domain> DomainByName { get; }

        public Dictionary<string, Unit> UnitBySingularName { get; }

        public Dictionary<string, Interface> InterfaceBySingularName { get; }

        public Dictionary<string, Class> ClassBySingularName { get; }

        public Dictionary<string, Type> TypeBySingularName { get; }

        public Dictionary<string, Composite> CompositeByName { get; }

        public Dictionary<string, Record> RecordByName { get; }

        public Domain[] Domains => this.DomainByName.Values.ToArray();

        public Unit[] Units => this.UnitBySingularName.Values.ToArray();

        public Interface[] Interfaces => this.InterfaceBySingularName.Values.ToArray();

        public Class[] Classes => this.ClassBySingularName.Values.ToArray();

        public Type[] Types => this.Composites.Cast<Type>().Union(this.Units).ToArray();

        public Composite[] Composites => this.ClassBySingularName.Values.Cast<Composite>().Union(this.InterfaceBySingularName.Values).ToArray();

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
