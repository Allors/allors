// <copyright file="Import.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using global::Npgsql;
using Allors.Database.Meta;
using NpgsqlTypes;

public class Restore
{
    private readonly Dictionary<long, Class> classByObjectId;
    private readonly NpgsqlConnection connection;
    private readonly Database database;
    private readonly ObjectNotRestoredEventHandler objectNotRestored;
    private readonly RelationNotRestoredEventHandler relationNotRestored;

    public Restore(Database database, NpgsqlConnection connection, ObjectNotRestoredEventHandler objectNotRestored,
        RelationNotRestoredEventHandler relationNotRestored)
    {
        this.database = database;
        this.connection = connection;
        this.objectNotRestored = objectNotRestored;
        this.relationNotRestored = relationNotRestored;

        this.classByObjectId = new Dictionary<long, Class>();
    }

    public void Execute(XmlReader reader)
    {
        while (reader.Read())
        {
            if (reader.NodeType.Equals(XmlNodeType.Element) && reader.Name.Equals(XmlBackup.Population))
            {
                var version = reader.GetAttribute(XmlBackup.Version);
                if (string.IsNullOrEmpty(version))
                {
                    throw new ArgumentException("Backup population has no version.");
                }

                XmlBackup.CheckVersion(int.Parse(version));

                if (!reader.IsEmptyElement)
                {
                    this.RestorePopulation(reader);
                }

                break;
            }
        }
    }

