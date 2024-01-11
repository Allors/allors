// <copyright file="AccessControl.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class UserGroup 
    {
        public static readonly Guid AdministratorsId = new Guid("CDC04209-683B-429C-BED2-440851F430DF");
        public static readonly Guid CreatorsId = new Guid("F0D8132B-79D6-4A30-A866-EF6E5C952761");
        public static readonly Guid GuestsId = new Guid("921C9FEE-63EE-4B1F-8D8A-D96292B2B1A3");
    }
}
