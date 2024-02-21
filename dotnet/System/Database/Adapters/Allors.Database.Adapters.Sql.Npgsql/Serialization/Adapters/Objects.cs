// <copyright file="Objects.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Allors.Database.Meta;

public class Objects : IEnumerable<object[]>
{
    private readonly Dictionary<long, Class> classByObjectId;
    private readonly Database database;
    private readonly Action<Guid, long> onObjectNotRestored;
    private readonly XmlReader reader;

    public Objects(
        Database database,
        Action<Guid, long> onObjectNotRestored,
        Dictionary<long, Class> classByObjectId,
        XmlReader reader)
    {
        this.database = database;
        this.onObjectNotRestored = onObjectNotRestored;
        this.classByObjectId = classByObjectId;
        this.reader = reader;
    }

    public IEnumerator<object[]> GetEnumerator()
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
                            var objectType = this.database.ObjectFactory.GetObjectType(objectTypeId);

                            var objectIdsString = this.reader.ReadElementContentAsString();
                            foreach (var objectIdString in objectIdsString.Split(XmlBackup.ObjectsSplitterCharArray))
                            {
                                var objectArray = objectIdString.Split(XmlBackup.ObjectSplitterCharArray);

                                var objectId = long.Parse(objectArray[0]);
                                var objectVersion = objectArray.Length > 1
                                    ? long.Parse(objectArray[1])
                                    : (long)Allors.Version.DatabaseInitial;

                                if (objectType is Class @class)
                                {
                                    this.classByObjectId[objectId] = @class;
                                    yield return new object[] { objectId, @class.Id, objectVersion };
                                }
                                else
                                {
                                    this.onObjectNotRestored(objectTypeId, objectId);
                                }
                            }

                            skip = this.reader.IsStartElement();
                        }
                        else
                        {
                            this.reader.Skip();
                        }
                    }

                    break;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
