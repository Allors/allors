// <copyright file="IMetaPopulation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IStaticRoleType : IStaticRelationEndType, IRoleType
{
    new string SingularName { get; internal set; }

    new string PluralName { get; internal set; }

    new IStaticRelationType RelationType { get; internal set; }

    new IStaticAssociationType AssociationType { get; }

    new ICompositeRoleType CompositeRoleType { get; internal set; }

    internal void InitializeCompositeRoleTypes(Dictionary<IComposite, HashSet<ICompositeRoleType>> compositeRoleTypesByComposite);

    internal void DeriveScaleAndSize();

    internal void DeriveIsRequired();

    internal void DeriveIsUnique();
}
