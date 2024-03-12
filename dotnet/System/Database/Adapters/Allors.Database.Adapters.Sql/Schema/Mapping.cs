// <copyright file="Mapping.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System.Collections.Generic;
using Allors.Database.Meta;

public abstract class Mapping
{
    public const string ColumnNameForObject = "o";
    public const string ColumnNameForClass = "c";
    public const string ColumnNameForVersion = "v";
    public const string ColumnNameForAssociation = "a";
    public const string ColumnNameForRole = "r";
    public abstract IDictionary<RoleType, string> ColumnNameByRoleType { get; }

    public abstract string TableNameForObjects { get; }
    public abstract IDictionary<Class, string> TableNameForObjectByClass { get; }
    public abstract IDictionary<RoleType, string> TableNameForRelationByRoleType { get; }

    public abstract string ParamInvocationNameForObject { get; }
    public abstract string ParamInvocationNameForClass { get; }
    public abstract string ParamInvocationFormat { get; }
    public abstract IDictionary<RoleType, string> ParamInvocationNameByRoleType { get; }

    public abstract IDictionary<Class, string> ProcedureNameForDeleteObjectByClass { get; }
    public abstract IDictionary<Class, string> ProcedureNameForCreateObjectsByClass { get; }
    public abstract IDictionary<Class, string> ProcedureNameForGetUnitRolesByClass { get; }
    public abstract IDictionary<Class, IDictionary<RoleType, string>> ProcedureNameForSetUnitRoleByRoleTypeByClass { get; }
    public abstract IDictionary<RoleType, string> ProcedureNameForGetRoleByRoleType { get; }
    public abstract IDictionary<RoleType, string> ProcedureNameForSetRoleByRoleType { get; }
    public abstract IDictionary<RoleType, string> ProcedureNameForAddRoleByRoleType { get; }
    public abstract IDictionary<RoleType, string> ProcedureNameForRemoveRoleByRoleType { get; }
    public abstract IDictionary<RoleType, string> ProcedureNameForClearRoleByRoleType { get; }
    public abstract IDictionary<RoleType, string> ProcedureNameForGetAssociationByRoleType { get; }
    public abstract IDictionary<Class, string> ProcedureNameForCreateObjectByClass { get; }
    public abstract string ProcedureNameForInstantiate { get; }
    public abstract string ProcedureNameForGetVersion { get; }
    public abstract string ProcedureNameForUpdateVersion { get; }
    public abstract IDictionary<Class, string> ProcedureNameForPrefetchUnitRolesByClass { get; }
    public abstract IDictionary<RoleType, string> ProcedureNameForPrefetchRoleByRoleType { get; }
    public abstract IDictionary<RoleType, string> ProcedureNameForPrefetchAssociationByRoleType { get; }

    public abstract string StringCollation { get; }
    public abstract string Ascending { get; }
    public abstract string Descending { get; }
}
