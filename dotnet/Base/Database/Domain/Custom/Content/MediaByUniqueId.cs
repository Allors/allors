// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class MediaByUniqueId
    {
        public Media Avatar => this.cache[Media.AvatarId];

        public Media Madeliefje => this.cache[Media.MadeliefjeId];

        public Media About => this.cache[Media.AboutId];
    }
}
