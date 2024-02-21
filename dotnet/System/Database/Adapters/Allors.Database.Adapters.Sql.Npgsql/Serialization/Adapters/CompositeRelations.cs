// <copyright file="CompositeRelations.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Allors.Database.Meta;

internal class CompositeRelations : IEnumerable<CompositeRelation>
{
    private readonly Action<XmlReader, Guid> canNotRestoreCompositeRole;
    private readonly Dictionary<long, Class> classByObjectId;
    private readonly Database database;
    private readonly Action<Guid, long, string> onRelationNotRestored;
    private readonly XmlReader reader;
    private readonly RelationType relationType;

    public CompositeRelations(
        Database database,
        RelationType relationType,
        Action<XmlReader, Guid> canNotRestoreCompositeRole,
        Action<Guid, long, string> onRelationNotRestored,
        Dictionary<long, Class> classByObjectId,
        XmlReader reader)
    {
        this.database = database;
        this.relationType = relationType;
        this.canNotRestoreCompositeRole = canNotRestoreCompositeRole;
        this.onRelationNotRestored = onRelationNotRestored;
        this.classByObjectId = classByObjectId;
        this.reader = reader;
    }

    public IEnumerator<CompositeRelation> GetEnumerator()
    {
        var allowedAssociationClasses = new HashSet<Class>(this.relationType.AssociationType.ObjectType.Classes);
        var allowedRoleClasses = new HashSet<Class>(((IComposite)this.relationType.RoleType.ObjectType).Classes);

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

                        this.classByObjectId.TryGetValue(associationId, out var associationClass);

                        if (associationClass == null || !allowedAssociationClasses.Contains(associationClass))
                        {
                            this.canNotRestoreCompositeRole(this.reader.ReadSubtree(), this.relationType.Id);
                        }
                        else
                        {
                            var value = string.Empty;
                            if (!this.reader.IsEmptyElement)
                            {
                                value = this.reader.ReadElementContentAsString();

                                var roleIdsString = value;
                                var roleIdStringArray = roleIdsString.Split(XmlBackup.ObjectsSplitterCharArray);

                                if (this.relationType.RoleType.IsOne && roleIdStringArray.Length != 1)
                                {
                                    foreach (var roleId in roleIdStringArray)
                                    {
                                        this.onRelationNotRestored(this.relationType.Id, associationId, roleId);
                                    }
                                }
                                else if (this.relationType.RoleType.IsOne)
                                {
                                    var roleId = long.Parse(roleIdStringArray[0]);

                                    this.classByObjectId.TryGetValue(roleId, out var roleClass);

                                    if (roleClass == null || !allowedRoleClasses.Contains(roleClass))
                                    {
                                        this.onRelationNotRestored(this.relationType.Id, associationId, roleIdStringArray[0]);
                                    }
                                    else
                                    {
                                        yield return new CompositeRelation(associationId, roleId);
                                    }
                                }
                                else
                                {
                                    foreach (var roleIdString in roleIdStringArray)
                                    {
                                        var roleId = long.Parse(roleIdString);

                                        this.classByObjectId.TryGetValue(roleId, out var roleClass);

                                        if (roleClass == null || !allowedRoleClasses.Contains(roleClass))
                                        {
                                            this.onRelationNotRestored(this.relationType.Id, associationId, roleId.ToString());
                                        }
                                        else
                                        {
                                            yield return new CompositeRelation(associationId, roleId);
                                        }
                                    }
                                }

                                skip = this.reader.IsStartElement();
                            }
                        }
                    }

                    break;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
