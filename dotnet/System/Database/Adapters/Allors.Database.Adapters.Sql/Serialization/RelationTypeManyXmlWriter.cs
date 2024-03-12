// <copyright file="RelationTypeManyXmlWriter.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System;
using System.Text;
using System.Xml;
using Allors.Database.Meta;

/// <summary>
///     Writes all relations from a <see cref="RelationType" /> with a <see cref="RoleType" /> with multiplicity of many
///     to the <see cref="XmlWriter" /> during a <see cref="IDatabase#Backup" />.
/// </summary>
internal class RelationTypeManyXmlWriter : IDisposable
{
    /// <summary>
    ///     The <see cref="roleType" />.
    /// </summary>
    private readonly RoleType roleType;

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
    ///     The previously written <see cref="Association" /> Id.
    /// </summary>
    private long previousAssociationId;

    /// <summary>
    ///     The <see cref="StringBuilder" /> that accumulates all the roles for one relation.
    /// </summary>
    private StringBuilder rolesStringBuilder;

    /// <summary>
    ///     Initializes a new state of the <see cref="RelationTypeManyXmlWriter" /> class.
    /// </summary>
    /// <param name="roleType">The relation type.</param>
    /// <param name="xmlWriter">The XML writer.</param>
    internal RelationTypeManyXmlWriter(RoleType roleType, XmlWriter xmlWriter)
    {
        this.roleType = roleType;
        this.xmlWriter = xmlWriter;
        this.rolesStringBuilder = new StringBuilder();
        this.previousAssociationId = -1;
        this.isClosed = false;
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() => this.Close();

    /// <summary>
    ///     Closes this "<see cref="RelationTypeManyXmlWriter" />.
    /// </summary>
    internal void Close()
    {
        if (!this.isClosed)
        {
            this.isClosed = true;

            this.WriteRolesIfPresent();

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
    /// <param name="roleId">The role id.</param>
    internal void Write(long associationId, long roleId)
    {
        if (!this.isInUse)
        {
            this.isInUse = true;
            if (this.roleType.ObjectType.IsUnit)
            {
                this.xmlWriter.WriteStartElement(XmlBackup.RelationTypeUnit);
            }
            else
            {
                this.xmlWriter.WriteStartElement(XmlBackup.RelationTypeComposite);
            }

            this.xmlWriter.WriteAttributeString(XmlBackup.Id, this.roleType.Id.ToString());
        }

        if (this.previousAssociationId != associationId)
        {
            this.WriteRolesIfPresent();
            this.rolesStringBuilder = new StringBuilder();
            this.previousAssociationId = associationId;
        }

        if (this.rolesStringBuilder.Length != 0)
        {
            this.rolesStringBuilder.Append(XmlBackup.ObjectsSplitter);
        }

        this.rolesStringBuilder.Append(XmlConvert.ToString(roleId));
    }

    /// <summary>
    ///     Writes the roles if the <see cref="RelationTypeManyXmlWriter#rolesStringBuilder" /> contains accumulated roles.
    /// </summary>
    private void WriteRolesIfPresent()
    {
        if (this.rolesStringBuilder.Length > 0)
        {
            this.xmlWriter.WriteStartElement(XmlBackup.Relation);
            this.xmlWriter.WriteAttributeString(XmlBackup.Association, XmlConvert.ToString(this.previousAssociationId));
            this.xmlWriter.WriteString(this.rolesStringBuilder.ToString());
            this.xmlWriter.WriteEndElement();
        }
    }
}
