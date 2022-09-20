// <copyright file="Transitional.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("ab2179ad-9eac-4b61-8d84-81cd777c4926")]

#endregion

public interface Transitional : Object
{
    #region Allors

    [Id("D9D86241-5FC7-4EDB-9FAA-FF5CA291F16C")]
    [Indexed]

    #endregion

    [Derived]
    ObjectState[] PreviousObjectStates { get; set; }

    #region Allors

    [Id("2BC8AFDF-92BE-4088-9E35-C1C942CFE74B")]
    [Indexed]

    #endregion

    [Derived]
    ObjectState[] LastObjectStates { get; set; }

    #region Allors

    [Id("52962C45-8A3E-4136-A968-C333CBE12685")]
    [Indexed]

    #endregion

    [Derived]
    ObjectState[] ObjectStates { get; set; }

    #region Allors

    [Id("02cd3896-ea9a-498c-8633-42a5df9c0b17")]

    #endregion

    [Derived]
    [Indexed]
    Revocation[] TransitionalRevocations { get; set; }
}
