// <copyright file="ObjectState.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Attributes;


#region Allors
[Id("f991813f-3146-4431-96d0-554aa2186887")]
#endregion
public interface ObjectState : UniquelyIdentifiable
{
    #region Allors
    [Id("913C994F-15B0-40D2-AC4F-81E362B9142C")]
    #endregion
    [Indexed]
    Revocation ObjectRevocation { get; set; }

    #region Allors
    [Id("b86f9e42-fe10-4302-ab7c-6c6c7d357c39")]
    #endregion
    
    [Indexed]
    [Size(256)]
    string Name { get; set; }
}
