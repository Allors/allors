// <copyright file="ILT32Unit.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;

#region Allors
[Id("228fa79f-afa7-418c-968e-8c0d38fb3ad2")]
#endregion
public interface ILT32Unit : Object
{
    #region Allors
    [Id("6822f677-7249-4c28-9b9c-18b21ba6f597")]
    [Size(256)]
    #endregion
    string AllorsString1 { get; set; }

    #region Allors
    [Id("b2734796-7140-4830-a0de-88df7d27b6a8")]
    [Size(256)]
    #endregion
    string AllorsString3 { get; set; }

    #region Allors
    [Id("ced16c48-6301-4652-8dcb-ed8a80ea7ce4")]
    [Size(256)]
    #endregion
    string AllorsString2 { get; set; }
}
