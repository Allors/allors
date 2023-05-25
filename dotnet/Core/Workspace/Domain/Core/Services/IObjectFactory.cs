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
        IObjectType GetObjectType<T>();

        IObjectType GetObjectType(Type type);

        IObjectType GetObjectType(string name);

        Type GetType(IObjectType objectType);

        T Instantiate<T>(IStrategy objects) where T : class, IObject;

        IEnumerable<T> Instantiate<T>(IEnumerable<IStrategy> objects) where T : class, IObject;
    }
}
