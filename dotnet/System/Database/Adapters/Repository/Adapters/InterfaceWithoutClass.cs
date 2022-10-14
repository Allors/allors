// <copyright file="InterfaceWithoutClass.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors
[Id("2f4bc713-47c9-4e07-9f2b-1d22a0cb4fad")]
#endregion
public interface InterfaceWithoutClass : Object
{
    #region Allors
    [Id("b490715d-e318-471b-bd37-1c1e12c0314e")]
    #endregion
    bool AllorsBoolean { get; set; }
}
