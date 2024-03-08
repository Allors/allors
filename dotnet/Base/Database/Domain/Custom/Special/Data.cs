﻿// <copyright file="Four.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Linq;

    /// <summary>
    /// Shared.
    /// </summary>
    public partial class Data
    {
        public void CustomOnInit(ObjectOnInit method)
        {
            var singleton = this.strategy.Transaction.GetSingleton();

            if (!this.ExistAutocompleteDerivedFilter)
            {
                this.AutocompleteDerivedFilter ??= singleton.AutocompleteDefault;
            }

            if (!this.ExistSelectDerived)
            {
                this.SelectDerived ??= singleton.SelectDefault;
            }
        }

        public void CustomOnPostDerive(ObjectOnPostDerive method)
        {
            var people = this.strategy.Transaction.Filter<Person>();
            people.AddEquals(this.m.Person.FirstName, "John");
            var john = people.First();

            this.AutocompleteDerivedFilter ??= john;
            this.AutocompleteDerivedOptions ??= john;
            this.SelectDerived ??= john;
        }
    }
}
