// <copyright file="TransactionExtension.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Linq;

    public static partial class ICachesExtensions
    {
        public static Singleton GetSingleton(this ITransaction @this)
        {
            var singletonId = @this.Database.Services.Get<ISingletonId>();

            var singleton = (Singleton)@this.Instantiate(singletonId.Id);
            if (singleton == null)
            {
                singleton = @this.Filter<Singleton>().FirstOrDefault();
                singletonId.Id = singleton?.Id ?? 0;
            }

            return singleton;
        }
    }
}
