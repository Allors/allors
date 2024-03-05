﻿// <copyright file="ObjectExtensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Linq;
    using Meta;

    public static partial class ExtentExtensions
    {
        public static T FindBy<T>(this Extent<T> @this, RoleType roleType, object value) where T: IObject
        {
            if (value == null)
            {
                return default(T);
            }

            @this.Filter.AddEquals(roleType, value);
            return @this.FirstOrDefault();
        }
    }
}
