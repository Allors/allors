// <copyright file="IClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    public abstract class DataType : MetaIdentifiableObject, IDataType
    {
        protected DataType(MetaPopulation metaPopulation, string tag)
            : base(metaPopulation, tag)
        {
        }
    }
}
