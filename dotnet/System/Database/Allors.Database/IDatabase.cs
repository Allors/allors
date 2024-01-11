// <copyright file="IDatabase.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database;

using System.Xml;
using Allors.Database.Meta;
using Allors.Database.Tracing;

/// <summary>
///     A database is an online <see cref="IDatabase" />.
/// </summary>
public interface IDatabase
{
    /// <summary>
    ///     Gets a value indicating whether this database is shared with other databases with the same name.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this database is shared; otherwise, <c>false</c>.
    /// </value>
    bool IsShared { get; }

    /// <summary>
    ///     Gets.
    ///     <ul>
    ///         <li>the id of this database</li>
    ///         <li>the id of the database from this workspace</li>
    ///     </ul>
    /// </summary>
    string Id { get; }

    /// <summary>
    ///     Gets the object factory.
    /// </summary>
    /// <value>The object factory.</value>
    IObjectFactory ObjectFactory { get; }

    /// <summary>
    ///     Gets the meta domain of this population.
    /// </summary>
    IMetaPopulation MetaPopulation { get; }

    IDatabaseServices Services { get; }

    public ISink Sink { get; set; }

    /// <summary>
    ///     Occurs when an object could not be restored.
    /// </summary>
    event ObjectNotRestoredEventHandler ObjectNotRestored;

    /// <summary>
    ///     Occurs when a relation could not be restored.
    /// </summary>
    event RelationNotRestoredEventHandler RelationNotRestored;

    /// <summary>
    ///     Initializes the database. If this population is persistent then
    ///     all existing objects will be deleted.
    /// </summary>
    void Init();

    /// <summary>
    ///     Creates a new database Transaction.
    /// </summary>
    /// <returns>a newly created Transaction.</returns>
    ITransaction CreateTransaction();

    /// <summary>
    ///     Restores the database from the backup.
    /// </summary>
    /// <param name="reader">The reader.</param>
    void Restore(XmlReader reader);

    /// <summary>
    ///     Creates a backup of the database. 
    /// </summary>
    /// <param name="writer">The writer.</param>
    void Backup(XmlWriter writer);
}
