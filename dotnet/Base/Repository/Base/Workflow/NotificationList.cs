// <copyright file="NotificationList.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("b6579993-4ff1-4853-b048-1f8e67419c00")]

#endregion

public partial class NotificationList : Deletable, Object
{
    #region Allors

    [Id("4516c5c1-73a0-4fdc-ac3c-aefaf417c8ba")]
    [SingleAssociation]
    [Indexed]

    #endregion

    public Notification[] Notifications { get; set; }

    #region Allors

    [Id("89487904-053e-470f-bcf9-0e01165b0143")]
    [SingleAssociation]
    [Indexed]

    #endregion

    [Derived]
    public Notification[] UnconfirmedNotifications { get; set; }

    #region Allors

    [Id("438fdc30-25ac-4d33-9a55-0ef817c05479")]
    [SingleAssociation]
    [Indexed]

    #endregion

    [Derived]
    public Notification[] ConfirmedNotifications { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }

    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild()
    {
    }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input)
    {
    }

    public void Delete()
    {
    }

    #endregion
}
