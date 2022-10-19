// <copyright file="Record.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    using System.Collections.Generic;

    public abstract class Record : DataType, IRecord
    {
        protected Record(MetaPopulation metaPopulation, string tag) 
            : base(metaPopulation, tag)
        {
        }

        public IReadOnlyList<IFieldType> FieldTypes { get; }
    }
}
