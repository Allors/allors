// <copyright file="Import.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System;
using System.Collections.Generic;
using System.Xml;
using Allors.Database.Meta;

public class Restore
{
    private static readonly byte[] emptyByteArray = Array.Empty<byte>();
    private readonly XmlReader reader;

    private readonly Transaction transaction;

    public Restore(Transaction transaction, XmlReader reader)
    {
        this.transaction = transaction;
        this.reader = reader;
    }

    public void Execute()
    {
        while (this.reader.Read())
        {
            // only process elements, ignore others
            if (this.reader.NodeType.Equals(XmlNodeType.Element) && this.reader.Name.Equals(XmlBackup.Population))
            {
                var version = this.reader.GetAttribute(XmlBackup.Version);
                if (string.IsNullOrEmpty(version))
                {
                    throw new ArgumentException("Backup population has no version.");
                }

                XmlBackup.CheckVersion(int.Parse(version));

                if (!this.reader.IsEmptyElement)
                {
                    this.RestorePopulation();
                }

                break;
            }
        }
    }

    private void RestorePopulation()
    {
        while (this.reader.Read())
        {
            switch (this.reader.NodeType)
            {
                // eat everything but elements
                case XmlNodeType.Element:
                    if (this.reader.Name.Equals(XmlBackup.Objects))
                    {
                        if (!this.reader.IsEmptyElement)
                        {
                            this.RestoreObjects();
                        }
                    }
                    else if (this.reader.Name.Equals(XmlBackup.Relations))
                    {
                        if (!this.reader.IsEmptyElement)
                        {
                            this.RestoreRelations();
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" +
                                            XmlBackup.Population + ">");
                    }

                    break;

                case XmlNodeType.EndElement:
                    if (!this.reader.Name.Equals(XmlBackup.Population))
                    {
                        throw new Exception("Expected closing element </" + XmlBackup.Population + ">");
                    }

                    return;
            }
        }
    }

