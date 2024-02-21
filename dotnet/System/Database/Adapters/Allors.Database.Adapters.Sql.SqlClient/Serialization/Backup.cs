// <copyright file="Backup.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.SqlClient;

using System.Collections.Generic;
using System.Xml;
using Allors.Database.Meta;

internal class Backup
{
    private readonly Database database;
    private readonly XmlWriter writer;

    internal Backup(Database database, XmlWriter writer)
    {
        this.database = database;
        this.writer = writer;
    }

    internal virtual void Execute(ManagementTransaction transaction)
    {
        var writeDocument = false;
        if (this.writer.WriteState == WriteState.Start)
        {
            this.writer.WriteStartDocument();
            this.writer.WriteStartElement(XmlBackup.Allors);
            writeDocument = true;
        }

        this.writer.WriteStartElement(XmlBackup.Population);
        this.writer.WriteAttributeString(XmlBackup.Version, XmlBackup.VersionCurrent.ToString());

        this.writer.WriteStartElement(XmlBackup.Objects);
        this.writer.WriteStartElement(XmlBackup.Database);
        this.BackupObjects(transaction);
        this.writer.WriteEndElement();
        this.writer.WriteEndElement();

        this.writer.WriteStartElement(XmlBackup.Relations);
        this.writer.WriteStartElement(XmlBackup.Database);
        this.BackupRelations(transaction);
        this.writer.WriteEndElement();
        this.writer.WriteEndElement();

        this.writer.WriteEndElement();

        if (writeDocument)
        {
            this.writer.WriteEndElement();
            this.writer.WriteEndDocument();
        }
    }

    protected void BackupObjects(ManagementTransaction transaction)
    {
        var mapping = this.database.Mapping;

        var concreteCompositeType = new List<Class>(this.database.MetaPopulation.Classes);
        concreteCompositeType.Sort();
        foreach (var type in concreteCompositeType)
        {
            var atLeastOne = false;

            var sql = $"SELECT {Sql.Mapping.ColumnNameForObject}, {Sql.Mapping.ColumnNameForVersion}\n";
            sql += $"FROM {this.database.Mapping.TableNameForObjects}\n";
            sql += $"WHERE {Sql.Mapping.ColumnNameForClass}={mapping.ParamInvocationNameForClass}\n";
            sql += $"ORDER BY {Sql.Mapping.ColumnNameForObject}";

            using (var command = transaction.Connection.CreateCommand())
            {
                command.CommandText = sql;
                command.AddInParameter(mapping.ParamInvocationNameForClass, type.Id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!atLeastOne)
                        {
                            atLeastOne = true;

                            this.writer.WriteStartElement(XmlBackup.ObjectType);
                            this.writer.WriteAttributeString(XmlBackup.Id, type.Id.ToString());
                        }
                        else
                        {
                            this.writer.WriteString(XmlBackup.ObjectsSplitter);
                        }

                        var objectId = long.Parse(reader[0].ToString());
                        var version = reader[1].ToString();

                        this.writer.WriteString(objectId + XmlBackup.ObjectSplitter + version);
                    }
                }
            }

