// <copyright file="Restore.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using Allors.Database.Meta;
using Microsoft.Data.SqlClient;

public class Restore
{
    private readonly Dictionary<long, IClass> classByObjectId;
    private readonly SqlConnection connection;
    private readonly Database database;
    private readonly ObjectNotRestoredEventHandler objectNotRestored;
    private readonly RelationNotRestoredEventHandler relationNotRestored;

    public Restore(Database database, SqlConnection connection, ObjectNotRestoredEventHandler objectNotRestored,
        RelationNotRestoredEventHandler relationNotRestored)
    {
        this.database = database;
        this.connection = connection;
        this.objectNotRestored = objectNotRestored;
        this.relationNotRestored = relationNotRestored;

        this.classByObjectId = new Dictionary<long, IClass>();
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
        var xmlObjects = new Objects(this.database, this.OnObjectNotRestored, this.classByObjectId, reader);
        using (var objectsReader = new RestoreObjectsReader(xmlObjects))
        using (var sqlBulkCopy = new SqlBulkCopy(this.connection, SqlBulkCopyOptions.KeepIdentity, null))
        {
            sqlBulkCopy.BulkCopyTimeout = 0;
            sqlBulkCopy.BatchSize = 5000;
            sqlBulkCopy.DestinationTableName = this.database.Mapping.TableNameForObjects;
            sqlBulkCopy.WriteToServer(objectsReader);
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
                            var relationTypeIdString = reader.GetAttribute(XmlBackup.Id);
                            if (string.IsNullOrEmpty(relationTypeIdString))
                            {
                                throw new Exception("Relation type has no id");
                            }

                            var relationTypeId = new Guid(relationTypeIdString);
                            var relationType = (RelationType)this.database.MetaPopulation.FindById(relationTypeId);

                            if (reader.Name.Equals(XmlBackup.RelationTypeUnit))
                            {
                                if (relationType == null || relationType.RoleType.ObjectType is IComposite)
                                {
                                    this.CantRestoreUnitRole(reader.ReadSubtree(), relationTypeId);
                                }
                                else
                                {
                                    this.RestoreUnitRelations(reader.ReadSubtree(), relationType);
                                }
                            }
                            else if (reader.Name.Equals(XmlBackup.RelationTypeComposite))
                            {
                                if (relationType == null || relationType.RoleType.ObjectType is Unit)
                                {
                                    this.CantRestoreCompositeRole(reader.ReadSubtree(), relationTypeId);
                                }
                                else
                                {
                                    this.RestoreCompositeRelations(reader.ReadSubtree(), relationType);
                                }
                            }
                        }
                    }

                    break;
            }
        }
    }

    private void RestoreUnitRelations(XmlReader reader, RelationType relationType)
    {
        var allowedClasses = new HashSet<IClass>(relationType.AssociationType.ObjectType.Classes);
        var unitRelationsByClass = new Dictionary<IClass, List<UnitRelation>>();

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
                            this.CantRestoreUnitRole(reader.ReadSubtree(), relationType.Id);
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
                                    var unitType = (Unit)relationType.RoleType.ObjectType;
                                    unit = unitType.Tag switch
                                    {
                                        UnitTags.String => string.Empty,
                                        UnitTags.Binary => Array.Empty<byte>(),
                                        _ => unit,
                                    };
                                }
                                else
                                {
                                    var unitType = (Unit)relationType.RoleType.ObjectType;
                                    var unitTypeTag = unitType.Tag;
                                    unit = XmlBackup.ReadString(value, unitTypeTag);
                                }

                                unitRelations.Add(new UnitRelation(associationId, unit));
                            }
                            catch
                            {
                                this.OnRelationNotRestored(relationType.Id, associationId, value);
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
            foreach (var kvp in unitRelationsByClass)
            {
                var @class = kvp.Key;
                var unitRelations = kvp.Value;
                var sql = this.database.Mapping.ProcedureNameForSetUnitRoleByRelationTypeByClass[@class][relationType];
                var command = con.CreateCommand();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;
                command.UnitTableParameter(relationType.RoleType, unitRelations);
                command.ExecuteNonQuery();
            }

            con.Commit();
        }
        catch
        {
            con.Rollback();
        }
    }

    private void RestoreCompositeRelations(XmlReader reader, RelationType relationType)
    {
        var con = this.database.ConnectionFactory.Create();
        try
        {
            var relations = new RestoreCompositeRelations(
                this.database,
                relationType,
                this.CantRestoreCompositeRole,
                this.OnRelationNotRestored,
                this.classByObjectId,
                reader);

            var sql = relationType.RoleType.IsOne
                ? this.database.Mapping.ProcedureNameForSetRoleByRelationType[relationType]
                : this.database.Mapping.ProcedureNameForAddRoleByRelationType[relationType];

            var command = con.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            command.AddCompositeRoleTableParameter(relations);
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
    private void OnObjectNotRestored(Guid objectTypeId, long allorsObjectId)
    {
        if (this.objectNotRestored != null)
        {
            this.objectNotRestored(this, new ObjectNotRestoredEventArgs(objectTypeId, allorsObjectId));
        }
        else
        {
            throw new Exception($"Object not restored: {objectTypeId}:{allorsObjectId}");
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
            throw new Exception($"Role not restored: {args}");
        }
    }
    #endregion Import Errors
}
