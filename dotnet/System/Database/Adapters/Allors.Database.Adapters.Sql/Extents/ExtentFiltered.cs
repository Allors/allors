// <copyright file="ExtentFiltered.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;

internal class ExtentFiltered<T> : Extent<T>, IInternalExtentFiltered, IFilter<T> where T : class, IObject
{
    private readonly Composite objectType;
    private readonly Transaction transaction;

    private And filter;

    public ExtentFiltered(Transaction transaction, Composite objectType)
    {
        this.transaction = transaction;
        this.objectType = objectType;
    }

    public override ICompositePredicate Predicate
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

    ICompositePredicate ICompositePredicate.AddAnd(Action<ICompositePredicate> init)
    {
        return this.Predicate.AddAnd(init);
    }

    IPredicate ICompositePredicate.AddBetween(RoleType role, object firstValue, object secondValue)
    {
        return this.Predicate.AddBetween(role, firstValue, secondValue);
    }

    IPredicate ICompositePredicate.AddWithin(RoleType role, IExtent<IObject> containingExtent)
    {
        return this.Predicate.AddWithin(role, containingExtent);
    }

    IPredicate ICompositePredicate.AddWithin(RoleType role, IEnumerable<IObject> containingEnumerable)
    {
        return this.Predicate.AddWithin(role, containingEnumerable);
    }

    IPredicate ICompositePredicate.AddWithin(AssociationType association, IExtent<IObject> containingExtent)
    {
        return this.Predicate.AddWithin(association, containingExtent);
    }

    IPredicate ICompositePredicate.AddWithin(AssociationType association, IEnumerable<IObject> containingEnumerable)
    {
        return this.Predicate.AddWithin(association, containingEnumerable);
    }

    IPredicate ICompositePredicate.AddContains(RoleType role, IObject containedObject)
    {
        return this.Predicate.AddContains(role, containedObject);
    }

    IPredicate ICompositePredicate.AddContains(AssociationType association, IObject containedObject)
    {
        return this.Predicate.AddContains(association, containedObject);
    }

    IPredicate ICompositePredicate.AddEquals(IObject allorsObject)
    {
        return this.Predicate.AddEquals(allorsObject);
    }

    IPredicate ICompositePredicate.AddEquals(RoleType roleType, object valueOrAllorsObject)
    {
        return this.Predicate.AddEquals(roleType, valueOrAllorsObject);
    }

    IPredicate ICompositePredicate.AddEquals(AssociationType association, IObject allorsObject)
    {
        return this.Predicate.AddEquals(association, allorsObject);
    }

    IPredicate ICompositePredicate.AddExists(RoleType role)
    {
        return this.Predicate.AddExists(role);
    }

    IPredicate ICompositePredicate.AddExists(AssociationType association)
    {
        return this.Predicate.AddExists(association);
    }

    IPredicate ICompositePredicate.AddGreaterThan(RoleType role, object value)
    {
        return this.Predicate.AddGreaterThan(role, value);
    }

    IPredicate ICompositePredicate.AddInstanceOf(Composite objectType)
    {
        return this.Predicate.AddInstanceOf(objectType);
    }

    IPredicate ICompositePredicate.AddInstanceOf(RoleType role, Composite objectType)
    {
        return this.Predicate.AddInstanceOf(role, objectType);
    }

    IPredicate ICompositePredicate.AddInstanceOf(AssociationType association, Composite objectType)
    {
        return this.Predicate.AddInstanceOf(association, objectType);
    }

    IPredicate ICompositePredicate.AddLessThan(RoleType role, object value)
    {
        return this.Predicate.AddLessThan(role, value);
    }

    IPredicate ICompositePredicate.AddLike(RoleType role, string value)
    {
        return this.Predicate.AddLike(role, value);
    }

    ICompositePredicate ICompositePredicate.AddNot(Action<ICompositePredicate> init)
    {
        return this.Predicate.AddNot(init);
    }

    ICompositePredicate ICompositePredicate.AddOr(Action<ICompositePredicate> init)
    {
        return this.Predicate.AddOr(init);
    }
}