            if (atLeastOne)
            {
                this.writer.WriteEndElement();
            }
        }
    }

    protected void BackupRelations(ManagementTransaction transaction)
    {
        var exclusiveRootClassesByObjectType = new Dictionary<IObjectType, HashSet<IObjectType>>();

        var relations = new List<RelationType>(this.database.MetaPopulation.RelationTypes);
        relations.Sort();

        foreach (var relation in relations)
        {
            var associationType = relation.AssociationType;

            if (associationType.ObjectType.Classes.Count > 0)
            {
                var roleType = relation.RoleType;

                var sql = string.Empty;
                if (roleType.ObjectType.IsUnit)
                {
                    if (!exclusiveRootClassesByObjectType.TryGetValue(associationType.ObjectType, out var exclusiveRootClasses))
                    {
                        exclusiveRootClasses = new HashSet<IObjectType>();
                        foreach (var concreteClass in associationType.ObjectType.Classes)
                        {
                            exclusiveRootClasses.Add(concreteClass.ExclusiveClass);
                        }

                        exclusiveRootClassesByObjectType[associationType.ObjectType] = exclusiveRootClasses;
                    }

                    var first = true;
                    foreach (var exclusiveRootClass in exclusiveRootClasses)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            sql += "UNION\n";
                        }

                        sql +=
                            $"SELECT {Sql.Mapping.ColumnNameForObject} As {Sql.Mapping.ColumnNameForAssociation}, {this.database.Mapping.ColumnNameByRelationType[roleType.RelationType]} As {Sql.Mapping.ColumnNameForRole}\n";
                        sql +=
                            $"FROM {this.database.Mapping.TableNameForObjectByClass[(Class)exclusiveRootClass]}\n";
                        sql +=
                            $"WHERE {this.database.Mapping.ColumnNameByRelationType[roleType.RelationType]} IS NOT NULL\n";
                    }

                    sql += $"ORDER BY {Sql.Mapping.ColumnNameForAssociation}";
                }
                else if ((roleType.IsMany && associationType.IsMany) || !relation.ExistExclusiveClasses)
                {
                    sql += $"SELECT {Sql.Mapping.ColumnNameForAssociation},{Sql.Mapping.ColumnNameForRole}\n";
                    sql += $"FROM {this.database.Mapping.TableNameForRelationByRelationType[relation]}\n";
                    sql += $"ORDER BY {Sql.Mapping.ColumnNameForAssociation},{Sql.Mapping.ColumnNameForRole}";
                }
                else
                {
                    // use foreign keys
                    if (roleType.IsOne)
                    {
                        sql +=
                            $"SELECT {Sql.Mapping.ColumnNameForObject} As {Sql.Mapping.ColumnNameForAssociation}, {this.database.Mapping.ColumnNameByRelationType[roleType.RelationType]} As {Sql.Mapping.ColumnNameForRole}\n";
                        sql +=
                            $"FROM {this.database.Mapping.TableNameForObjectByClass[associationType.ObjectType.ExclusiveClass]}\n";
                        sql +=
                            $"WHERE {this.database.Mapping.ColumnNameByRelationType[roleType.RelationType]} IS NOT NULL\n";
                        sql += $"ORDER BY {Sql.Mapping.ColumnNameForAssociation}";
                    }
                    else
                    {
                        // role.Many
                        sql +=
                            $"SELECT {this.database.Mapping.ColumnNameByRelationType[associationType.RelationType]} As {Sql.Mapping.ColumnNameForAssociation}, {Sql.Mapping.ColumnNameForObject} As {Sql.Mapping.ColumnNameForRole}\n";
                        sql +=
                            $"FROM {this.database.Mapping.TableNameForObjectByClass[((IComposite)roleType.ObjectType).ExclusiveClass]}\n";
                        sql +=
                            $"WHERE {this.database.Mapping.ColumnNameByRelationType[associationType.RelationType]} IS NOT NULL\n";
                        sql += $"ORDER BY {Sql.Mapping.ColumnNameForAssociation},{Sql.Mapping.ColumnNameForRole}";
                    }
                }

                using (var command = transaction.Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (var reader = command.ExecuteReader())
                    {
                        if (roleType.IsMany)
                        {
                            using (var relationTypeManyXmlWriter = new RelationTypeManyXmlWriter(relation, this.writer))
                            {
                                while (reader.Read())
                                {
                                    var a = long.Parse(reader[0].ToString());
                                    var r = long.Parse(reader[1].ToString());
                                    relationTypeManyXmlWriter.Write(a, r);
                                }
                            }
                        }
                        else
                        {
                            using (var relationTypeOneXmlWriter = new RelationTypeOneXmlWriter(relation, this.writer))
                            {
                                while (reader.Read())
                                {
                                    var a = long.Parse(reader[0].ToString());

                                    if (roleType.ObjectType.IsUnit)
                                    {
                                        var unitTypeTag = ((Unit)roleType.ObjectType).Tag;
                                        var r = command.GetValue(reader, unitTypeTag, 1);
                                        var content = XmlBackup.WriteString(unitTypeTag, r);
                                        relationTypeOneXmlWriter.Write(a, content);
                                    }
                                    else
                                    {
                                        var r = reader[1];
                                        relationTypeOneXmlWriter.Write(a, XmlConvert.ToString(long.Parse(r.ToString())));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
