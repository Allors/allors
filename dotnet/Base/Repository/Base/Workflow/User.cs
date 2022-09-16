// <copyright file="User.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Attributes;
using static Workspaces;

public partial interface User
{
    #region Allors
    [Id("bed34563-4ed8-4c6b-88d2-b4199e521d74")]
    [Indexed]
    #endregion
    [SingleAssociation]
    [Workspace(Default)]
    NotificationList NotificationList { get; set; }
}
