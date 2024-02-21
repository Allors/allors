// <copyright file="RelationTypeOneXmlWriter.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IRelationTypeOneXmlWriter type.</summary>

namespace Allors.Database.Adapters.Sql;

using System;
using System.Xml;
using Allors.Database.Meta;

/// <summary>
///     Writes all relations from a <see cref="RelationType" /> with a Role
///     with multiplicity of one  to the <see cref="XmlWriter" /> during a <see cref="IDatabase#Backup" />.
/// </summary>
internal class RelationTypeOneXmlWriter : IDisposable
{
    /// <summary>
    ///     The <see cref="relationType" />.
    /// </summary>
    private readonly RelationType relationType;

    /// <summary>
    ///     The <see cref="xmlWriter" />.
    /// </summary>
    private readonly XmlWriter xmlWriter;

    /// <summary>
    ///     Indicates that this <see cref="RelationTypeOneXmlWriter" /> has been closed.
    /// </summary>
    private bool isClosed;

    /// <summary>
    ///     At least one role was written.
    /// </summary>
    private bool isInUse;

    /// <summary>
    ///     Initializes a new state of the <see cref="RelationTypeOneXmlWriter" /> class.
    /// </summary>
    /// <param name="relationType">Type of the relation.</param>
    /// <param name="xmlWriter">The XML writer.</param>
    internal RelationTypeOneXmlWriter(RelationType relationType, XmlWriter xmlWriter)
    {
        this.relationType = relationType;
        this.xmlWriter = xmlWriter;
        this.isClosed = false;
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() => this.Close();

    /// <summary>
    ///     Closes this "<see cref="RelationTypeOneXmlWriter" />.
    /// </summary>
    internal void Close()
    {
        if (!this.isClosed)
        {
            this.isClosed = true;

            if (this.isInUse)
            {
                this.xmlWriter.WriteEndElement();
            }
        }
    }

    /// <summary>
    ///     Writes the the association and role to the <see cref="xmlWriter" />.
    /// </summary>
    /// <param name="associationId">The association id.</param>
    /// <param name="roleContents">The role contents.</param>
    internal void Write(long associationId, string roleContents)
    {
        if (!this.isInUse)
        {
            this.isInUse = true;
            if (this.relationType.RoleType.ObjectType.IsUnit)
            {
                this.xmlWriter.WriteStartElement(XmlBackup.RelationTypeUnit);
            }
            else
            {
                this.xmlWriter.WriteStartElement(XmlBackup.RelationTypeComposite);
            }

            this.xmlWriter.WriteAttributeString(XmlBackup.Id, this.relationType.Id.ToString());
        }

        this.xmlWriter.WriteStartElement(XmlBackup.Relation);
        this.xmlWriter.WriteAttributeString(XmlBackup.Association, XmlConvert.ToString(associationId));
        this.xmlWriter.WriteString(roleContents);
        this.xmlWriter.WriteEndElement();
    }
}
