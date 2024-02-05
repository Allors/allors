﻿// <copyright file="IMetaPopulation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;

public interface IStaticMetaPopulation : IMetaPopulation
{
    new IReadOnlyList<IStaticDomain> Domains { get; internal set; }

    new IReadOnlyList<IStaticUnit> Units { get; internal set; }

    new IReadOnlyList<IStaticComposite> Composites { get; internal set; }

    new IReadOnlyList<IStaticInterface> Interfaces { get; internal set; }

    new IReadOnlyList<IStaticClass> Classes { get; internal set; }

    new IReadOnlyList<IStaticRelationType> RelationTypes { get; internal set; }

    new IReadOnlyList<IStaticMethodType> MethodTypes { get; internal set; }

    void OnCreated(IMetaIdentifiableObject metaObject);
}
