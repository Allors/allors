// <copyright file="DerivationErrorGeneric.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration.Derivations.Default
{
    using System;
    using Allors.Database.Derivations;
    using Allors.Database.Meta;

    public class DerivationErrorGeneric : DerivationError, IDerivationErrorGeneric
    {
        public DerivationErrorGeneric(IValidation validation, IDerivationRelation[] relations, string errorCode)
            : base(validation, relations, errorCode)
        {
        }

        public DerivationErrorGeneric(IValidation validation, IDerivationRelation relation, string errorCode)
            : this(validation, relation != null ? [relation] : Array.Empty<IDerivationRelation>(), errorCode)
        {
        }

        public DerivationErrorGeneric(IValidation validation, IObject association, IRoleType roleType, string message)
            : this(validation, new DerivationRelation(association, roleType), message)
        {
        }
    }
}
