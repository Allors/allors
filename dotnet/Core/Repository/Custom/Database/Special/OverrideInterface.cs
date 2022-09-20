// <copyright file="OverrideInterface.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>

namespace Allors.Repository;

using Attributes;


#region Allors
[Id("45B34E4F-38DE-4E73-BF09-B53572CEF609")]
#endregion
public interface OverrideInterface : Object
{
    #region Allors
    [Id("6CB2E5CC-1EF8-47DA-A1C3-40423F2DAC68")]
    [Size(256)]
    
    #endregion

    public string OverrideRequired { get; set; }
}