    private void RestoreObjects()
    {
        while (this.reader.Read())
        {
            switch (this.reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (this.reader.Name.Equals(XmlBackup.Database))
                    {
                        if (!this.reader.IsEmptyElement)
                        {
                            this.RestoreObjectTypes();
                        }
                    }
                    else if (this.reader.Name.Equals(XmlBackup.Workspace))
                    {
                        throw new Exception("Can not restore workspace objects in a database.");
                    }
                    else
                    {
                        throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" + XmlBackup.Objects +
                                            ">");
                    }

                    break;

                case XmlNodeType.EndElement:
                    if (!this.reader.Name.Equals(XmlBackup.Objects))
                    {
                        throw new Exception("Expected closing element </" + XmlBackup.Objects + ">");
                    }

                    return;
            }
        }
    }

    private void RestoreObjectTypes()
    {
        var skip = false;
        while (skip || this.reader.Read())
        {
            skip = false;

            switch (this.reader.NodeType)
            {
                // eat everything but elements
                case XmlNodeType.Element:
                    if (this.reader.Name.Equals(XmlBackup.ObjectType))
                    {
                        if (!this.reader.IsEmptyElement)
                        {
                            var objectTypeIdString = this.reader.GetAttribute(XmlBackup.Id);
                            if (string.IsNullOrEmpty(objectTypeIdString))
                            {
                                throw new Exception("object type has no id");
                            }

                            var objectTypeId = new Guid(objectTypeIdString);
                            var objectType = this.transaction.Database.ObjectFactory.GetObjectType(objectTypeId);

                            var objectIdsString = this.reader.ReadElementContentAsString();
                            foreach (var objectIdString in objectIdsString.Split(XmlBackup.ObjectsSplitterCharArray))
                            {
                                var objectArray = objectIdString.Split(XmlBackup.ObjectSplitterCharArray);

                                var objectId = long.Parse(objectArray[0]);
                                var objectVersion = objectArray.Length > 1
                                    ? XmlBackup.EnsureVersion(long.Parse(objectArray[1]))
                                    : (long)Allors.Version.DatabaseInitial;

                                if (objectType is IClass)
                                {
                                    this.transaction.InsertStrategy((IClass)objectType, objectId, objectVersion);
                                }
                                else
                                {
                                    this.transaction.Database.OnObjectNotRestored(objectTypeId, objectId);
                                }
                            }

                            skip = this.reader.IsStartElement() ||
                                   (this.reader.NodeType == XmlNodeType.EndElement && this.reader.Name.Equals(XmlBackup.Database));
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" +
                                            XmlBackup.Database + ">");
                    }

                    break;

                case XmlNodeType.EndElement:
                    if (!this.reader.Name.Equals(XmlBackup.Database))
                    {
                        throw new Exception("Expected closing element </" + XmlBackup.Database + ">");
                    }

                    return;
            }
        }
    }

    private void RestoreRelations()
    {
        while (this.reader.Read())
        {
            switch (this.reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (this.reader.Name.Equals(XmlBackup.Database))
                    {
                        if (!this.reader.IsEmptyElement)
                        {
                            this.RestoreDatabaseRelationTypes();
                        }
                    }
                    else if (this.reader.Name.Equals(XmlBackup.Workspace))
                    {
                        throw new Exception("Can not restore workspace relations in a database.");
                    }
                    else
                    {
                        throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" +
                                            XmlBackup.Relations + ">");
                    }

                    break;

                case XmlNodeType.EndElement:
                    if (!this.reader.Name.Equals(XmlBackup.Relations))
                    {
                        throw new Exception("Expected closing element </" + XmlBackup.Relations + ">");
                    }

                    return;
            }
        }
    }

    private void RestoreDatabaseRelationTypes()
    {
        while (this.reader.Read())
        {
            switch (this.reader.NodeType)
            {
                // eat everything but elements
                case XmlNodeType.Element:
                    if (!this.reader.IsEmptyElement)
                    {
                        if (this.reader.Name.Equals(XmlBackup.RelationTypeUnit)
                            || this.reader.Name.Equals(XmlBackup.RelationTypeComposite))
                        {
                            var relationTypeIdString = this.reader.GetAttribute(XmlBackup.Id);
                            if (string.IsNullOrEmpty(relationTypeIdString))
                            {
                                throw new Exception("Relation type has no id");
                            }

                            var relationTypeId = new Guid(relationTypeIdString);
                            var relationType = (RelationType)this.transaction.Database.MetaPopulation.FindById(relationTypeId);

                            if (this.reader.Name.Equals(XmlBackup.RelationTypeUnit))
                            {
                                if (relationType == null || relationType.RoleType.ObjectType is IComposite)
                                {
                                    this.CantRestoreUnitRole(relationTypeId);
                                }
                                else
                                {
                                    this.RestoreUnitRelations(relationType);
                                }
                            }
                            else if (this.reader.Name.Equals(XmlBackup.RelationTypeComposite))
                            {
                                if (relationType == null || relationType.RoleType.ObjectType is Unit)
                                {
                                    this.CantRestoreCompositeRole(relationTypeId);
                                }
                                else
                                {
                                    this.RestoreCompositeRelations(relationType);
                                }
                            }
                        }
                        else
                        {
                            throw new Exception(
                                "Unknown child element <" + this.reader.Name + "> in parent element <"
                                + XmlBackup.Database + ">");
                        }
                    }

                    break;

                case XmlNodeType.EndElement:
                    if (!this.reader.Name.Equals(XmlBackup.Database))
                    {
                        throw new Exception("Expected closing element </" + XmlBackup.Database + ">");
                    }

                    return;
            }
        }
    }

    private void RestoreUnitRelations(RelationType relationType)
    {
        var skip = false;
        while (skip || this.reader.Read())
        {
            skip = false;

            switch (this.reader.NodeType)
            {
                // eat everything but elements
                case XmlNodeType.Element:
                    if (this.reader.Name.Equals(XmlBackup.Relation))
                    {
                        var associationIdString = this.reader.GetAttribute(XmlBackup.Association);
                        var associationId = long.Parse(associationIdString);
                        var strategy = this.RestoreInstantiateStrategy(associationId);

                        var value = string.Empty;
                        if (!this.reader.IsEmptyElement)
                        {
                            value = this.reader.ReadElementContentAsString();

                            skip = this.reader.IsStartElement() ||
                                   (this.reader.NodeType == XmlNodeType.EndElement &&
                                    this.reader.Name.Equals(XmlBackup.RelationTypeUnit));
                        }

                        if (strategy == null)
                        {
                            this.transaction.Database.OnRelationNotRestored(relationType.Id, associationId, value);
                        }
                        else
                        {
                            try
                            {
                                this.transaction.Database.UnitRoleChecks(strategy, relationType.RoleType);
                                if (this.reader.IsEmptyElement)
                                {
                                    var unitType = (Unit)relationType.RoleType.ObjectType;
                                    switch (unitType.Tag)
                                    {
                                        case UnitTags.String:
                                            strategy.SetUnitRole(relationType.RoleType, string.Empty);
                                            break;

                                        case UnitTags.Binary:
                                            strategy.SetUnitRole(relationType.RoleType, emptyByteArray);
                                            break;
                                    }
                                }
                                else
                                {
                                    var unitType = (Unit)relationType.RoleType.ObjectType;
                                    var unitTypeTag = unitType.Tag;

                                    var unit = XmlBackup.ReadString(value, unitTypeTag);
                                    strategy.SetUnitRole(relationType.RoleType, unit);
                                }
                            }
                            catch
                            {
                                this.transaction.Database.OnRelationNotRestored(relationType.Id, associationId, value);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" +
                                            XmlBackup.RelationTypeUnit + ">");
                    }

                    break;

                case XmlNodeType.EndElement:
                    if (!this.reader.Name.Equals(XmlBackup.RelationTypeUnit))
                    {
                        throw new Exception("Expected closing element </" + XmlBackup.RelationTypeUnit + ">");
                    }

                    return;
            }
        }
    }

    private void RestoreCompositeRelations(RelationType relationType)
    {
        var skip = false;
        while (skip || this.reader.Read())
        {
            skip = false;

            switch (this.reader.NodeType)
            {
                // eat everything but elements
                case XmlNodeType.Element:
                    if (this.reader.Name.Equals(XmlBackup.Relation))
                    {
                        var associationId = long.Parse(this.reader.GetAttribute(XmlBackup.Association));
                        var association = this.RestoreInstantiateStrategy(associationId);

                        var value = string.Empty;
                        if (!this.reader.IsEmptyElement)
                        {
                            value = this.reader.ReadElementContentAsString();

                            skip = this.reader.IsStartElement() ||
                                   (this.reader.NodeType == XmlNodeType.EndElement &&
                                    this.reader.Name.Equals(XmlBackup.RelationTypeComposite));

                            var roleIdsString = value;
                            var roleIdStringArray = roleIdsString.Split(XmlBackup.ObjectsSplitterCharArray);

                            if (association == null ||
                                !this.transaction.Database.ContainsClass(
                                    relationType.AssociationType.ObjectType, association.UncheckedObjectType) ||
                                (relationType.RoleType.IsOne && roleIdStringArray.Length != 1))
                            {
                                foreach (var roleId in roleIdStringArray)
                                {
                                    this.transaction.Database.OnRelationNotRestored(relationType.Id, associationId, roleId);
                                }
                            }
                            else if (relationType.RoleType.IsOne)
                            {
                                var roleIdString = long.Parse(roleIdStringArray[0]);
                                var roleStrategy = this.RestoreInstantiateStrategy(roleIdString);
                                if (roleStrategy == null ||
                                    !this.transaction.Database.ContainsClass((IComposite)relationType.RoleType.ObjectType,
                                        roleStrategy.UncheckedObjectType))
                                {
                                    this.transaction.Database.OnRelationNotRestored(relationType.Id, associationId, roleIdStringArray[0]);
                                }
                                else if (relationType.RoleType.AssociationType.IsMany)
                                {
                                    association.SetCompositeRoleMany2One(relationType.RoleType, roleStrategy);
                                }
                                else
                                {
                                    association.SetCompositeRoleOne2One(relationType.RoleType, roleStrategy);
                                }
                            }
                            else
                            {
                                var roleStrategies = new HashSet<Strategy>();
                                foreach (var roleIdString in roleIdStringArray)
                                {
                                    var roleId = long.Parse(roleIdString);
                                    var role = this.RestoreInstantiateStrategy(roleId);
                                    if (role == null ||
                                        !this.transaction.Database.ContainsClass(
                                            (IComposite)relationType.RoleType.ObjectType,
                                            role.UncheckedObjectType))
                                    {
                                        this.transaction.Database.OnRelationNotRestored(relationType.Id, associationId, roleId.ToString());
                                    }
                                    else
                                    {
                                        roleStrategies.Add(role);
                                    }
                                }

                                if (relationType.RoleType.AssociationType.IsMany)
                                {
                                    association.SetCompositesRolesMany2Many(relationType.RoleType, roleStrategies);
                                }
                                else
                                {
                                    association.SetCompositesRolesOne2Many(relationType.RoleType, roleStrategies);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown child element <" + this.reader.Name + "> in parent element <" +
                                            XmlBackup.RelationTypeComposite + ">");
                    }

                    break;

                case XmlNodeType.EndElement:
                    if (!this.reader.Name.Equals(XmlBackup.RelationTypeComposite))
                    {
                        throw new Exception("Expected closing element </" + XmlBackup.RelationTypeComposite +
                                            ">");
                    }

                    return;
            }
        }
    }

    private Strategy RestoreInstantiateStrategy(long id) => this.transaction.GetStrategy(id);

    private void CantRestoreUnitRole(Guid relationTypeId)
    {
        while (this.reader.Read())
        {
            switch (this.reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (this.reader.Name.Equals(XmlBackup.Relation))
                    {
                        var a = this.reader.GetAttribute(XmlBackup.Association);
                        var value = string.Empty;

                        if (!this.reader.IsEmptyElement)
                        {
                            value = this.reader.ReadElementContentAsString();
                        }

                        this.transaction.Database.OnRelationNotRestored(relationTypeId, long.Parse(a), value);
                    }

                    break;

                case XmlNodeType.EndElement:
                    return;
            }
        }
    }

    private void CantRestoreCompositeRole(Guid relationTypeId)
    {
        while (this.reader.Read())
        {
            switch (this.reader.NodeType)
            {
                case XmlNodeType.Element:
                    if (this.reader.Name.Equals(XmlBackup.Relation))
                    {
                        var associationIdString = this.reader.GetAttribute(XmlBackup.Association);
                        var associationId = long.Parse(associationIdString);
                        if (string.IsNullOrEmpty(associationIdString))
                        {
                            throw new Exception("Association id is missing");
                        }

                        if (this.reader.IsEmptyElement)
                        {
                            this.transaction.Database.OnRelationNotRestored(relationTypeId, associationId, null);
                        }
                        else
                        {
                            var value = this.reader.ReadElementContentAsString();
                            foreach (var r in value.Split(XmlBackup.ObjectsSplitterCharArray))
                            {
                                this.transaction.Database.OnRelationNotRestored(relationTypeId, associationId, r);
                            }
                        }
                    }

                    break;

                case XmlNodeType.EndElement:
                    return;
            }
        }
    }
}
