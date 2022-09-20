// <copyright file="Task.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("84eb0e6e-68e1-478c-a35f-6036d45792be")]

#endregion

public interface Task : UniquelyIdentifiable, Deletable
{
    #region Allors

    [Id("f247de73-70fe-47e4-a763-22ee9c68a476")]
    [Indexed]

    #endregion

    [Derived]
    WorkItem WorkItem { get; set; }

    #region Allors

    [Id("0233714E-44A4-4363-8CC4-9D1C1DDD9BE5")]
    [Indexed]

    #endregion

    [Size(512)]
    [Derived]
    string Title { get; set; }

    #region Allors

    [Id("8ebd9048-a344-417c-bae7-359ca9a74aa1")]
    [Indexed]

    #endregion

    [Derived]
    DateTime DateCreated { get; set; }

    #region Allors

    [Id("54A7B60A-F855-4230-B340-5EC746DC5C7D")]
    [Indexed]

    #endregion

    [Derived]
    DateTime DateDue { get; set; }

    #region Allors

    [Id("5ad0b9f5-669c-4b05-8c97-89b59a227da2")]
    [Indexed]

    #endregion

    DateTime DateClosed { get; set; }

    #region Allors

    [Id("55375d57-34b0-43d0-9fac-e9788e1b6cd2")]
    [Indexed]

    #endregion

    [Derived]
    User[] Participants { get; set; }

    #region Allors

    [Id("ea8abc59-b625-4d25-85bd-dd04bfe55086")]
    [Indexed]

    #endregion

    [Derived]
    User Performer { get; set; }
}
