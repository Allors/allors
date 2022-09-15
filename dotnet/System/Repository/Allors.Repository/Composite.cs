// <copyright file="Composite.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Inflector;
    using Text;

    public abstract class Composite : ObjectType
    {
        private readonly Inflector inflector;

        protected Composite(Inflector inflector, ISet<RepositoryObject> objects, Guid id, string name, Domain domain)
            : base(objects, id, name, domain)
        {
            this.inflector = inflector;
            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();
            this.ImplementedInterfaces = new List<Interface>();
            this.Properties = new HashSet<Property>();
            this.DefinedReverseProperties = new HashSet<Property>();
            this.InheritedReverseProperties = new HashSet<Property>();
            this.Methods = new HashSet<Method>();
        }

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }

        public XmlDoc XmlDoc { get; set; }

        public string PluralName
        {
            get
            {
                dynamic attribute = this.AttributeByName.Get("Plural");
                return attribute != null ? attribute.Value : this.inflector.Pluralize(this.SingularName);
            }
        }

        public string AssignedPluralName => !Pluralizer.Pluralize(this.SingularName).Equals(this.PluralName) ? this.PluralName : null;

        public abstract Interface[] Interfaces { get; }

        public IList<Interface> ImplementedInterfaces { get; }

        public ISet<Property> Properties { get; }

        public Property[] DefinedProperties => this.Properties.Where(v => v.DefiningProperty == null).ToArray();

        public Property[] InheritedProperties => this.Properties.Where(v => v.DefiningProperty != null).ToArray();

        public ISet<Property> DefinedReverseProperties { get; }

        public ISet<Property> InheritedReverseProperties { get; }

        public ISet<Method> Methods { get; }

        public Method[] DefinedMethods => this.Methods.Where(v => v.DefiningMethod == null).ToArray();

        public Method[] InheritedMethods => this.Methods.Where(v => v.DefiningMethod != null).ToArray();

        public Composite[] Subtypes { get; set; }

        public override string ToString() => this.SingularName;
    }
}
