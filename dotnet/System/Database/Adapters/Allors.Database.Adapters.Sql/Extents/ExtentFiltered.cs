﻿// <copyright file="ExtentFiltered.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;

internal class ExtentFiltered<T> : Extent<T>, IInternalExtentFiltered where T : class, IObject
{
    private readonly Composite objectType;
    private readonly Transaction transaction;

    private And filter;

    internal ExtentFiltered(Transaction transaction, Strategy strategy, RoleType roleType)
        : this(transaction, (Composite)roleType.ObjectType)
    {
        this.Strategy = strategy;
        this.RoleType = roleType;
    }

    internal ExtentFiltered(Transaction transaction, Strategy strategy, AssociationType associationType)
        : this(transaction, associationType.Composite)
    {
        this.Strategy = strategy;
        this.AssociationType = associationType;
    }

    internal ExtentFiltered(Transaction transaction, Composite objectType)
    {
        this.transaction = transaction;
        this.objectType = objectType;
    }

    public override ICompositePredicate Filter
    {
        get
        {
            this.LazyLoadFilter();
            return this.filter;
        }
    }

    internal Mapping Mapping => this.transaction.Database.Mapping;

    public override Composite ObjectType => this.objectType;

    public override Transaction Transaction => this.transaction;

    public AssociationType AssociationType { get; private set; }

    public RoleType RoleType { get; private set; }

    public Strategy Strategy { get; private set; }

    public override string BuildSql(ExtentStatement statement)
    {
        this.LazyLoadFilter();
        this.filter.Setup(statement);

        if (this.objectType.Classes.Count > 0)
        {
            if (this.objectType.ExclusiveClass != null)
            {
                return this.BuildSqlWithExclusiveClass(statement);
            }

            return this.BuildSqlWithClasses(statement);
        }

        return null;
    }

    public void CheckAssociation(AssociationType associationType)
    {
        if (!this.objectType.AssociationTypes.Contains(associationType))
        {
            throw new ArgumentException("Extent does not have association " + associationType);
        }
    }

    public void CheckRole(RoleType roleType)
    {
        if (!this.objectType.RoleTypes.Contains(roleType))
        {
            throw new ArgumentException("Extent does not have role " + roleType.SingularName);
        }
    }

    protected override IList<long> GetObjectIds()
    {
        if (this.Strategy != null)
        {
            if (this.AssociationType != null)
            {
                return this.Strategy.ExtentGetCompositeAssociations(this.AssociationType);
            }

            return this.Strategy.GetCompositesRole(this.RoleType).ToList();
        }

        this.transaction.State.Flush();

        var statement = new ExtentStatementRoot(this);
        var objectIds = new List<long>();

        var alias = this.BuildSql(statement);

        using var command = statement.CreateDbCommand(alias);
        if (command == null)
        {
            return objectIds;
        }

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var objectId = this.transaction.State.GetObjectIdForExistingObject(reader.GetValue(0).ToString());
            objectIds.Add(objectId);
        }

