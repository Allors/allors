// <copyright file="ExtentSort.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using Allors.Database.Meta;

public class ExtentSort
{
    private readonly SortDirection direction;
    private readonly Mapping mapping;
    private readonly RoleType roleType;
    private readonly Transaction transaction;
    private ExtentSort subSorter;

    internal ExtentSort(Transaction transaction, RoleType roleType, SortDirection direction)
    {
        this.transaction = transaction;
        this.roleType = roleType;
        this.direction = direction;
        this.mapping = this.transaction.Database.Mapping;
    }

    internal void AddSort(RoleType subSortIRoleType, SortDirection subSortDirection)
    {
        if (this.subSorter == null)
        {
            this.subSorter = new ExtentSort(this.transaction, subSortIRoleType, subSortDirection);
        }
        else
        {
            this.subSorter.AddSort(subSortIRoleType, subSortDirection);
        }
    }

    internal void BuildOrder(ExtentStatement statement)
    {
        statement.Append(
            statement.Sorter.Equals(this)
                ? $" ORDER BY {statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]}"
                : $" , {statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]}");

        statement.Append(this.direction == SortDirection.Ascending ? $" {this.mapping.Ascending} " : $" {this.mapping.Descending} ");

        this.subSorter?.BuildOrder(statement);
    }

    internal void BuildOrder(ExtentStatement statement, string alias)
    {
        statement.Append(
            statement.Sorter.Equals(this)
                ? $" ORDER BY {alias}.{statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]}"
                : $" , {alias}.{statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]}");

        statement.Append(this.direction == SortDirection.Ascending
            ? $" {this.mapping.Ascending} "
            : $" {this.mapping.Descending} ");

        this.subSorter?.BuildOrder(statement, alias);
    }

    internal void BuildSelect(ExtentStatement statement)
    {
        statement.Append($" , {statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]} ");
        this.subSorter?.BuildSelect(statement);
    }

    internal void BuildSelect(ExtentStatement statement, string alias)
    {
        statement.Append($" , {alias}.{statement.Mapping.ColumnNameByRelationType[this.roleType.RelationType]} ");
        this.subSorter?.BuildSelect(statement, alias);
    }
}
