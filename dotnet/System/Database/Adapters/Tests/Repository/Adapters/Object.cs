// <copyright file="Object.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors
[Id("12504f04-02c6-4778-98fe-04eba12ef8b2")]
#endregion
public partial interface Object
{
    #region Allors
    [Id("FDD32313-CF62-4166-9167-EF90BE3A3C75")]
    #endregion
    void OnBuild();

    #region Allors
    [Id("2B827E22-155D-4AA8-BA9F-46A64D7C79C8")]
    #endregion
    void OnPostBuild();
}
