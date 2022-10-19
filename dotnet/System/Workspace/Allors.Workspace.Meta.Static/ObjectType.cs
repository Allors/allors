// <copyright file="IClass.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    using System;
    using Allors.Text;

    public abstract class ObjectType : DataType, IObjectType
    {
        protected ObjectType(MetaPopulation metaPopulation, string tag, string singularName, string assignedPluralName)
            : base(metaPopulation, tag)
        {
            this.SingularName = singularName;
            this.AssignedPluralName = !string.IsNullOrEmpty(assignedPluralName) ? assignedPluralName : null;
            this.PluralName = this.ExistAssignedPluralName ? this.AssignedPluralName : Pluralizer.Pluralize(this.SingularName);
        }

        public string SingularName { get; }

        public bool ExistAssignedPluralName => this.AssignedPluralName != null;

        public string AssignedPluralName { get; }

        public string PluralName { get; }

        public abstract Type ClrType { get; set; }

        public bool IsUnit => this is IUnit;

        public bool IsComposite => this is IComposite;

        public bool IsInterface => this is IInterface;

        public bool IsClass => this is IClass;

        public override bool Equals(object other) => this.Tag.Equals((other as IMetaIdentifiableObject)?.Tag);

        public override int GetHashCode() => this.Tag.GetHashCode();

        public int CompareTo(IObjectType other)
        {
            return string.Compare(this.Tag, other?.Tag, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.SingularName))
            {
                return this.SingularName;
            }

            return this.Tag;
        }
    }
}
