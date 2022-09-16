// <copyright file="Addressable.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;
using static Workspaces;

#region Allors
[Id("FA760DB7-59FE-49C7-8198-6A08A2DFDEF9")]
#endregion
public interface Addressable : Object
{
    #region Allors
    [Id("5E8CA38B-F4FB-4BBF-8A60-4DDA8EA6EF0E")]
    [Indexed]
    #endregion
    [Derived]
    [Workspace(Default)]
    Address Address { get; set; }
}
