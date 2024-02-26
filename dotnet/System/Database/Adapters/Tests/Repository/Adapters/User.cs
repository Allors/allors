// <copyright file="User.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors
[Id("0d6bc154-112b-4a58-aa96-3b2a96f82523")]
#endregion
public class User : Object
{
    #region Allors
    [Id("1ffa3cb7-41f0-406a-a3a5-2f3a4c5ad59c")]
    [Indexed]
    #endregion
    public User[] Selects { get; set; }

    #region Allors
    [Id("bc6b71a8-2a66-4b57-9c86-ecf521b973ba")]
    [Size(256)]
    #endregion
    public string From { get; set; }

    #region inherited properties
    #endregion

    #region inherited methods
    public void OnBuild()
    {
    }

    public void OnPostBuild()
    {
    }

    #endregion
}
