// <copyright file="Mapping.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;
using NpgsqlTypes;

public class Mapping : Sql.Mapping
{
    internal const string SqlTypeForClass = "uuid";
    internal const string SqlTypeForObject = "bigint";
    internal const string SqlTypeForVersion = "bigint";
    private const string SqlTypeForCount = "integer";

    internal const NpgsqlDbType NpgsqlDbTypeForClass = NpgsqlDbType.Uuid;
    internal const NpgsqlDbType NpgsqlDbTypeForObject = NpgsqlDbType.Bigint;
    internal const NpgsqlDbType NpgsqlDbTypeForCount = NpgsqlDbType.Integer;

    private const string ProcedurePrefixForInstantiate = "i";
    private const string ProcedurePrefixForGetVersion = "gv";
    private const string ProcedurePrefixForUpdateVersion = "uv";
    private const string ProcedurePrefixForCreateObject = "co_";
    private const string ProcedurePrefixForCreateObjects = "cos_";
    private const string ProcedurePrefixForDeleteObject = "do_";
    private const string ProcedurePrefixForRestore = "r_";
    private const string ProcedurePrefixForGetUnits = "gu_";
    private const string ProcedurePrefixForPrefetchUnits = "pu_";
    private const string ProcedurePrefixForGetRole = "gc_";
    private const string ProcedurePrefixForPrefetchRole = "pc_";
    private const string ProcedurePrefixForSetRole = "sc_";
    private const string ProcedurePrefixForClearRole = "cc_";
    private const string ProcedurePrefixForAddRole = "ac_";
    private const string ProcedurePrefixForRemoveRole = "rc_";
    private const string ProcedurePrefixForGetAssociation = "ga_";
    private const string ProcedurePrefixForPrefetchAssociation = "pa_";

    internal const string ParameterFormat = "p_{0}";
    private const string ParameterInvocationFormat = ":p_{0}";
    private readonly IDictionary<RoleType, string> columnNameByRoleType;
    private readonly IDictionary<RoleType, string> paramInvocationNameByRoleType;
    private readonly IDictionary<RoleType, string> procedureNameForAddRoleByRoleType;
    private readonly IDictionary<RoleType, string> procedureNameForClearRoleByRoleType;
    private readonly IDictionary<Class, string> procedureNameForCreateObjectByClass;
    private readonly IDictionary<Class, string> procedureNameForCreateObjectsByClass;
    private readonly IDictionary<Class, string> procedureNameForDeleteObjectByClass;
    private readonly IDictionary<RoleType, string> procedureNameForGetAssociationByRoleType;
    private readonly IDictionary<RoleType, string> procedureNameForGetRoleByRoleType;
    private readonly IDictionary<Class, string> procedureNameForGetUnitRolesByClass;
    private readonly IDictionary<RoleType, string> procedureNameForPrefetchAssociationByRoleType;
    private readonly IDictionary<RoleType, string> procedureNameForPrefetchRoleByRoleType;
    private readonly IDictionary<Class, string> procedureNameForPrefetchUnitRolesByClass;
    private readonly IDictionary<RoleType, string> procedureNameForRemoveRoleByRoleType;
    private readonly IDictionary<RoleType, string> procedureNameForSetRoleByRoleType;
    private readonly IDictionary<Class, IDictionary<RoleType, string>> procedureNameForSetUnitRoleByRoleTypeByClass;
    private readonly IDictionary<Class, string> tableNameForObjectByClass;
    private readonly IDictionary<RoleType, string> tableNameForRelationByRoleType;

