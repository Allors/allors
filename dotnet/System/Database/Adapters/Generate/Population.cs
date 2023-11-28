// <copyright file="Program.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta.Generation.Storage;

using System.Collections.Generic;
using Allors.Database.Meta;
using Database.Population;

public class Population : IPopulation
{
    public IDictionary<IClass, IObject> ObjectsByClass { get; } = new Dictionary<IClass, IObject>();
}
