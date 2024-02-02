// <copyright file="Profile.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class Fixture<T> : FixtureBase<T>
{

    public Fixture()
    {
        var connectionString = this.Configuration[this.ConnectionStringCreateKey];
        
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"DROP DATABASE IF EXISTS {this.Database}";
        command.ExecuteNonQuery();
        command.CommandText = $"CREATE DATABASE {this.Database}";
        command.ExecuteNonQuery();
    }

    public string ConnectionStringCreateKey => "ConnectionStrings:sqlclient-create";

    public override string ConnectionStringKey => "ConnectionStrings:sqlclient";
}