    private void RestorePopulation(XmlReader reader)
    {
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                // eat everything but elements
                case XmlNodeType.Element:
                    if (reader.Name.Equals(XmlBackup.Objects))
                    {
                        if (!reader.IsEmptyElement)
                        {
                            this.RestoreObjects(reader.ReadSubtree());
                        }
                    }
                    else if (reader.Name.Equals(XmlBackup.Relations))
                    {
                        if (!reader.IsEmptyElement)
                        {
                            this.RestoreRelations(reader.ReadSubtree());
                        }
                    }

                    break;
            }
        }
    }

    private void RestoreObjects(XmlReader reader)
    {
        reader.MoveToContent();

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name.Equals(XmlBackup.Database))
                    {
                        if (!reader.IsEmptyElement)
                        {
                            this.RestoreObjectsDatabase(reader.ReadSubtree());
                        }
                    }
                    else if (reader.Name.Equals(XmlBackup.Workspace))
                    {
                        throw new Exception("Can not restore workspace objects in a database.");
                    }

                    break;
            }
        }
    }

    private void RestoreObjectsDatabase(XmlReader reader)
    {
        var xmlObjects = new Objects(this.database, this.OnObjectNotRestore, this.classByObjectId, reader);
        var mapping = this.database.Mapping;
        using (var writer = this.connection.BeginBinaryImport(
                   $"COPY {mapping.TableNameForObjects} ({Sql.Mapping.ColumnNameForObject}, {Sql.Mapping.ColumnNameForClass}, {Sql.Mapping.ColumnNameForVersion}) FROM STDIN (FORMAT BINARY)"))
        {
            foreach (var values in xmlObjects)
            {
                writer.StartRow();
                writer.Write(values[0], NpgsqlDbType.Bigint);
                writer.Write(values[1], NpgsqlDbType.Uuid);
                writer.Write(XmlBackup.EnsureVersion((long)values[2]), NpgsqlDbType.Bigint);
            }

            writer.Complete();
        }

        // TODO: move this to a stored procedure
        // insert from _o table into class tables
        using (var transaction = this.connection.BeginTransaction())
        {
            foreach (var @class in this.database.MetaPopulation.Classes)
            {
                var tableName = this.database.Mapping.TableNameForObjectByClass[@class];

                using (var command = this.connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.Text;
                    command.CommandText = $@"
insert into {tableName} (o, c)
select o, c from allors._o
where c = '{@class.Id}'";

                    command.ExecuteNonQuery();
                }
            }

            transaction.Commit();
        }
    }

    private void RestoreRelations(XmlReader reader)
    {
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (reader.Name.Equals(XmlBackup.Database))
                    {
                        if (!reader.IsEmptyElement)
                        {
                            this.RestoreRelationsDatabase(reader);
                        }
                    }
                    else if (reader.Name.Equals(XmlBackup.Workspace))
                    {
                        throw new Exception("Can not restore workspace relations in a database.");
                    }

                    break;
            }
        }
    }

    private void RestoreRelationsDatabase(XmlReader reader)
    {
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                // eat everything but elements
                case XmlNodeType.Element:
                    if (!reader.IsEmptyElement)
                    {
                        if (reader.Name.Equals(XmlBackup.RelationTypeUnit)
                            || reader.Name.Equals(XmlBackup.RelationTypeComposite))
                        {
                            var roleTypeIdString = reader.GetAttribute(XmlBackup.Id);
                            if (string.IsNullOrEmpty(roleTypeIdString))
                            {
                                throw new Exception("Relation type has no id");
                            }

                            var roleTypeId = new Guid(roleTypeIdString);
                            var roleType = (RoleType)this.database.MetaPopulation.FindById(roleTypeId);

                            if (reader.Name.Equals(XmlBackup.RelationTypeUnit))
                            {
                                if (roleType == null || roleType.ObjectType is Composite)
                                {
                                    this.CantRestoreUnitRole(reader.ReadSubtree(), roleTypeId);
                                }
                                else
                                {
                                    this.RestoreUnitRelations(reader.ReadSubtree(), roleType);
                                }
                            }
                            else if (reader.Name.Equals(XmlBackup.RelationTypeComposite))
                            {
                                if (roleType == null || roleType.ObjectType is Unit)
                                {
                                    this.CantRestoreCompositeRole(reader.ReadSubtree(), roleTypeId);
                                }
                                else
                                {
                                    this.RestoreCompositeRelations(reader.ReadSubtree(), roleType);
                                }
                            }
                        }
                    }

                    break;
            }
        }
    }

    private void RestoreUnitRelations(XmlReader reader, RoleType roleType)
    {
        var allowedClasses = new HashSet<Class>(roleType.AssociationType.Composite.Classes);
        var unitRelationsByClass = new Dictionary<Class, List<UnitRelation>>();

        var skip = false;
        while (skip || reader.Read())
        {
            skip = false;

            switch (reader.NodeType)
            {
                // eat everything but elements
                case XmlNodeType.Element:
                    if (reader.Name.Equals(XmlBackup.Relation))
                    {
                        var associationIdString = reader.GetAttribute(XmlBackup.Association);
                        var associationId = long.Parse(associationIdString);

                        this.classByObjectId.TryGetValue(associationId, out var @class);

                        if (@class == null || !allowedClasses.Contains(@class))
                        {
                            this.CantRestoreUnitRole(reader.ReadSubtree(), roleType.Id);
                        }
                        else
                        {
                            if (!unitRelationsByClass.TryGetValue(@class, out var unitRelations))
                            {
                                unitRelations = new List<UnitRelation>();
                                unitRelationsByClass[@class] = unitRelations;
                            }

                            var value = string.Empty;
                            if (!reader.IsEmptyElement)
                            {
                                value = reader.ReadElementContentAsString();
                            }

                            try
                            {
                                object unit = null;
                                if (reader.IsEmptyElement)
                                {
                                    var unitType = (Unit)roleType.ObjectType;
                                    unit = unitType.Tag switch
                                    {
                                        UnitTags.String => string.Empty,
                                        UnitTags.Binary => Array.Empty<byte>(),
                                        _ => unit,
                                    };
                                }
                                else
                                {
                                    var unitType = (Unit)roleType.ObjectType;
                                    var unitTypeTag = unitType.Tag;
                                    unit = XmlBackup.ReadString(value, unitTypeTag);
                                }

                                unitRelations.Add(new UnitRelation(associationId, unit));
                            }
                            catch
                            {
                                this.OnRelationNotRestored(roleType.Id, associationId, value);
                            }

                            skip = reader.IsStartElement();
                        }
                    }

                    break;
            }
        }

        var con = this.database.ConnectionFactory.Create();
        try
        {
            foreach ((Class @class, List<UnitRelation> unitRelations) in unitRelationsByClass)
            {
                var sql = this.database.Mapping.ProcedureNameForSetUnitRoleByRoleTypeByClass[@class][roleType];
                var command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.UnitTableParameter(roleType, unitRelations);
                command.ExecuteNonQuery();
            }

            con.Commit();
        }
        catch
        {
            con.Rollback();
        }
    }

    private void RestoreCompositeRelations(XmlReader reader, RoleType roleType)
    {
        var con = this.database.ConnectionFactory.Create();
        try
        {
            var relations = new CompositeRelations(
                this.database,
                roleType,
                this.CantRestoreCompositeRole,
                this.OnRelationNotRestored,
                this.classByObjectId,
                reader);

            var sql = roleType.IsOne
                ? this.database.Mapping.ProcedureNameForSetRoleByRoleType[roleType]
                : this.database.Mapping.ProcedureNameForAddRoleByRoleType[roleType];

            var command = con.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            command.AddCompositeRoleTableParameter(relations.ToArray());
            command.ExecuteNonQuery();

            con.Commit();
        }
        catch
        {
            con.Rollback();
        }
    }

    private void CantRestoreUnitRole(XmlReader reader, Guid relationTypeId)
    {
        while (reader.Read())
        {
            if (reader.IsStartElement() && reader.Name.Equals(XmlBackup.Relation))
            {
                var a = reader.GetAttribute(XmlBackup.Association);
                var value = string.Empty;

                if (!reader.IsEmptyElement)
                {
                    value = reader.ReadElementContentAsString();
                }

                this.OnRelationNotRestored(relationTypeId, long.Parse(a), value);
            }
        }
    }

    private void CantRestoreCompositeRole(XmlReader reader, Guid relationTypeId)
    {
        while (reader.Read())
        {
            if (reader.IsStartElement() && reader.Name.Equals(XmlBackup.Relation))
            {
                var associationIdString = reader.GetAttribute(XmlBackup.Association);
                var associationId = long.Parse(associationIdString);
                if (string.IsNullOrEmpty(associationIdString))
                {
                    throw new Exception("Association id is missing");
                }

                if (reader.IsEmptyElement)
                {
                    this.OnRelationNotRestored(relationTypeId, associationId, null);
                }
                else
                {
                    var value = reader.ReadElementContentAsString();
                    foreach (var r in value.Split(XmlBackup.ObjectsSplitterCharArray))
                    {
                        this.OnRelationNotRestored(relationTypeId, associationId, r);
                    }
                }
            }
        }
    }

    #region Import Errors
    private void OnObjectNotRestore(Guid objectTypeId, long allorsObjectId)
    {
        if (this.objectNotRestored != null)
        {
            this.objectNotRestored(this, new ObjectNotRestoredEventArgs(objectTypeId, allorsObjectId));
        }
        else
        {
            throw new Exception("Object not restored: " + objectTypeId + ":" + allorsObjectId);
        }
    }

    private void OnRelationNotRestored(Guid relationTypeId, long associationObjectId, string roleContents)
    {
        var args = new RelationNotRestoredEventArgs(relationTypeId, associationObjectId, roleContents);
        if (this.relationNotRestored != null)
        {
            this.relationNotRestored(this, args);
        }
        else
        {
            throw new Exception("Role not restored: " + args);
        }
    }
    #endregion Import Errors
}
