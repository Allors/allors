﻿// <copyright file="S1Extensions.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Person type.</summary>

namespace Allors.Database.Domain
{
    public static partial class S1Extensions
    {
        public static void BaseSuperinterfaceMethod(this S1 @this, S1SuperinterfaceMethod method) => method.Value += "S1Base";

        public static void CustomSuperinterfaceMethod(this S1 @this, S1SuperinterfaceMethod method) => method.Value += "S1Custom";
    }
}
