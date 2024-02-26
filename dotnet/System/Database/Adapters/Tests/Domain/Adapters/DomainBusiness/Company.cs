// <copyright file="Company.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain;

public partial class Company
{
    public static Company Create(ITransaction transaction, string name)
    {
        return transaction.Build<Company>(v=>v.Name = name);
    }

    public static Company Create(ITransaction transaction, string name, int index)
    {
        return transaction.Build<Company>(v =>
        {
            v.Name = name;
            v.Index = index;
        });
    }

    public override string ToString() => this.Name;
}