    public Mapping(Database database)
    {
        this.Database = database;

        this.ParamInvocationNameForObject = string.Format(ParameterInvocationFormat, ColumnNameForObject);
        this.ParamInvocationNameForClass = string.Format(ParameterInvocationFormat, ColumnNameForClass);

        this.ProcedureNameForInstantiate = this.Database.SchemaName + "." + ProcedurePrefixForInstantiate;
        this.ProcedureNameForGetVersion = this.Database.SchemaName + "." + ProcedurePrefixForGetVersion;
        this.ProcedureNameForUpdateVersion = this.Database.SchemaName + "." + ProcedurePrefixForUpdateVersion;

        this.ParamNameForAssociation = string.Format(ParameterFormat, ColumnNameForAssociation);
        this.ParamNameForCompositeRole = string.Format(ParameterFormat, ColumnNameForRole);
        this.ParamNameForCount = string.Format(ParameterFormat, "count");
        this.ParamNameForObject = string.Format(ParameterFormat, ColumnNameForObject);
        this.ParamNameForClass = string.Format(ParameterFormat, ColumnNameForClass);

        this.ParamInvocationNameForAssociation = string.Format(ParameterInvocationFormat, ColumnNameForAssociation);
        this.ParamInvocationNameForCompositeRole = string.Format(ParameterInvocationFormat, ColumnNameForRole);
        this.ParamInvocationNameForCount = string.Format(ParameterInvocationFormat, "count");

        this.ObjectArrayParam = new MappingArrayParameter(database, this, "arr_o", NpgsqlDbType.Bigint);
        this.CompositeRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Bigint);
        this.StringRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Varchar);
        this.StringMaxRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Text);
        this.IntegerRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Integer);
        this.DecimalRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Numeric);
        this.DoubleRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Double);
        this.BooleanRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Boolean);
        this.DateTimeRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Timestamp);
        this.UniqueRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Uuid);
        this.BinaryRoleArrayParam = new MappingArrayParameter(database, this, "arr_r", NpgsqlDbType.Bytea);

        // Tables
        // ------
        this.TableNameForObjects = database.SchemaName + "._o";
        this.tableNameForObjectByClass = new Dictionary<Class, string>();
        this.columnNameByRoleType = new Dictionary<RoleType, string>();
        this.paramInvocationNameByRoleType = new Dictionary<RoleType, string>();

        foreach (var @class in this.Database.MetaPopulation.Classes)
        {
            this.tableNameForObjectByClass.Add(@class, this.Database.SchemaName + "." + this.NormalizeName(@class.SingularName));

            foreach (var associationType in @class.AssociationTypes)
            {
                var roleType = associationType.RoleType;
                if (!(associationType.IsMany && roleType.IsMany) && roleType.ExistExclusiveClasses && roleType.IsMany)
                {
                    this.columnNameByRoleType[roleType] = this.NormalizeName(associationType.SingularName);
                }
            }

            foreach (var roleType in @class.RoleTypes)
            {
                var associationType = roleType.AssociationType;
                if (roleType.ObjectType.IsUnit)
                {
                    this.columnNameByRoleType[roleType] = this.NormalizeName(roleType.SingularName);
                    this.paramInvocationNameByRoleType[roleType] = string.Format(ParameterInvocationFormat, roleType.SingularFullName);
                }
                else if (!(associationType.IsMany && roleType.IsMany) && roleType.ExistExclusiveClasses && !roleType.IsMany)
                {
                    this.columnNameByRoleType[roleType] = this.NormalizeName(roleType.SingularName);
                }
            }
        }

        this.tableNameForRelationByRoleType = new Dictionary<RoleType, string>();

        foreach (var roleType in this.Database.MetaPopulation.RoleTypes)
        {
            var associationType = roleType.AssociationType;

            if (!roleType.ObjectType.IsUnit && ((associationType.IsMany && roleType.IsMany) || !roleType.ExistExclusiveClasses))
            {
                this.tableNameForRelationByRoleType.Add(roleType,
                    this.Database.SchemaName + "." + this.NormalizeName(roleType.SingularFullName));
            }
        }

        // Procedures
        // ----------
        this.ProcedureDefinitionByName = new Dictionary<string, string>();

        this.procedureNameForCreateObjectByClass = new Dictionary<Class, string>();
        this.procedureNameForCreateObjectsByClass = new Dictionary<Class, string>();
        this.procedureNameForDeleteObjectByClass = new Dictionary<Class, string>();

        this.procedureNameForGetUnitRolesByClass = new Dictionary<Class, string>();
        this.procedureNameForPrefetchUnitRolesByClass = new Dictionary<Class, string>();
        this.procedureNameForSetUnitRoleByRoleTypeByClass = new Dictionary<Class, IDictionary<RoleType, string>>();

        this.procedureNameForGetRoleByRoleType = new Dictionary<RoleType, string>();
        this.procedureNameForPrefetchRoleByRoleType = new Dictionary<RoleType, string>();
        this.procedureNameForSetRoleByRoleType = new Dictionary<RoleType, string>();
        this.procedureNameForAddRoleByRoleType = new Dictionary<RoleType, string>();
        this.procedureNameForRemoveRoleByRoleType = new Dictionary<RoleType, string>();
        this.procedureNameForClearRoleByRoleType = new Dictionary<RoleType, string>();
        this.procedureNameForGetAssociationByRoleType = new Dictionary<RoleType, string>();
        this.procedureNameForPrefetchAssociationByRoleType = new Dictionary<RoleType, string>();

        this.Instantiate();
        this.GetVersionIds();
        this.UpdateVersionIds();

        foreach (var @class in this.Database.MetaPopulation.Classes)
        {
            this.RestoreObjects(@class);
            this.CreateObject(@class);
            this.CreateObjects(@class);
            this.DeleteObject(@class);

            if (this.Database.GetSortedUnitRolesByObjectType(@class).Length > 0)
            {
                this.GetUnitRoles(@class);
                this.PrefetchUnitRoles(@class);
            }

            foreach (var associationType in @class.AssociationTypes)
            {
                if (!(associationType.IsMany && associationType.RoleType.IsMany) && associationType.RoleType.ExistExclusiveClasses &&
                    associationType.RoleType.IsMany)
                {
                    this.GetCompositesRoleObjectTable(@class, associationType);
                    this.PrefetchCompositesRoleObjectTable(@class, associationType);

                    if (associationType.IsOne)
                    {
                        this.GetCompositeAssociationObjectTable(@class, associationType);
                        this.PrefetchCompositeAssociationObjectTable(@class, associationType);
                    }

                    this.AddCompositeRoleObjectTable(@class, associationType);
                    this.RemoveCompositeRoleObjectTable(@class, associationType);
                    this.ClearCompositeRoleObjectTable(@class, associationType);
                }
            }

            foreach (var roleType in @class.RoleTypes)
            {
                if (roleType.ObjectType.IsUnit)
                {
                    this.SetUnitRoleType(@class, roleType);
                }
                else if (!(roleType.AssociationType.IsMany && roleType.IsMany) 
                         && roleType.ExistExclusiveClasses 
                         && roleType.IsOne)
                {
                    this.GetCompositeRoleObjectTable(@class, roleType);
                    this.PrefetchCompositeRoleObjectTable(@class, roleType);

                    if (roleType.AssociationType.IsOne)
                    {
                        this.GetCompositeAssociationOne2OneObjectTable(@class, roleType);
                        this.PrefetchCompositeAssociationObjectTable(@class, roleType);
                    }
                    else
                    {
                        this.GetCompositesAssociationMany2OneObjectTable(@class, roleType);
                        this.PrefetchCompositesAssociationMany2OneObjectTable(@class, roleType);
                    }

                    this.SetCompositeRole(@class, roleType);
                    this.ClearCompositeRole(@class, roleType);
                }
            }
        }

        foreach (var roleType in this.Database.MetaPopulation.RoleTypes)
        {
            if (!roleType.ObjectType.IsUnit && ((roleType.AssociationType.IsMany && roleType.IsMany) || !roleType.ExistExclusiveClasses))
            {
                this.procedureNameForPrefetchAssociationByRoleType.Add(roleType, this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + roleType.SingularFullName.ToLowerInvariant());
                this.procedureNameForClearRoleByRoleType.Add(roleType, this.Database.SchemaName + "." + ProcedurePrefixForClearRole + roleType.SingularFullName.ToLowerInvariant());

                if (roleType.IsMany)
                {
                    this.GetCompositesRoleRelationTable(roleType);
                    this.PrefetchCompositesRoleRelationTable(roleType);
                    this.AddCompositeRoleRelationTable(roleType);
                    this.RemoveCompositeRoleRelationTable(roleType);
                }
                else
                {
                    this.GetCompositeRoleRelationTable(roleType);
                    this.PrefetchCompositeRoleRelationType(roleType);
                    this.SetCompositeRoleRelationType(roleType);
                }

                if (roleType.AssociationType.IsOne)
                {
                    this.GetCompositeAssociationRelationTable(roleType);
                    this.PrefetchCompositeAssociationRelationTable(roleType);
                }
                else
                {
                    this.GetCompositesAssociationRelationTable(roleType);
                    this.PrefetchCompositesAssociationRelationTable(roleType);
                }

                this.ClearCompositeRoleRelationTable(roleType);
            }
        }
    }

    public override string ParamInvocationFormat => ParameterInvocationFormat;
    public override string ParamInvocationNameForObject { get; }
    public override string ParamInvocationNameForClass { get; }

    public override IDictionary<RoleType, string> ParamInvocationNameByRoleType => this.paramInvocationNameByRoleType;

    public override string TableNameForObjects { get; }
    public override IDictionary<Class, string> TableNameForObjectByClass => this.tableNameForObjectByClass;
    public override IDictionary<RoleType, string> ColumnNameByRoleType => this.columnNameByRoleType;
    public override IDictionary<RoleType, string> TableNameForRelationByRoleType => this.tableNameForRelationByRoleType;

    public override IDictionary<Class, string> ProcedureNameForCreateObjectByClass => this.procedureNameForCreateObjectByClass;
    public override IDictionary<Class, string> ProcedureNameForCreateObjectsByClass => this.procedureNameForCreateObjectsByClass;
    public override IDictionary<Class, string> ProcedureNameForDeleteObjectByClass => this.procedureNameForDeleteObjectByClass;
    public override IDictionary<Class, string> ProcedureNameForGetUnitRolesByClass => this.procedureNameForGetUnitRolesByClass;
    public override IDictionary<Class, string> ProcedureNameForPrefetchUnitRolesByClass => this.procedureNameForPrefetchUnitRolesByClass;

    public override IDictionary<Class, IDictionary<RoleType, string>> ProcedureNameForSetUnitRoleByRoleTypeByClass =>
        this.procedureNameForSetUnitRoleByRoleTypeByClass;

    public override IDictionary<RoleType, string> ProcedureNameForGetRoleByRoleType => this.procedureNameForGetRoleByRoleType;

    public override IDictionary<RoleType, string> ProcedureNameForPrefetchRoleByRoleType => this.procedureNameForPrefetchRoleByRoleType;

    public override IDictionary<RoleType, string> ProcedureNameForSetRoleByRoleType => this.procedureNameForSetRoleByRoleType;
    public override IDictionary<RoleType, string> ProcedureNameForAddRoleByRoleType => this.procedureNameForAddRoleByRoleType;

    public override IDictionary<RoleType, string> ProcedureNameForRemoveRoleByRoleType => this.procedureNameForRemoveRoleByRoleType;

    public override IDictionary<RoleType, string> ProcedureNameForClearRoleByRoleType => this.procedureNameForClearRoleByRoleType;

    public override IDictionary<RoleType, string> ProcedureNameForGetAssociationByRoleType => this.procedureNameForGetAssociationByRoleType;

    public override IDictionary<RoleType, string> ProcedureNameForPrefetchAssociationByRoleType => this.procedureNameForPrefetchAssociationByRoleType;

    public override string StringCollation => string.Empty;
    public override string Ascending => "ASC NULLS FIRST";
    public override string Descending => "DESC NULLS LAST";

    public override string ProcedureNameForInstantiate { get; }
    public override string ProcedureNameForGetVersion { get; }
    public override string ProcedureNameForUpdateVersion { get; }

    internal MappingArrayParameter ObjectArrayParam { get; }
    private MappingArrayParameter CompositeRoleArrayParam { get; }
    internal MappingArrayParameter StringRoleArrayParam { get; }
    private MappingArrayParameter StringMaxRoleArrayParam { get; }
    private MappingArrayParameter IntegerRoleArrayParam { get; }
    private MappingArrayParameter DecimalRoleArrayParam { get; }
    private MappingArrayParameter DoubleRoleArrayParam { get; }
    private MappingArrayParameter BooleanRoleArrayParam { get; }
    private MappingArrayParameter DateTimeRoleArrayParam { get; }
    private MappingArrayParameter UniqueRoleArrayParam { get; }
    private MappingArrayParameter BinaryRoleArrayParam { get; }

    private string ParamNameForAssociation { get; }
    private string ParamNameForCompositeRole { get; }
    private string ParamNameForCount { get; }
    private string ParamNameForObject { get; }
    private string ParamNameForClass { get; }

    internal string ParamInvocationNameForAssociation { get; }
    internal string ParamInvocationNameForCompositeRole { get; }
    internal string ParamInvocationNameForCount { get; }

    internal Dictionary<string, string> ProcedureDefinitionByName { get; }

    protected internal Database Database { get; }

    internal string NormalizeName(string name)
    {
        name = name.ToLowerInvariant();
        if (ReservedWords.Names.Contains(name))
        {
            return "\"" + name + "\"";
        }

        return name;
    }

    internal string GetSqlType(RoleType roleType)
    {
        var unit = (Unit)roleType.ObjectType;
        switch (unit.Tag)
        {
            case UnitTags.String:
                if (roleType.Size == -1 || roleType.Size > 4000)
                {
                    return "text";
                }

                return "varchar(" + roleType.Size + ")";

            case UnitTags.Integer:
                return "integer";

            case UnitTags.Decimal:
                return "numeric(" + roleType.Precision + "," + roleType.Scale + ")";

            case UnitTags.Float:
                return "double precision";

            case UnitTags.Boolean:
                return "boolean";

            case UnitTags.DateTime:
                return "timestamp";

            case UnitTags.Unique:
                return "uuid";

            case UnitTags.Binary:
                return "bytea";

            default:
                return "!UNKNOWN VALUE TYPE!";
        }
    }

    internal NpgsqlDbType GetNpgsqlDbType(RoleType roleType)
    {
        var unit = (Unit)roleType.ObjectType;
        return unit.Tag switch
        {
            UnitTags.String => NpgsqlDbType.Varchar,
            UnitTags.Integer => NpgsqlDbType.Integer,
            UnitTags.Decimal => NpgsqlDbType.Numeric,
            UnitTags.Float => NpgsqlDbType.Double,
            UnitTags.Boolean => NpgsqlDbType.Boolean,
            UnitTags.DateTime => NpgsqlDbType.Timestamp,
            UnitTags.Unique => NpgsqlDbType.Uuid,
            UnitTags.Binary => NpgsqlDbType.Bytea,
            _ => throw new Exception("Unknown Unit Type"),
        };
    }

    private void RestoreObjects(Class @class)
    {
        var table = this.tableNameForObjectByClass[@class];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForRestore + @class.SingularName.ToLowerInvariant();

        // Import Objects
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForClass},{this.ObjectArrayParam.TypeName});
CREATE FUNCTION {name}(
	{this.ParamNameForClass} {SqlTypeForClass},
	{this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS void
    LANGUAGE sql
AS $$
    INSERT INTO  {table} ({ColumnNameForClass}, {ColumnNameForObject})
    SELECT p_c, o
    FROM unnest(p_arr_o) AS t(o)
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void CreateObject(Class @class)
    {
        var table = this.tableNameForObjectByClass[@class];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForCreateObject + @class.SingularName.ToLowerInvariant();
        this.procedureNameForCreateObjectByClass.Add(@class, name);

        // CreateObject
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForClass});
CREATE FUNCTION {name}({this.ParamNameForClass} {SqlTypeForClass})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForObject} {SqlTypeForObject};
BEGIN

    INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
    VALUES ({this.ParamNameForClass}, {(long)Allors.Version.DatabaseInitial})
    RETURNING {ColumnNameForObject} INTO {this.ParamNameForObject};

    INSERT INTO {table} ({ColumnNameForObject},{ColumnNameForClass})
    VALUES ({this.ParamNameForObject},{this.ParamNameForClass});

    RETURN {this.ParamNameForObject};
