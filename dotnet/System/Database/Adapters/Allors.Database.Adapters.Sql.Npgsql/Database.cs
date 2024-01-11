// <copyright file="Database.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql;

using System;
using System.Xml;
using global::Npgsql;
using NpgsqlTypes;

public class Database : Sql.Database
{
    private readonly object lockObject = new();
    private IConnectionFactory connectionFactory;

    private bool? isValid;

    private IConnectionFactory managementConnectionFactory;

    private Mapping mapping;

    private string validationMessage;

    public Database(IDatabaseServices state, Configuration configuration) : base(state, configuration)
    {
        this.connectionFactory = configuration.ConnectionFactory;
        this.managementConnectionFactory = configuration.ManagementConnectionFactory;

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(this.ConnectionString);
        var applicationName = connectionStringBuilder.ApplicationName?.Trim();
        if (!string.IsNullOrWhiteSpace(applicationName))
        {
            this.Id = applicationName;
        }
        else if (!string.IsNullOrWhiteSpace(connectionStringBuilder.Database))
        {
            this.Id = connectionStringBuilder.Database.ToLowerInvariant();
        }
        else
        {
            using var connection = new NpgsqlConnection(this.ConnectionString);
            connection.Open();
            this.Id = connection.Database.ToLowerInvariant();
        }

        this.Services.OnInit(this);
    }

    public override IConnectionFactory ConnectionFactory
    {
        get => this.connectionFactory ??= new ConnectionFactory(this);

        set => this.connectionFactory = value;
    }

    public override IConnectionFactory ManagementConnectionFactory
    {
        get => this.managementConnectionFactory ??= new ConnectionFactory(this);

        set => this.managementConnectionFactory = value;
    }

    public override string Id { get; }

    public override bool IsValid
    {
        get
        {
            if (!this.isValid.HasValue)
            {
                lock (this.lockObject)
                {
                    if (!this.isValid.HasValue)
                    {
                        var validateResult = new Validation(this);
                        this.isValid = validateResult.IsValid;
                        this.validationMessage = validateResult.Message;
                    }
                }
            }

            return this.isValid.Value;
        }
    }

    public override Sql.Mapping Mapping
    {
        get
        {
            if (this.ObjectFactory.MetaPopulation != null && this.mapping == null)
            {
                this.mapping = new Mapping(this);
            }

            return this.mapping;
        }
    }

    public override string ValidationMessage => this.validationMessage;

    public override event ObjectNotRestoredEventHandler ObjectNotRestored;

    public override event RelationNotRestoredEventHandler RelationNotRestored;

    public override void Init()
    {
        try
        {
            new Initialization(this).Execute();
        }
        finally
        {
            this.mapping = null;
            this.Cache.Invalidate();
            this.Services.OnInit(this);
        }
    }

    public override void Restore(XmlReader reader)
    {
        lock (this)
        {
            this.Init();

            using (var connection = new NpgsqlConnection(this.ConnectionString))
            {
                try
                {
                    connection.Open();

                    var restore = new Restore(this, connection, this.ObjectNotRestored, this.RelationNotRestored);
                    restore.Execute(reader);

                    connection.Close();
                }
                catch (Exception e)
                {
                    try
                    {
                        connection.Close();
                    }
                    finally
                    {
                        this.Init();
                        throw e;
                    }
                }
            }
        }
    }

    public override void Backup(XmlWriter writer)
    {
        lock (this.lockObject)
        {
            var transaction = new ManagementTransaction(this, this.ManagementConnectionFactory);
            try
            {
                var backup = new Backup(this, writer);
                backup.Execute(transaction);
            }
            finally
            {
                transaction.Rollback();
            }
        }
    }

    internal string GetSqlType(NpgsqlDbType type) =>
        type switch
        {
            NpgsqlDbType.Varchar => "VARCHAR",
            NpgsqlDbType.Text => "TEXT",
            NpgsqlDbType.Integer => "INTEGER",
            NpgsqlDbType.Bigint => "BIGINT",
            NpgsqlDbType.Numeric => "NUMERIC",
            NpgsqlDbType.Double => "DOUBLE PRECISION",
            NpgsqlDbType.Boolean => "BOOLEAN",
            NpgsqlDbType.Date => "DATE",
            NpgsqlDbType.Timestamp => "TIMESTAMP",
            NpgsqlDbType.Uuid => "UUID",
            NpgsqlDbType.Bytea => "BYTEA",
            _ => "!UNKNOWN VALUE TYPE!",
        };
}
