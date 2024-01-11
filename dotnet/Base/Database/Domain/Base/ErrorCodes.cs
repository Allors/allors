// <copyright file="Setup.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public static partial class ErrorCodes
    {
        public static readonly string InvalidPassword = "InvalidPassword";
        public static readonly string InvalidNewPassword = "InvalidNewPassword";

        public static readonly string DerivationErrorAtLeastOne = "DerivationErrorAtLeastOne";
        public static readonly string DerivationErrorAtMostOne = "DerivationErrorAtMostOne";
        public static readonly string DerivationErrorConflict = "DerivationErrorConflict";
        public static readonly string DerivationErrorEquals = "DerivationErrorEquals";
        public static readonly string DerivationErrorNotAllowed = "DerivationErrorNotAllowed";
        public static readonly string DerivationErrorRequired = "DerivationErrorRequired";
        public static readonly string DerivationErrorUnique = "DerivationErrorUnique";
    }
}