END
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void CreateObjects(Class @class)
    {
        var name = this.Database.SchemaName + "." + ProcedurePrefixForCreateObjects + @class.SingularName.ToLowerInvariant();
        this.procedureNameForCreateObjectsByClass.Add(@class, name);

        // CreateObjects
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForClass}, {SqlTypeForCount});
CREATE FUNCTION {name}({this.ParamNameForClass} {SqlTypeForClass}, {this.ParamNameForCount} {SqlTypeForCount})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE ID integer;
DECLARE COUNTER integer := 0;
BEGIN
    WHILE COUNTER < {this.ParamNameForCount} LOOP

        INSERT INTO {this.TableNameForObjects} ({ColumnNameForClass}, {ColumnNameForVersion})
        VALUES ({this.ParamNameForClass}, {(long)Allors.Version.DatabaseInitial} )
        RETURNING {ColumnNameForObject} INTO ID;

        INSERT INTO {this.tableNameForObjectByClass[@class.ExclusiveClass]} ({ColumnNameForObject},{ColumnNameForClass})
        VALUES (ID,{this.ParamNameForClass});

        COUNTER := COUNTER+1;

        RETURN NEXT ID;
    END LOOP;
END
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void DeleteObject(Class @class)
    {
        var table = this.tableNameForObjectByClass[@class];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForDeleteObject + @class.SingularName.ToLowerInvariant();
        this.procedureNameForDeleteObjectByClass.Add(@class, name);

        var definition = $@"DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForObject} {SqlTypeForObject})
    RETURNS void
    LANGUAGE sql
AS $$

    DELETE FROM {this.TableNameForObjects}
    WHERE {ColumnNameForObject}={this.ParamNameForObject};

    DELETE FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForObject};
