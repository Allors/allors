﻿// <copyright file="DefaultDomainTransactionServices.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors.Database.Configuration
{
    using Database;

    public class TransactionServices : ITransactionServices
    {
        public void OnInit(ITransaction transaction) { }

        public void Dispose() { }

        public T Get<T>() => default;
    }
}
