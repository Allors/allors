// <copyright file="RoleType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Text;

    public abstract class RoleType : IRoleType, IComparable
    {
        /// <summary>
        /// The maximum size value.
        /// </summary>
        public const int MaximumSize = -1;
        private ObjectType objectType;

        private string singularName;
        private string pluralName;
        private int? precision;
        private int? scale;
        private int? size;
        private bool? isRequired;
        private bool? isUnique;

        protected RoleType(RelationType relationType)
        {
            this.MetaPopulation = relationType.MetaPopulation;
            this.RelationType = relationType;

            this.MetaPopulation.OnRoleTypeCreated(this);
        }

        IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;
        public MetaPopulation MetaPopulation { get; }

        IRelationType IRoleType.RelationType => this.RelationType;
        public RelationType RelationType { get; }

        IAssociationType IRoleType.AssociationType => this.AssociationType;
        public AssociationType AssociationType => this.RelationType.AssociationType;

        IObjectType IPropertyType.ObjectType => this.ObjectType;
        public ObjectType ObjectType
        {
            get => this.objectType;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.objectType = value;
                this.MetaPopulation.Stale();
            }
        }

        public IEnumerable<string> WorkspaceNames => this.RelationType.WorkspaceNames;

        public string[] AssignedWorkspaceNames => this.RelationType.AssignedWorkspaceNames;

        public string Name => this.IsMany ? this.PluralName : this.SingularName;

        public string FullName => this.IsMany ? this.PluralFullName : this.SingularFullName;

        public string SingularName
        {
            get => !string.IsNullOrEmpty(this.singularName) ? this.singularName : this.ObjectType.SingularName;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.singularName = value;
                this.MetaPopulation.Stale();
            }
        }

        public bool ExistAssignedSingularName => !this.SingularName.Equals(this.ObjectType.SingularName, StringComparison.Ordinal);

        /// <summary>
        /// Gets the full singular name.
        /// </summary>
        /// <value>The full singular name.</value>
        public string SingularFullName => this.RelationType.AssociationType.ObjectType + this.SingularName;

        public string PluralName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.pluralName))
                {
                    return this.pluralName;
                }

                if (!string.IsNullOrEmpty(this.singularName))
                {
                    return Pluralizer.Pluralize(this.singularName);
                }

                return this.ObjectType.PluralName;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.pluralName = value;
                this.MetaPopulation.Stale();
            }
        }

        public bool ExistAssignedPluralName => !this.PluralName.Equals(Pluralizer.Pluralize(this.SingularName), StringComparison.Ordinal);

        /// <summary>
        /// Gets the full plural name.
        /// </summary>
        /// <value>The full plural name.</value>
        public string PluralFullName => this.RelationType.AssociationType.ObjectType + this.PluralName;

        public bool IsMany =>
            this.RelationType.Multiplicity switch
            {
                Multiplicity.OneToMany => true,
                Multiplicity.ManyToMany => true,
                _ => false
            };

        /// <summary>
        /// Gets a value indicating whether this state has a multiplicity of one.
        /// </summary>
        /// <value><c>true</c> if this state is one; otherwise, <c>false</c>.</value>
        public bool IsOne => !this.IsMany;

        public int? Size
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.size;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.size = value;
                this.MetaPopulation.Stale();
            }
        }

        public int? Precision
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.precision;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.precision = value;
                this.MetaPopulation.Stale();
            }
        }

        public int? Scale
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.scale;
            }

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.scale = value;
                this.MetaPopulation.Stale();
            }
        }

        public bool IsRequired
        {
            get => this.isRequired ?? false;
            set => this.isRequired = value;
        }

        public bool IsUnique
        {
            get => this.isUnique ?? false;
            set => this.isUnique = value;
        }

        public string MediaType { get; set; }

        internal string ValidationName => "RoleType: " + this.RelationType.Name;
        public override bool Equals(object other) => this.RelationType.Id.Equals((other as RoleType)?.RelationType.Id);

        public override int GetHashCode() => this.RelationType.Id.GetHashCode();

        public int CompareTo(object other) => this.RelationType.Id.CompareTo((other as RoleType)?.RelationType.Id);

        public override string ToString() => $"{this.AssociationType.ObjectType.Name}.{this.Name}";

        /// <summary>
        /// Get the value of the role on this object.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <returns>
        /// The role value.
        /// </returns>
        public object Get(IStrategy strategy, IComposite ofType)
        {
            var role = strategy.GetRole(this);

            if (ofType == null || role == null || !this.ObjectType.IsComposite)
            {
                return role;
            }

            if (this.IsOne)
            {
                return ofType.IsAssignableFrom(((IObject)role).Strategy.Class) ? role : null;
            }

            var extent = (IEnumerable<IObject>)role;
            return extent.Where(v => ofType.IsAssignableFrom(v.Strategy.Class));
        }

        /// <summary>
        /// Set the value of the role on this object.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <param name="value">
        /// The role value.
        /// </param>
        public void Set(IStrategy strategy, object value) => strategy.SetRole(this, value);

        /// <summary>
        /// Derive multiplicity, scale and size.
        /// </summary>
        internal void DeriveScaleAndSize()
        {
            if (this.ObjectType is IUnit unitType)
            {
                switch (unitType.Tag)
                {
                    case UnitTags.String:
                        if (!this.Size.HasValue)
                        {
                            this.Size = 256;
                        }

                        this.Scale = null;
                        this.Precision = null;

                        break;

                    case UnitTags.Binary:
                        if (!this.Size.HasValue)
                        {
                            this.Size = MaximumSize;
                        }

                        this.Scale = null;
                        this.Precision = null;

                        break;

                    case UnitTags.Decimal:
                        if (!this.Precision.HasValue)
                        {
                            this.Precision = 19;
                        }

                        if (!this.Scale.HasValue)
                        {
                            this.Scale = 2;
                        }

                        this.Size = null;

                        break;

                    default:
                        this.Size = null;
                        this.Scale = null;
                        this.Precision = null;

                        break;
                }
            }
            else
            {
                this.Size = null;
                this.Scale = null;
                this.Precision = null;
            }
        }

        /// <summary>
        /// Validates the state.
        /// </summary>
        /// <param name="validationLog">The validation.</param>
        internal void Validate(ValidationLog validationLog)
        {
            if (this.ObjectType == null)
            {
                var message = this.ValidationName + " has no IObjectType";
                validationLog.AddError(message, this, ValidationKind.Required, "RoleType.IObjectType");
            }

            if (!string.IsNullOrEmpty(this.SingularName) && this.SingularName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned singular name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.SingularName");
            }

            if (!string.IsNullOrEmpty(this.PluralName) && this.PluralName.Length < 2)
            {
                var message = this.ValidationName + " should have an assigned plural role name with at least 2 characters";
                validationLog.AddError(message, this, ValidationKind.MinimumLength, "RoleType.PluralName");
            }
        }
    }
}
