// <copyright file="ISandbox.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors
[Id("7ba2ab26-491b-49eb-944c-26f6bb66e50f")]
#endregion
public interface ISandbox : Object
{
    #region Allors
    [Id("38361bff-62b3-4607-8291-cfdaeedbd36d")]
    [Size(256)]
    #endregion
    string InvisibleValue { get; set; }

    #region Allors
    [Id("796ab057-88a0-4d71-bc4a-2673a209161b")]
    #endregion
    ISandbox[] InvisibleManies { get; set; }

    #region Allors
    [Id("dba5deb2-880d-47f4-adae-0b3125ff1379")]
    [SingleAssociation]
    #endregion
    ISandbox InvisibleOne { get; set; }
}
