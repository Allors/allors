﻿// <copyright file="IMetaPopulation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System.Linq;
using Allors.Text;
using Embedded;

public class EmbeddedDerivations
{
    public EmbeddedDerivations(MetaPopulation meta)
    {
        meta.EmbeddedDerivationById[nameof(ObjectTypePluralNameDerivation)] = new ObjectTypePluralNameDerivation();
    }

    public class ObjectTypePluralNameDerivation : IEmbeddedDerivation
    {
        public void EmbeddedDerive(IEmbeddedChangeSet changeSet)
        {
            var singularNames = changeSet.EmbeddedChangedRoles<ObjectType>(nameof(ObjectType.SingularName));
            var assignedPluralNames = changeSet.EmbeddedChangedRoles<ObjectType>(nameof(ObjectType.AssignedPluralName));

            if (singularNames.Any() || assignedPluralNames.Any())
            {
                var objectTypes = singularNames
                    .Union(assignedPluralNames)
                    .Select(v => v.Key)
                    .OfType<ObjectType>()
                    .Distinct(); ;

                foreach (var @this in objectTypes)
                {
                    @this.PluralName = @this.AssignedPluralName ?? Pluralizer.Pluralize(@this.SingularName);
                }
            }
        }
    }
}