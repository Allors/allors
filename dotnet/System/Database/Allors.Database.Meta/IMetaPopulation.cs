// <copyright file="IMetaPopulation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using Embedded;

public interface IMetaPopulation : IEmbeddedPopulation
{
    IReadOnlyList<IDomain> Domains { get; internal set; }

    IReadOnlyList<IUnit> Units { get; internal set; }

    IReadOnlyList<IComposite> Composites { get; internal set; }

    IReadOnlyList<IInterface> Interfaces { get; internal set; }

    IReadOnlyList<IClass> Classes { get; internal set; }

    IReadOnlyList<IRelationType> RelationTypes { get; internal set; }

    IReadOnlyList<IMethodType> MethodTypes { get; internal set; }

    IReadOnlyList<string> WorkspaceNames { get; }

    bool IsValid { get; }

    IMetaIdentifiableObject FindById(Guid metaObjectId);

    IMetaIdentifiableObject FindByTag(string tag);

    IComposite FindCompositeByName(string name);

    IValidationLog Validate();

    void Bind(Type[] types);

    T Create<T>(params Action<T>[] builders) where T : IEmbeddedObject;

    internal void OnCreated(IMetaIdentifiableObject metaObject);
}
