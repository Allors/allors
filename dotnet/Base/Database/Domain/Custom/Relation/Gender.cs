// <copyright file="Genders.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    
    public partial class Gender
    {
        public static readonly Guid MaleId = new Guid("DAB59C10-0D45-4478-A802-3ABE54308CCD");
        public static readonly Guid FemaleId = new Guid("B68704AD-82F1-4d5d-BBAF-A54635B5034F");
        public static readonly Guid OtherId = new Guid("09210D7C-804B-4E76-AD91-0E150D36E86E");
        public static readonly Guid PreferNotToSayId = new Guid("AEE7F928-B36B-47AE-BB17-747F1D0A9D23");
    }
}
