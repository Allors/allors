// <copyright file="IRelationEndType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using Embedded.Meta;

/// <summary>
///     A <see cref="OperandType" /> can be a <see cref="AssociationType" /> or a <see cref="RoleType" />.
/// </summary>
public abstract class OperandType : EmbeddedObject, IMetaObject
{
    protected OperandType(MetaPopulation metaPopulation, EmbeddedObjectType embeddedObjectType)
        : base(metaPopulation, embeddedObjectType)
    {
        this.MetaPopulation = metaPopulation;
    }

    public MetaPopulation MetaPopulation { get; }
    
    public abstract void Validate(ValidationLog validationLog);

    public override bool Equals(object obj)
    {
        if (obj is not OperandType other)
        {
            return false;
        }

        if (this.EmbeddedPopulation != other.EmbeddedPopulation)
        {
            throw new ArgumentException("Object is from another meta population");
        }

        return this == other;
    }
}
