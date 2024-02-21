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
    public abstract IDictionary<RelationType, string> ColumnNameByRelationType { get; }

    public abstract string TableNameForObjects { get; }
    public abstract IDictionary<IClass, string> TableNameForObjectByClass { get; }
    public abstract IDictionary<RelationType, string> TableNameForRelationByRelationType { get; }

    public abstract string ParamInvocationNameForObject { get; }
    public abstract string ParamInvocationNameForClass { get; }
    public abstract string ParamInvocationFormat { get; }
    public abstract IDictionary<IRoleType, string> ParamInvocationNameByRoleType { get; }

    public abstract IDictionary<IClass, string> ProcedureNameForDeleteObjectByClass { get; }
    public abstract IDictionary<IClass, string> ProcedureNameForCreateObjectsByClass { get; }
    public abstract IDictionary<IClass, string> ProcedureNameForGetUnitRolesByClass { get; }
    public abstract IDictionary<IClass, IDictionary<RelationType, string>> ProcedureNameForSetUnitRoleByRelationTypeByClass { get; }
    public abstract IDictionary<RelationType, string> ProcedureNameForGetRoleByRelationType { get; }
    public abstract IDictionary<RelationType, string> ProcedureNameForSetRoleByRelationType { get; }
    public abstract IDictionary<RelationType, string> ProcedureNameForAddRoleByRelationType { get; }
    public abstract IDictionary<RelationType, string> ProcedureNameForRemoveRoleByRelationType { get; }
    public abstract IDictionary<RelationType, string> ProcedureNameForClearRoleByRelationType { get; }
    public abstract IDictionary<RelationType, string> ProcedureNameForGetAssociationByRelationType { get; }
    public abstract IDictionary<IClass, string> ProcedureNameForCreateObjectByClass { get; }
    public abstract string ProcedureNameForInstantiate { get; }
    public abstract string ProcedureNameForGetVersion { get; }
    public abstract string ProcedureNameForUpdateVersion { get; }
    public abstract IDictionary<IClass, string> ProcedureNameForPrefetchUnitRolesByClass { get; }
    public abstract IDictionary<RelationType, string> ProcedureNameForPrefetchRoleByRelationType { get; }
    public abstract IDictionary<RelationType, string> ProcedureNameForPrefetchAssociationByRelationType { get; }

    public abstract string StringCollation { get; }
    public abstract string Ascending { get; }
    public abstract string Descending { get; }
}
