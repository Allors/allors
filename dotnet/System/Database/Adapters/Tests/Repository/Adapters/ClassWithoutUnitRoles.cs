// <copyright file="ClassWithoutUnitRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors
[Id("071d291d-fcc6-4511-8aa2-2d30fdeede8f")]
#endregion
[Plural("ClassWithoutUnitRoleses")]
public class ClassWithoutUnitRoles : Object
{
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
