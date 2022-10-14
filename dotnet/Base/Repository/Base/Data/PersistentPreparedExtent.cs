// <copyright file="PersistentPreparedExtent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using System;
using Allors.Repository.Attributes;

#region Allors

[Id("645A4F92-F1F1-41C7-BA76-53A1CC4D2A61")]

#endregion

public class PersistentPreparedExtent : UniquelyIdentifiable, Deletable
{
    #region Allors

    [Id("CEADE44E-AA67-4E77-83FC-2C6E141A89F6")]

    #endregion

    [Size(256)]
    public string Name { get; set; }

    #region Allors

    [Id("03B7FB15-970F-453D-B6AC-A50654775E5E")]

    #endregion

    [Size(-1)]
    public string Description { get; set; }

    #region Allors

    [Id("712367B5-85ED-4623-9AC9-C082A32D8889")]

    #endregion

    [Size(-1)]
    public string Content { get; set; }

    #region inherited

    public Guid UniqueId { get; set; }


    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild()
    {
    }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input)
    {
    }

    public void Delete()
    {
    }

    #endregion
}
