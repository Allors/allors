// <copyright file="DefaultDatabaseScope.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors.Database.Configuration
{
    using System;
    using Domain;

    public class DatabaseServices : IDatabaseServices
    {
        public virtual void OnInit(IDatabase database)
        {
            this.M = new MetaIndex(database.MetaPopulation);
        }

        public ITransactionServices CreateTransactionServices() => new TransactionServices();

        public IMetaIndex M { get; private set; }

        public T Get<T>() where T : class =>
           typeof(T) switch
           {
               // System
               { } type when type == typeof(IMetaIndex) => (T)this.M,
               _ => throw new NotSupportedException($"Service {typeof(T)} not supported"),
           };

        public void Dispose() { }
    }
}
