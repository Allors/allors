// <copyright file="ObjectType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using Text;

    public abstract class ObjectType : FieldObjectType, IObjectType
    {
        protected ObjectType(MetaPopulation metaPopulation, Guid id, string tag = null) : base(metaPopulation, id, tag)
        {
        }

        public bool IsUnit => this is IUnit;

        public bool IsComposite => this is IComposite;

        public bool IsInterface => this is IInterface;

        public bool IsClass => this is IClass;
    }
}
