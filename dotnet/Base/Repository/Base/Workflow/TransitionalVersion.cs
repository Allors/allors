// <copyright file="TransitionalVersion.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("A13C9057-8786-40CA-8421-476E55787D73")]

#endregion

public partial interface TransitionalVersion : Object
{
    #region Allors

    [Id("96685F17-ABE3-459C-BF9F-8C5F05788C04")]

    #endregion

    ObjectState[] PreviousObjectStates { get; set; }

    #region Allors

    [Id("39C43EB4-AA16-4CF8-93A0-60066CB746E8")]
    [Indexed]

    #endregion

    ObjectState[] LastObjectStates { get; set; }

    #region Allors

    [Id("F2472C1F-8D2A-4400-B372-34C2B03207B6")]
    [Indexed]

    #endregion

    ObjectState[] ObjectStates { get; set; }
}
