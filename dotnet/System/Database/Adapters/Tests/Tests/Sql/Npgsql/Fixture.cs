// <copyright file="Profile.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql;

using System;
using global::Npgsql;

public class Fixture<T>
{
    private const string ConnectionStringKey = "ConnectionStrings:npgsql";

    static Fixture()
    {
        // TODO: replace timestamp with timestamp with time zone
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // TODO: https://www.npgsql.org/doc/release-notes/7.0.html#a-namecommandtypestoredprocedure-commandtypestoredprocedure-now-invokes-procedures-instead-of-functions
        AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
    }

    public Fixture()
    {
        this.Config = new Config();

        var connectionString = this.ConnectionStringBuilder.ConnectionString;

        int version;

        {
            // version 13+
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SHOW server_version";
            var scalar = command.ExecuteScalar();
            var full = scalar.ToString();
            var major = full.Substring(0, full.IndexOf("."));
            version = int.Parse(major);
            connection.Close();
        }
        
        {
            // version 13+
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            var withForce = version >= 13 ? "WITH (FORCE)" : string.Empty;
            command.CommandText = $"DROP DATABASE IF EXISTS {Database} {withForce}";
            command.ExecuteNonQuery();
            connection.Close();
        }

        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $"CREATE DATABASE {Database}";
            command.ExecuteNonQuery();
        }
    }

    private Config Config { get; set; }

    private NpgsqlConnectionStringBuilder ConnectionStringBuilder
    {
        get
        {
            var connectionString = this.Config.Root[ConnectionStringKey];
            var builder = new NpgsqlConnectionStringBuilder(connectionString)
            {
                Pooling = false,
                Enlist = false,
                CommandTimeout = 300
            };
            return builder;
        }
    }

    private string Database => typeof(T).Name.ToLowerInvariant();
}
