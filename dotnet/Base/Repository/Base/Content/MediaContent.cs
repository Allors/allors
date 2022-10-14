// <copyright file="MediaContent.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// </copyright>


namespace Allors.Repository;

using Allors.Repository.Attributes;

#region Allors

[Id("6c20422e-cb3e-4402-bb40-dacaf584405e")]

#endregion

public class MediaContent : Deletable, Object
{
    #region Allors

    [Id("890598a9-0be4-49ee-8dd8-3581ee9355e6")]

    #endregion

    [Required]
    [Indexed]
    [Size(1024)]
    public string Type { get; set; }

    #region Allors

    [Id("0756d508-44b7-405e-bf92-bc09e5702e63")]

    #endregion

    [Required]
    [Size(-1)]
    public byte[] Data { get; set; }

    #region inherited

    public DelegatedAccess AccessDelegation { get; set; }
    public Revocation[] Revocations { get; set; }


    public SecurityToken[] SecurityTokens { get; set; }

    public void OnPostBuild() { }

    public void OnInit()
    {
    }

    public void OnPostDerive(OnPostDeriveInput input) { }

    public void Delete() { }

    #endregion
}
