// <copyright file="Profile.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient;

using Microsoft.Data.SqlClient;

public class Fixture<T>
{
    private const string ConnectionStringCreateKey = "ConnectionStrings:sqlclient-create";
    private const string ConnectionStringKey = "ConnectionStrings:sqlclient";

    public Fixture()
    {
        this.Config = new Config();

        var connectionString = this.Config.Root[ConnectionStringCreateKey];

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = $"DROP DATABASE IF EXISTS {this.Database}";
        command.ExecuteNonQuery();
        command.CommandText = $"CREATE DATABASE {this.Database}";
        command.ExecuteNonQuery();
    }

    private Config Config { get; }

    private SqlConnectionStringBuilder ConnectionStringBuilder
    {
        get
        {
            var connectionString = this.Config.Root[ConnectionStringKey];
            connectionString = connectionString.Replace("[database]", this.Database);
            return new SqlConnectionStringBuilder(connectionString);
        }
    }
    private string Database => typeof(T).Name.ToLowerInvariant();
}

