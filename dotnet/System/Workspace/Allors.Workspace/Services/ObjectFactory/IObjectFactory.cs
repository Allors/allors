// <copyright file="ObjectFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using Meta;

    public interface IObjectFactory
    {
        IObjectType GetObjectTypeForObject<T>();

        IObjectType GetObjectTypeForObject(Type type);

        IObjectType GetObjectTypeForObject(string name);

        Type GetTypeForObject(IObjectType objectType);

        T Object<T>(IStrategy @object) where T : class, IObject;

        IEnumerable<T> Object<T>(IEnumerable<IStrategy> objects) where T : class, IObject;

        ICompositeRole CompositeRole(IStrategy strategy, IRoleType roleType);

        ICompositesRole CompositesRole(IStrategy strategy, IRoleType roleType);

        ICompositeAssociation CompositeAssociation(IStrategy strategy, IAssociationType associationType);

        ICompositesAssociation CompositesAssociation(IStrategy strategy, IAssociationType associationType);
    }
}