$$;
";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void GetUnitRoles(Class @class)
    {
        var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetUnits + @class.SingularName.ToLowerInvariant();
        this.procedureNameForGetUnitRolesByClass.Add(@class, name);

        // Get Unit Roles
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForObject} {SqlTypeForObject})
    RETURNS TABLE
    ({string.Join(", ", sortedUnitRoleTypes.Select(v => $"{this.columnNameByRoleType[v]} {this.GetSqlType(v)}"))})
    LANGUAGE sql
AS $$
    SELECT {string.Join(", ", sortedUnitRoleTypes.Select(v => this.columnNameByRoleType[v]))}
    FROM {this.tableNameForObjectByClass[@class.ExclusiveClass]}
    WHERE {ColumnNameForObject}={this.ParamNameForObject};
$$;";
        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void PrefetchUnitRoles(Class @class)
    {
        var sortedUnitRoleTypes = this.Database.GetSortedUnitRolesByObjectType(@class);
        var table = this.tableNameForObjectByClass[@class.ExclusiveClass];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchUnits + @class.SingularName.ToLowerInvariant();
        this.procedureNameForPrefetchUnitRolesByClass.Add(@class, name);

        var definition =
            $@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
        {ColumnNameForObject} {SqlTypeForObject},
        {string.Join(", ", sortedUnitRoleTypes.Select(v => $"{this.columnNameByRoleType[v]} {this.GetSqlType(v)}"))}
    )
LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {string.Join(", ", sortedUnitRoleTypes.Select(v => this.columnNameByRoleType[v]))}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void GetCompositesRoleObjectTable(Class @class, AssociationType associationType)
    {
        var roleType = associationType.RoleType;
        var table = this.tableNameForObjectByClass[@class];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForGetRoleByRoleType.Add(roleType, name);

        // Get Composites Role (1-*) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForAssociation} {SqlTypeForObject})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE sql
AS $$
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRoleType[roleType]}={this.ParamNameForAssociation};
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void PrefetchCompositesRoleObjectTable(Class @class, AssociationType associationType)
    {
        var roleType = associationType.RoleType;
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForPrefetchRoleByRoleType.Add(roleType, name);

        // Prefetch Composites Role (1-*) [object table]
        var definition =
            $@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {this.columnNameByRoleType[roleType]} {SqlTypeForObject},
         {ColumnNameForObject} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {this.columnNameByRoleType[roleType]}, {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRoleType[roleType]} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";
        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void GetCompositeAssociationObjectTable(Class @class, AssociationType associationType)
    {
        var roleType = associationType.RoleType;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + @class.SingularName.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
        var table = this.tableNameForObjectByClass[@class];
        this.procedureNameForGetAssociationByRoleType.Add(roleType, name);

        // Get Composite Association (1-*) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForAssociation} {SqlTypeForObject};
BEGIN
    SELECT {this.columnNameByRoleType[roleType]}
    FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForCompositeRole}
    INTO {this.ParamNameForAssociation};

    RETURN {this.ParamNameForAssociation};
END
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void PrefetchCompositeAssociationObjectTable(Class @class, AssociationType associationType)
    {
        var roleType = associationType.RoleType;
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + @class.SingularName.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForPrefetchAssociationByRoleType.Add(roleType, name);

        // Prefetch Composite Association (1-*) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {this.columnNameByRoleType[roleType]} {SqlTypeForObject},
         {ColumnNameForObject} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {this.columnNameByRoleType[roleType]}, {ColumnNameForObject}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void AddCompositeRoleObjectTable(Class @class, AssociationType associationType)
    {
        var roleType = associationType.RoleType;
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var roles = this.CompositeRoleArrayParam;
        var rolesType = roles.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForAddRole + @class.SingularName.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForAddRoleByRoleType.Add(roleType, name);

        // Add Composite Role (1-*) [object table]
        var definition =
            $@"DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.columnNameByRoleType[roleType]} = relations.{ColumnNameForAssociation}
    FROM relations
    WHERE {table}.{ColumnNameForObject} = relations.{ColumnNameForRole}
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void RemoveCompositeRoleObjectTable(Class @class, AssociationType associationType)
    {
        var roleType = associationType.RoleType;
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var roles = this.CompositeRoleArrayParam;
        var rolesType = roles.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + @class.SingularName.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForRemoveRoleByRoleType.Add(roleType, name);

        // Remove Composite Role (1-*) [object table]
        var definition =
            $@"DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.columnNameByRoleType[roleType]} = null
    FROM relations
    WHERE {table}.{this.columnNameByRoleType[roleType]} = relations.{ColumnNameForAssociation} AND
          {table}.{ColumnNameForObject} = relations.{ColumnNameForRole}
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void ClearCompositeRoleObjectTable(Class @class, AssociationType associationType)
    {
        var roleType = associationType.RoleType;
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForClearRole + @class.SingularName.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForClearRoleByRoleType.Add(roleType, name);

        // Clear Composites Role (1-*) [object table]
        var definition =
            $@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    UPDATE {table}
    SET {this.columnNameByRoleType[roleType]} = null
    FROM objects
    WHERE {table}.{this.columnNameByRoleType[roleType]} = objects.{ColumnNameForObject}
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void SetUnitRoleType(Class @class, RoleType roleType)
    {
        if (!this.procedureNameForSetUnitRoleByRoleTypeByClass.TryGetValue(@class, out var procedureNameForSetUnitRoleByRelationType))
        {
            procedureNameForSetUnitRoleByRelationType = new Dictionary<RoleType, string>();
            this.procedureNameForSetUnitRoleByRoleTypeByClass.Add(@class, procedureNameForSetUnitRoleByRelationType);
        }

        var unitTypeTag = ((Unit)roleType.ObjectType).Tag;
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForSetRole + @class.SingularName.ToLowerInvariant() + "_" +
                   roleType.SingularFullName.ToLowerInvariant();
        procedureNameForSetUnitRoleByRelationType.Add(roleType, name);

        var roles = unitTypeTag switch
        {
            UnitTags.String => this.StringMaxRoleArrayParam,
            UnitTags.Integer => this.IntegerRoleArrayParam,
            UnitTags.Float => this.DoubleRoleArrayParam,
            UnitTags.Decimal => this.DecimalRoleArrayParam,
            UnitTags.Boolean => this.BooleanRoleArrayParam,
            UnitTags.DateTime => this.DateTimeRoleArrayParam,
            UnitTags.Unique => this.UniqueRoleArrayParam,
            UnitTags.Binary => this.BinaryRoleArrayParam,
            _ => throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.SingularName),
        };

        var rolesType = roles.TypeName;

        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.columnNameByRoleType[roleType]} = relations.{ColumnNameForRole}
    FROM relations
    WHERE {ColumnNameForObject} = relations.{ColumnNameForAssociation}
$$;";
        this.ProcedureDefinitionByName.Add(procedureNameForSetUnitRoleByRelationType[roleType], definition);
    }

    private void GetCompositeRoleObjectTable(Class @class, RoleType roleType)
    {
        var table = this.tableNameForObjectByClass[@class];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForGetRoleByRoleType.Add(roleType, name);

        // Get Composite Role (1-1 and *-1) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForAssociation} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForCompositeRole} {SqlTypeForObject};
BEGIN
    SELECT {this.columnNameByRoleType[roleType]}
    FROM {table}
    WHERE {ColumnNameForObject}={this.ParamNameForAssociation}
    INTO {this.ParamNameForCompositeRole};

    RETURN {this.ParamNameForCompositeRole};
END
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void PrefetchCompositeRoleObjectTable(Class @class, RoleType roleType)
    {
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForPrefetchRoleByRoleType.Add(roleType, name);

        // Prefetch Composite Role (1-1 and *-1) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {this.columnNameByRoleType[roleType]} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {this.columnNameByRoleType[roleType]}
    FROM {table}
    WHERE {ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void GetCompositeAssociationOne2OneObjectTable(Class @class, RoleType roleType)
    {
        var table = this.tableNameForObjectByClass[@class];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + @class.SingularName.ToLowerInvariant() + "_" +
                   roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForGetAssociationByRoleType.Add(roleType, name);

        // Get Composite Association (1-1) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForAssociation} {SqlTypeForObject};
BEGIN
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRoleType[roleType]}={this.ParamNameForCompositeRole}
    INTO {this.ParamNameForAssociation};

    RETURN {this.ParamNameForAssociation};
END
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void PrefetchCompositeAssociationObjectTable(Class @class, RoleType roleType)
    {
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + @class.SingularName.ToLowerInvariant() + "_" +
                   roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForPrefetchAssociationByRoleType.Add(roleType, name);

        // Prefetch Composite Association (1-1) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForAssociation} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {this.columnNameByRoleType[roleType]}
    FROM {table}
    WHERE {this.columnNameByRoleType[roleType]} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void GetCompositesAssociationMany2OneObjectTable(Class @class, RoleType roleType)
    {
        var table = this.tableNameForObjectByClass[@class];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + @class.SingularName.ToLowerInvariant() + "_" + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForGetAssociationByRoleType.Add(roleType, name);

        // Get Composite Association (*-1) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE sql
AS $$
    SELECT {ColumnNameForObject}
    FROM {table}
    WHERE {this.columnNameByRoleType[roleType]}={this.ParamNameForCompositeRole};
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void PrefetchCompositesAssociationMany2OneObjectTable(Class @class, RoleType roleType)
    {
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchAssociation + @class.SingularName.ToLowerInvariant() + "_" +
                   roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForPrefetchAssociationByRoleType.Add(roleType, name);

        // Prefetch Composite Association (*-1) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForAssociation} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {this.columnNameByRoleType[roleType]}
    FROM {table}
    WHERE {this.columnNameByRoleType[roleType]} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void SetCompositeRole(Class @class, RoleType roleType)
    {
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var roles = this.CompositeRoleArrayParam;
        var rolesType = roles.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForSetRole + @class.SingularName.ToLowerInvariant() + "_" +
                   roleType.SingularFullName.ToLowerInvariant();

        // Set Composite Role (1-1 and *-1) [object table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    UPDATE {table}
    SET {this.columnNameByRoleType[roleType]} = relations.{ColumnNameForRole}
    FROM relations
    WHERE {ColumnNameForObject} = relations.{ColumnNameForAssociation}
$$;";

        this.procedureNameForSetRoleByRoleType.Add(roleType, name);
        this.ProcedureDefinitionByName.Add(this.procedureNameForSetRoleByRoleType[roleType], definition);
    }

    private void ClearCompositeRole(Class @class, RoleType roleType)
    {
        var table = this.tableNameForObjectByClass[@class];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForClearRole + @class.SingularName.ToLowerInvariant() + "_" +
                   roleType.SingularFullName.ToLowerInvariant();

        // Clear Composite Role (1-1 and *-1) [object table]
        var definition =
            $@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    UPDATE {table}
    SET {this.columnNameByRoleType[roleType]} = null
    FROM objects
    WHERE {table}.{ColumnNameForObject} = objects.{ColumnNameForObject}
$$;";

        this.procedureNameForClearRoleByRoleType.Add(roleType, name);
        this.ProcedureDefinitionByName.Add(this.procedureNameForClearRoleByRoleType[roleType], definition);
    }

    private void GetCompositesRoleRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForGetRoleByRoleType.Add(roleType, name);

        // Get Composites Role (1-* and *-*) [relation table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForAssociation} {SqlTypeForObject})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE sql
AS $$
    SELECT {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation}={this.ParamNameForAssociation};
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void PrefetchCompositesRoleRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var objectsArray = this.ObjectArrayParam;
        var objectsArrayType = objectsArray.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForPrefetchRoleByRoleType.Add(roleType, name);

        // Prefetch Composites Role (1-* and *-*) [relation table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsArrayType});
CREATE FUNCTION {name}({objectsArray} {objectsArrayType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForRole} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objectsArray}) AS {ColumnNameForObject})

    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void AddCompositeRoleRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var roles = this.CompositeRoleArrayParam;
        var rolesType = roles.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForAddRole + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForAddRoleByRoleType.Add(roleType, name);

        // Add Composite Role (1-* and *-*) [relation table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    INSERT INTO {table} ({ColumnNameForAssociation},{ColumnNameForRole})
    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM relations
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void RemoveCompositeRoleRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var roles = this.CompositeRoleArrayParam;
        var rolesType = roles.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForRemoveRole + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForRemoveRoleByRoleType.Add(roleType, name);

        // Remove Composite Role (1-* and *-*) [relation table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    DELETE FROM {table}
    USING relations
    WHERE {table}.{ColumnNameForAssociation}=relations.{ColumnNameForAssociation} AND {table}.{ColumnNameForRole}=relations.{ColumnNameForRole}
$$;";

        this.ProcedureDefinitionByName.Add(this.procedureNameForRemoveRoleByRoleType[roleType], definition);
    }

    private void GetCompositeRoleRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetRole + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForGetRoleByRoleType.Add(roleType, name);

        // Get Composite Role (1-1 and *-1) [relation table]
        var definition =
            $@"DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForAssociation} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForCompositeRole} {SqlTypeForObject};
BEGIN
    SELECT {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation}={this.ParamNameForAssociation}
    INTO {this.ParamNameForCompositeRole};

    RETURN {this.ParamNameForCompositeRole};
END
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void PrefetchCompositeRoleRelationType(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForPrefetchRole + roleType.SingularFullName.ToLowerInvariant();

        // Prefetch Composite Role (1-1 and *-1) [relation table]
        var definition =
            $@"DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForRole} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForAssociation}, {ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.procedureNameForPrefetchRoleByRoleType.Add(roleType, name);
        this.ProcedureDefinitionByName.Add(this.procedureNameForPrefetchRoleByRoleType[roleType], definition);
    }

    private void SetCompositeRoleRelationType(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var roles = this.CompositeRoleArrayParam;
        var rolesType = roles.TypeName;
        var name = this.Database.SchemaName + "." + ProcedurePrefixForSetRole + roleType.SingularFullName.ToLowerInvariant();

        // Set Composite Role (1-1 and *-1) [relation table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType}, {rolesType});
CREATE FUNCTION {name}({objects} {objectsType}, {roles} {rolesType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH relations AS (SELECT UNNEST({objects}) AS {ColumnNameForAssociation}, UNNEST({roles}) AS {ColumnNameForRole})

    INSERT INTO {table}
    SELECT {ColumnNameForAssociation}, {ColumnNameForRole} from relations

    ON CONFLICT ({ColumnNameForAssociation})
    DO UPDATE
        SET {ColumnNameForRole} = excluded.{ColumnNameForRole};
$$;";

        this.procedureNameForSetRoleByRoleType.Add(roleType, name);
        this.ProcedureDefinitionByName.Add(this.procedureNameForSetRoleByRoleType[roleType], definition);
    }

    private void GetCompositeAssociationRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + roleType.SingularFullName.ToLowerInvariant();

        // Get Composite Association (1-1) [relation table]
        var definition =
            $@"DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS {SqlTypeForObject}
    LANGUAGE plpgsql
AS $$
DECLARE {this.ParamNameForAssociation} {SqlTypeForObject};
BEGIN
    SELECT {ColumnNameForAssociation}
    FROM {table}
    WHERE {ColumnNameForRole}={this.ParamNameForCompositeRole}
    INTO {this.ParamNameForAssociation};

    RETURN {this.ParamNameForAssociation};
END
$$;";

        this.procedureNameForGetAssociationByRoleType.Add(roleType, name);
        this.ProcedureDefinitionByName.Add(this.procedureNameForGetAssociationByRoleType[roleType], definition);
    }

    private void PrefetchCompositeAssociationRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.procedureNameForPrefetchAssociationByRoleType[roleType];

        // Prefetch Composite Association (1-1) [relation table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForAssociation} {SqlTypeForObject},
         {ColumnNameForObject} {SqlTypeForObject}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForAssociation},{ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForRole} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void GetCompositesAssociationRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var name = this.Database.SchemaName + "." + ProcedurePrefixForGetAssociation + roleType.SingularFullName.ToLowerInvariant();
        this.procedureNameForGetAssociationByRoleType.Add(roleType, name);

        // Get Composite Association (*-1) [relation table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({SqlTypeForObject});
CREATE FUNCTION {name}({this.ParamNameForCompositeRole} {SqlTypeForObject})
    RETURNS SETOF {SqlTypeForObject}
    LANGUAGE sql
AS $$
    SELECT {ColumnNameForAssociation}
    FROM {table}
    WHERE {ColumnNameForRole}={this.ParamNameForCompositeRole}
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void PrefetchCompositesAssociationRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.procedureNameForPrefetchAssociationByRoleType[roleType];

        // Prefetch Composite Association (*-1) [relation table]
        var definition = $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForAssociation} {SqlTypeForObject}
    )
    LANGUAGE SQL
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForAssociation},{ColumnNameForRole}
    FROM {table}
    WHERE {ColumnNameForRole} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void ClearCompositeRoleRelationTable(RoleType roleType)
    {
        var table = this.tableNameForRelationByRoleType[roleType];
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;
        var name = this.procedureNameForClearRoleByRoleType[roleType];

        // Clear Composite Role (1-1 and *-1) [relation table]
        var definition =
            $@"
DROP FUNCTION IF EXISTS {name}({objectsType});
CREATE FUNCTION {name}({objects} {objectsType})
    RETURNS void
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    DELETE FROM {table}
    WHERE {ColumnNameForAssociation} IN (SELECT {ColumnNameForObject} FROM objects)
$$;";

        this.ProcedureDefinitionByName.Add(name, definition);
    }

    private void UpdateVersionIds()
    {
        // Update Version Ids
        var definition = $@"
DROP FUNCTION IF EXISTS {this.ProcedureNameForUpdateVersion}({this.ObjectArrayParam.TypeName});
CREATE FUNCTION {this.ProcedureNameForUpdateVersion}({this.ObjectArrayParam} {this.ObjectArrayParam.TypeName})
    RETURNS void
    LANGUAGE sql
AS $$
    UPDATE {this.TableNameForObjects}
    SET {ColumnNameForVersion} = {ColumnNameForVersion} + 1
    WHERE {ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM unnest({this.ObjectArrayParam}) as t({ColumnNameForObject}));
$$;";

        this.ProcedureDefinitionByName.Add(this.ProcedureNameForUpdateVersion, definition);
    }

    private void GetVersionIds()
    {
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;

        // Get Version Ids
        var definition =
            $@"DROP FUNCTION IF EXISTS {this.ProcedureNameForGetVersion}({objectsType});
CREATE FUNCTION {this.ProcedureNameForGetVersion}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForVersion} {SqlTypeForVersion}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {this.TableNameForObjects}.{ColumnNameForObject}, {this.TableNameForObjects}.{ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {this.TableNameForObjects}.{ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";
        this.ProcedureDefinitionByName.Add(this.ProcedureNameForGetVersion, definition);
    }

    private void Instantiate()
    {
        var objects = this.ObjectArrayParam;
        var objectsType = objects.TypeName;

        // Instantiate
        var definition = $@"
DROP FUNCTION IF EXISTS {this.ProcedureNameForInstantiate}({objectsType});
CREATE FUNCTION {this.ProcedureNameForInstantiate}({objects} {objectsType})
    RETURNS TABLE
    (
         {ColumnNameForObject} {SqlTypeForObject},
         {ColumnNameForClass} {SqlTypeForClass},
         {ColumnNameForVersion} {SqlTypeForVersion}
    )
    LANGUAGE sql
AS $$
    WITH objects AS (SELECT UNNEST({objects}) AS {ColumnNameForObject})

    SELECT {ColumnNameForObject}, {ColumnNameForClass}, {ColumnNameForVersion}
    FROM {this.TableNameForObjects}
    WHERE {this.TableNameForObjects}.{ColumnNameForObject} IN (SELECT {ColumnNameForObject} FROM objects);
$$;";

        this.ProcedureDefinitionByName.Add(this.ProcedureNameForInstantiate, definition);
    }
}
