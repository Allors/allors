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
    }
}
