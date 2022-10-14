// <copyright file="IClass.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Text;

    public abstract class Class : IComposite
    {
        private HashSet<AssociationType> lazyAssociationTypes;
        private HashSet<RoleType> lazyDatabaseRoleTypes;
        private HashSet<MethodType> lazyMethodTypes;
        private HashSet<RoleType> lazyRoleTypes;

        private HashSet<AssociationType> LazyAssociationTypes => this.lazyAssociationTypes ??=
            new HashSet<AssociationType>(
                this.ExclusiveAssociationTypes.Union(this.Supertypes.SelectMany(v => v.ExclusiveAssociationTypes)));

        private HashSet<RoleType> LazyRoleTypes => this.lazyRoleTypes ??=
            new HashSet<RoleType>(this.ExclusiveRoleTypes.Union(this.Supertypes.SelectMany(v => v.ExclusiveRoleTypes)));

        private HashSet<RoleType> LazyDatabaseRoleTypes => this.lazyDatabaseRoleTypes ??= new HashSet<RoleType>(this.LazyRoleTypes);

        private HashSet<MethodType> LazyMethodTypes => this.lazyMethodTypes ??=
            new HashSet<MethodType>(this.ExclusiveMethodTypes.Union(this.Supertypes.SelectMany(v => v.ExclusiveMethodTypes)));

        public MetaPopulation MetaPopulation { get; set; }

        public string Tag { get; set; }
        public string SingularName { get; set; }
        public string PluralName { get; set; }
        public Type ClrType { get; set; }
        public ISet<Class> Classes { get; set; }

        public ISet<Interface> DirectSupertypes { get; set; }
        public ISet<Interface> Supertypes { get; set; }
        public RoleType[] ExclusiveRoleTypes { get; set; }
        public AssociationType[] ExclusiveAssociationTypes { get; set; }
        public MethodType[] ExclusiveMethodTypes { get; set; }

        int IComparable<IObjectType>.CompareTo(IObjectType other) =>
            string.Compare(this.SingularName, other.SingularName, StringComparison.InvariantCulture);

        public bool IsUnit => false;

        public bool IsComposite => true;

        public bool IsInterface => false;

        public bool IsClass => true;

        public ISet<AssociationType> AssociationTypes => this.LazyAssociationTypes;

        public ISet<RoleType> RoleTypes => this.LazyRoleTypes ??
                                           new HashSet<RoleType>(
                                               this.ExclusiveRoleTypes.Union(this.Supertypes.SelectMany(v => v.ExclusiveRoleTypes)));

        public ISet<RoleType> DatabaseOriginRoleTypes => this.LazyDatabaseRoleTypes;

        public ISet<MethodType> MethodTypes => this.LazyMethodTypes;

        public bool IsAssignableFrom(IComposite objectType) => this.Equals(objectType);

        public void Bind(Dictionary<string, Type> typeByTypeName) => this.ClrType = typeByTypeName[this.SingularName];

        public void Init(string tag, string singularName, string pluralName = null)
        {
            this.Tag = tag;
            this.SingularName = singularName;
            this.PluralName = pluralName ?? Pluralizer.Pluralize(singularName);
            this.Classes = new HashSet<Class> { this };
        }
    }
}
