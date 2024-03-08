// <copyright file="ExtentT.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ExtentT type.</summary>

namespace Allors.Database;

public interface IFilter<out T> : IExtent<T>, ICompositePredicate where T : class, IObject
{
}
