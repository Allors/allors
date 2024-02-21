// <copyright file="IMetaCache.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using Allors.Database.Meta;

namespace Allors.Database.Services;


public interface IMethodService
{
    Action<object, object>[] Get(IClass @class, MethodType methodType);
}
