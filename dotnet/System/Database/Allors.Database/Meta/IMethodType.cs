// <copyright file="IMethodType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MethodType type.</summary>

namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IMethodType : IMetaIdentifiableObject, IOperandType
{
    IReadOnlyList<string> AssignedWorkspaceNames { get; }

    ICompositeMethodType CompositeMethodType { get; }

    IReadOnlyDictionary<IComposite, ICompositeMethodType> CompositeMethodTypeByComposite { get; }

    IComposite ObjectType { get; }

    string Name { get; }

    IRecord Input { get; }

    IRecord Output { get; }
}