        return objectIds;
    }

    /// <summary>
    ///     Lazy loads the filter.
    ///     Should also be used to upgrade from a strategy extent to a full extent.
    /// </summary>
    protected override void LazyLoadFilter()
    {
        if (this.filter == null)
        {
            this.filter = new And(this);
            this.Strategy = null;
            this.AssociationType = null;
            this.RoleType = null;
            this.FlushCache();
        }
    }

    private string BuildSqlWithExclusiveClass(ExtentStatement statement)
    {
        var alias = statement.CreateAlias();
        var rootClass = this.objectType.ExclusiveClass;

        if (statement.IsRoot)
        {
            statement.Append("SELECT DISTINCT " + alias + "." + Mapping.ColumnNameForObject);
            statement.Sorter?.BuildSelect(statement, alias);

            statement.Append(" FROM " + this.Mapping.TableNameForObjectByClass[rootClass] + " " + alias);
            statement.AddJoins(rootClass, alias);
            statement.AddWhere(rootClass, alias);
            this.filter?.BuildWhere(statement, alias);
        }
        else
        {
            // In
            var inStatement = (ExtentStatementChild)statement;

            if (inStatement.RoleType != null)
            {
                var inRole = inStatement.RoleType;
                var inIRelationType = inRole.RelationType;
                if (inIRelationType.Multiplicity == Multiplicity.ManyToMany || !inIRelationType.ExistExclusiveClasses)
                {
                    statement.Append("SELECT " + inRole.AssociationType.SingularFullName + "_A." + Mapping.ColumnNameForAssociation);
                }
                else if (inRole.IsMany)
                {
                    statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject);
                }
                else
                {
                    statement.Append("SELECT " + inRole.AssociationType.SingularFullName + "_A." +
                                     this.Mapping.ColumnNameByRelationType[inRole.RelationType]);
                }

                statement.Append(" FROM " + this.Mapping.TableNameForObjectByClass[rootClass] + " " + alias);
                statement.AddJoins(rootClass, alias);

                var wherePresent = statement.AddWhere(rootClass, alias);
                var filterUsed = false;
                if (this.filter != null)
                {
                    filterUsed = this.filter.BuildWhere(statement, alias);
                }

                if (wherePresent || filterUsed)
                {
                    statement.Append(" AND ");
                }
                else
                {
                    statement.Append(" WHERE ");
                }

                if (inIRelationType.Multiplicity == Multiplicity.ManyToMany || !inIRelationType.ExistExclusiveClasses)
                {
                    statement.Append(inRole.AssociationType.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " IS NOT NULL ");
                }
                else if (inRole.IsMany)
                {
                    statement.Append(alias + "." + this.Mapping.ColumnNameByRelationType[inRole.RelationType] + " IS NOT NULL ");
                }
                else
                {
                    statement.Append(inRole.AssociationType.SingularFullName + "_A." +
                                     this.Mapping.ColumnNameByRelationType[inRole.RelationType] + " IS NOT NULL ");
                }
            }
            else
            {
                if (statement.IsRoot)
                {
                    statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject);
                    if (statement.Sorter != null)
                    {
                        statement.Sorter.BuildSelect(statement);
                    }
                }
                else
                {
                    statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject);
                }

                statement.Append(" FROM " + this.Mapping.TableNameForObjectByClass[rootClass] + " " + alias);

                statement.AddJoins(rootClass, alias);
                statement.AddWhere(rootClass, alias);

                if (this.filter != null)
                {
                    this.filter.BuildWhere(statement, alias);
                }
            }
        }

        return alias;
    }

    private string BuildSqlWithClasses(ExtentStatement statement)
    {
        if (statement.IsRoot)
        {
            for (var i = 0; i < this.objectType.Classes.Count(); i++)
            {
                var alias = statement.CreateAlias();
                var rootClass = this.objectType.Classes.ToArray()[i];

                statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject);
                if (statement.Sorter != null)
                {
                    statement.Sorter.BuildSelect(statement);
                }

                statement.Append(" FROM " + this.Mapping.TableNameForObjectByClass[rootClass] + " " + alias);

                statement.AddJoins(rootClass, alias);
                statement.AddWhere(rootClass, alias);

                if (this.filter != null)
                {
                    this.filter.BuildWhere(statement, alias);
                }

                if (i < this.objectType.Classes.Count() - 1)
                {
                    statement.Append("\nUNION\n");
                }
            }
        }
        else
        {
            var inStatement = (ExtentStatementChild)statement;

            if (inStatement.RoleType != null)
            {
                var useUnion = false;
                foreach (var rootClass in this.objectType.Classes)
                {
                    var inRole = inStatement.RoleType;
                    var inIRelationType = inRole.RelationType;

                    if (!((Composite)inRole.ObjectType).Classes.Contains(rootClass))
                    {
                        continue;
                    }

                    if (useUnion)
                    {
                        statement.Append("\nUNION\n");
                    }
                    else
                    {
                        useUnion = true;
                    }

                    var alias = statement.CreateAlias();

                    if (inIRelationType.Multiplicity == Multiplicity.ManyToMany || !inIRelationType.ExistExclusiveClasses)
                    {
                        statement.Append("SELECT " + inRole.AssociationType.SingularFullName + "_A." + Mapping.ColumnNameForAssociation);
                    }
                    else if (inRole.IsMany)
                    {
                        statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject);
                    }
                    else
                    {
                        statement.Append("SELECT " + inRole.AssociationType.SingularFullName + "_A." +
                                         this.Mapping.ColumnNameByRelationType[inRole.RelationType]);
                    }

                    statement.Append(" FROM " + this.Mapping.TableNameForObjectByClass[rootClass] + " " + alias);

                    statement.AddJoins(rootClass, alias);

                    var wherePresent = statement.AddWhere(rootClass, alias);
                    var filterUsed = false;
                    if (this.filter != null)
                    {
                        filterUsed = this.filter.BuildWhere(statement, alias);
                    }

                    if (wherePresent || filterUsed)
                    {
                        statement.Append(" AND ");
                    }
                    else
                    {
                        statement.Append(" WHERE ");
                    }

                    if (inIRelationType.Multiplicity == Multiplicity.ManyToMany || !inIRelationType.ExistExclusiveClasses)
                    {
                        statement.Append(inRole.AssociationType.SingularFullName + "_A." + Mapping.ColumnNameForAssociation +
                                         " IS NOT NULL ");
                    }
                    else if (inRole.IsMany)
                    {
                        statement.Append(alias + "." + this.Mapping.ColumnNameByRelationType[inRole.RelationType] + " IS NOT NULL ");
                    }
                    else
                    {
                        statement.Append(inRole.AssociationType.SingularFullName + "_A." +
                                         this.Mapping.ColumnNameByRelationType[inRole.RelationType] + " IS NOT NULL ");
                    }
                }
            }
            else
            {
                for (var i = 0; i < this.objectType.Classes.Count(); i++)
                {
                    var alias = statement.CreateAlias();
                    var rootClass = this.objectType.Classes.ToArray()[i];

                    if (statement.IsRoot)
                    {
                        statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject);
                        statement.Sorter?.BuildSelect(statement);
                    }
                    else
                    {
                        statement.Append("SELECT " + alias + "." + Mapping.ColumnNameForObject);
                    }

                    statement.Append(" FROM " + this.Mapping.TableNameForObjectByClass[rootClass] + " " + alias);

                    statement.AddJoins(rootClass, alias);
                    statement.AddWhere(rootClass, alias);

                    this.filter?.BuildWhere(statement, alias);

                    if (i < this.objectType.Classes.Count() - 1)
                    {
                        statement.Append("\nUNION\n");
                    }
                }
            }
        }

        return null;
    }
}
