// <copyright file="IMethodType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MethodType type.</summary>

namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IMethodType : IMetaIdentifiableObject, IOperandType
{
    ICompositeMethodType CompositeMethodType { get; }

    IReadOnlyDictionary<IComposite, ICompositeMethodType> CompositeMethodTypeByComposite { get; }

    IComposite ObjectType { get; }

    string Name { get; }
}
