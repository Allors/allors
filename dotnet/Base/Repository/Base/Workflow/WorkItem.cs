// <copyright file="WorkItem.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("fbea29c6-6109-4163-a088-9f0b4deac896")]

#endregion

public partial interface WorkItem : Object
{
    #region Allors

    [Id("7e6d392b-00e7-4095-8525-d9f4ef8cfaa3")]
    [Derived]
    [Indexed]
    [Size(-1)]

    #endregion

    string WorkItemDescription { get; set; }
}
