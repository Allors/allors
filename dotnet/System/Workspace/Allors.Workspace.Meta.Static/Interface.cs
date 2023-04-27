// <copyright file="IInterface.cs" company="Allors bvba">
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

    public abstract class Interface : Composite, IInterface
    {
        protected Interface(MetaPopulation metaPopulation, string tag, IReadOnlyList<IInterface> directSupertypes, string singularName, string assignedPluralName)
            : base(metaPopulation, tag, directSupertypes, singularName, assignedPluralName)
        {
        }

        public override Type ClrType { get; set; }

        internal void InitializeDirectSubtypes()
        {
            this.DirectSubtypes = this.MetaPopulation.Composites.Where(v => v.DirectSupertypes.Contains(this)).ToArray();
        }

        public override IReadOnlyList<IComposite> DirectSubtypes { get; set; }

        public override IReadOnlyList<IComposite> Subtypes { get; set; }

        public override IReadOnlyList<IComposite> Composites { get; }

        public override IReadOnlyList<IClass> Classes { get; set; }

        public override IClass ExclusiveClass { get; }

        public override IReadOnlyList<IRoleType> ExclusiveRoleTypes { get; set; }

        public override IReadOnlyList<IAssociationType> ExclusiveAssociationTypes { get; set; }

        public override IReadOnlyList<IMethodType> ExclusiveMethodTypes { get; set; }

        public override bool IsAssignableFrom(IComposite objectType) => this.Equals(objectType) || this.Subtypes.Contains(objectType);

        public override void Bind(Dictionary<string, Type> typeByName) => this.ClrType = typeByName[this.SingularName];
    }
}
