﻿// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database.Derivations;
    using Derivations.Rules;
    using Meta;

    public class TransitionalDeniedPermissionRule : Rule<Transitional>
    {
        public TransitionalDeniedPermissionRule(IMetaIndex m) : base(m, new Guid("5affa463-9365-4916-89ef-cfc18d41b4fb")) =>
            this.Patterns =
            [
                new RolePattern<Transitional, Transitional>(m.Transitional.ObjectStates) ,
                new RolePattern<ObjectState, Transitional>(m.ObjectState.ObjectRevocation, v=>v.TransitionalsWhereObjectState),
            ];

        public override void Derive(ICycle cycle, IEnumerable<Transitional> matches)
        {
            var validation = cycle.Validation;

            foreach (var @this in matches)
            {
                @this.DeriveTransitionalDeniedPermission(validation);
            }
        }
    }

    public static class TransitionalDeniedPermissionRuleExtensions
    {
        public static void DeriveTransitionalDeniedPermission(this Transitional @this, IValidation validation)
        {
            @this.TransitionalRevocations = @this.ObjectStates.Select(v => v.ObjectRevocation).ToArray();
        }
    }
}
