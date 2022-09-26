// <copyright file="IMetaPopulation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Reflection;

public interface IMetaPopulation
{
    IDomain[] Domains { get; }

    IUnit[] Units { get; }

    IComposite[] Composites { get; }

    IInterface[] Interfaces { get; }

    IClass[] Classes { get; }

    IRelationType[] RelationTypes { get; }

    IMethodType[] MethodTypes { get; }

    IRecord[] Records { get; }

    IFieldType[] FieldTypes { get; }

    bool IsValid { get; }

    IMetaIdentifiableObject FindById(Guid metaObjectId);

    IMetaIdentifiableObject FindByTag(string tag);

    IComposite FindDatabaseCompositeByName(string name);

    IValidationLog Validate();

    void Bind(Type[] types, Dictionary<Type, MethodInfo[]> extensionMethodsByInterface);
}
