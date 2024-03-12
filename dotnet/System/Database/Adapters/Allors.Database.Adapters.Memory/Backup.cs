// <copyright file="Backup.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System.Collections.Generic;
using System.Xml;
using Allors.Database.Meta;

public class Backup
{
    private readonly Dictionary<ObjectType, List<Strategy>> sortedNonDeletedStrategiesByObjectType;
    private readonly Transaction transaction;
    private readonly XmlWriter writer;

    public Backup(Transaction transaction, XmlWriter writer, Dictionary<ObjectType, List<Strategy>> sortedNonDeletedStrategiesByObjectType)
    {
        this.transaction = transaction;
        this.writer = writer;
        this.sortedNonDeletedStrategiesByObjectType = sortedNonDeletedStrategiesByObjectType;
    }

    public void Execute()
    {
        var writeDocument = false;
        if (this.writer.WriteState == WriteState.Start)
        {
            this.writer.WriteStartDocument();
            this.writer.WriteStartElement(XmlBackup.Allors);
            writeDocument = true;
        }

        this.BackupPopulation();

        if (writeDocument)
        {
            this.writer.WriteEndElement();
            this.writer.WriteEndDocument();
        }
    }

    internal void BackupPopulation()
    {
        this.writer.WriteStartElement(XmlBackup.Population);
        this.writer.WriteAttributeString(XmlBackup.Version, XmlBackup.VersionCurrent.ToString());

        this.BackupObjectType();

        this.BackupRelationType();

        this.writer.WriteEndElement();
    }

    internal virtual void BackupObjectType()
    {
        this.writer.WriteStartElement(XmlBackup.Objects);
        this.writer.WriteStartElement(XmlBackup.Database);

        var sortedObjectTypes = new List<ObjectType>(this.sortedNonDeletedStrategiesByObjectType.Keys);
        sortedObjectTypes.Sort();

        foreach (var objectType in sortedObjectTypes)
        {
            var sortedNonDeletedStrategies = this.sortedNonDeletedStrategiesByObjectType[objectType];

            if (sortedNonDeletedStrategies.Count > 0)
            {
                this.writer.WriteStartElement(XmlBackup.ObjectType);
                this.writer.WriteAttributeString(XmlBackup.Id, objectType.Id.ToString("N").ToLowerInvariant());

                for (var i = 0; i < sortedNonDeletedStrategies.Count; i++)
                {
                    var strategy = sortedNonDeletedStrategies[i];
                    if (i > 0)
                    {
                        this.writer.WriteString(XmlBackup.ObjectsSplitter);
                    }

                    this.writer.WriteString(strategy.ObjectId + XmlBackup.ObjectSplitter + strategy.ObjectVersion);
                }

                this.writer.WriteEndElement();
            }
        }

        this.writer.WriteEndElement();
        this.writer.WriteEndElement();
    }

    internal virtual void BackupRelationType()
    {
        this.writer.WriteStartElement(XmlBackup.Relations);
        this.writer.WriteStartElement(XmlBackup.Database);

        var sortedStrategiesByRoleType = new Dictionary<RoleType, List<Strategy>>();
        foreach (var dictionaryEntry in this.sortedNonDeletedStrategiesByObjectType)
        {
            foreach (var strategy in dictionaryEntry.Value)
            {
                strategy.FillRoleForBackup(sortedStrategiesByRoleType);
            }
        }

        var strategySorter = new Strategy.ObjectIdComparer();
        foreach (var dictionaryEntry in sortedStrategiesByRoleType)
        {
            var strategies = dictionaryEntry.Value;
            strategies.Sort(strategySorter);
        }

        var sortedRelationTypes = new List<RoleType>(((IDatabase)this.transaction.Database).MetaPopulation.RoleTypes);
        sortedRelationTypes.Sort();
        foreach (var roleType in sortedRelationTypes)
        {
            sortedStrategiesByRoleType.TryGetValue(roleType, out var strategies);

            if (strategies != null)
            {
                this.writer.WriteStartElement(roleType.ObjectType is Unit
                    ? XmlBackup.RelationTypeUnit
                    : XmlBackup.RelationTypeComposite);

                this.writer.WriteAttributeString(XmlBackup.Id, roleType.Id.ToString("N").ToLowerInvariant());

                if (roleType.ObjectType is Unit)
                {
                    foreach (var strategy in strategies)
                    {
                        strategy.BackupUnit(this.writer, roleType);
                    }
                }
                else if (roleType.IsMany)
                {
                    foreach (var strategy in strategies)
                    {
                        strategy.BackupComposites(this.writer, roleType);
                    }
                }
                else
                {
                    foreach (var strategy in strategies)
                    {
                        strategy.BackupComposite(this.writer, roleType);
                    }
                }

                this.writer.WriteEndElement();
            }
        }

        this.writer.WriteEndElement();
        this.writer.WriteEndElement();
    }
}
