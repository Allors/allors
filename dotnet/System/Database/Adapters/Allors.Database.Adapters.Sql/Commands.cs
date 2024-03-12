// <copyright file="Commands.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Allors.Database.Meta;

public abstract class Commands
{
    private readonly IConnection connection;
    private Dictionary<RoleType, ICommand> addCompositeRoleByRoleType;
    private Dictionary<RoleType, ICommand> clearCompositeAndCompositesRoleByRoleType;

    private Dictionary<Class, ICommand> createObjectByClass;
    private Dictionary<Class, ICommand> createObjectsByClass;
    private Dictionary<Class, ICommand> deleteObjectByClass;
    private Dictionary<AssociationType, ICommand> getCompositeAssociationByAssociationType;
    private Dictionary<RoleType, ICommand> getCompositeRoleByRoleType;
    private Dictionary<AssociationType, ICommand> getCompositesAssociationByAssociationType;
    private Dictionary<RoleType, ICommand> getCompositesRoleByRoleType;

    private Dictionary<Class, ICommand> getUnitRolesByClass;

    private ICommand getVersion;

    private ICommand instantiateObject;
    private ICommand instantiateObjects;
    private Dictionary<RoleType, ICommand> removeCompositeRoleByRoleType;
    private Dictionary<RoleType, ICommand> setCompositeRoleByRoleType;
    private Dictionary<Class, Dictionary<RoleType, ICommand>> setUnitRoleByRoleTypeByClass;
    private Dictionary<Class, Dictionary<IList<RoleType>, ICommand>> setUnitRolesByRoleTypeByClass;
    private ICommand updateVersions;

    protected Commands(Transaction transaction, IConnection connection)
    {
        this.Transaction = transaction;
        this.connection = connection;
    }

    public Transaction Transaction { get; }

    private Database Database => this.Transaction.Database;

    internal void ResetCommands()
    {
        this.getUnitRolesByClass = null;
        this.setUnitRoleByRoleTypeByClass = null;

        this.getCompositeRoleByRoleType = null;
        this.setCompositeRoleByRoleType = null;
        this.getCompositesRoleByRoleType = null;
        this.addCompositeRoleByRoleType = null;
        this.removeCompositeRoleByRoleType = null;
        this.clearCompositeAndCompositesRoleByRoleType = null;
        this.getCompositeAssociationByAssociationType = null;
        this.getCompositesAssociationByAssociationType = null;

        this.instantiateObject = null;
        this.instantiateObjects = null;
        this.setUnitRolesByRoleTypeByClass = null;
        this.createObjectByClass = null;
        this.createObjectsByClass = null;
        this.deleteObjectByClass = null;

        this.getVersion = null;
        this.updateVersions = null;
    }

    internal virtual void DeleteObject(Strategy strategy)
    {
        this.deleteObjectByClass ??= new Dictionary<Class, ICommand>();

        var @class = strategy.Class;

        if (!this.deleteObjectByClass.TryGetValue(@class, out var command))
        {
            var sql = this.Database.Mapping.ProcedureNameForDeleteObjectByClass[@class];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.deleteObjectByClass[@class] = command;
        }

        command.ObjectParameter(strategy.ObjectId);
        command.ExecuteNonQuery();
    }

    internal virtual void GetUnitRoles(Strategy strategy)
    {
        this.getUnitRolesByClass ??= new Dictionary<Class, ICommand>();

        var reference = strategy.Reference;
        var @class = reference.Class;

        if (!this.getUnitRolesByClass.TryGetValue(@class, out var command))
        {
            var sql = this.Database.Mapping.ProcedureNameForGetUnitRolesByClass[@class];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.getUnitRolesByClass[@class] = command;
        }

        command.ObjectParameter(reference.ObjectId);

        using (var reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                var sortedUnitRoles = this.Database.GetSortedUnitRolesByObjectType(reference.Class);

                for (var i = 0; i < sortedUnitRoles.Length; i++)
                {
                    var roleType = sortedUnitRoles[i];

                    object unit = null;
                    if (!reader.IsDBNull(i))
                    {
                        switch (((Unit)roleType.ObjectType).Tag)
                        {
                            case UnitTags.String:
                                unit = reader.GetString(i);
                                break;

                            case UnitTags.Integer:
                                unit = reader.GetInt32(i);
                                break;

                            case UnitTags.Float:
                                unit = reader.GetDouble(i);
                                break;

                            case UnitTags.Decimal:
                                unit = reader.GetDecimal(i);
                                break;

                            case UnitTags.DateTime:
                                var dateTime = reader.GetDateTime(i);
                                if (dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
                                {
                                    unit = dateTime;
                                }
                                else
                                {
                                    unit = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute,
                                        dateTime.Second, dateTime.Millisecond, DateTimeKind.Utc);
                                }

                                break;

                            case UnitTags.Boolean:
                                unit = reader.GetBoolean(i);
                                break;

                            case UnitTags.Unique:
                                unit = reader.GetGuid(i);
                                break;

                            case UnitTags.Binary:
                                unit = (byte[])reader.GetValue(i);
                                break;

                            default:
                                throw new ArgumentException("Unknown Unit ObjectType: " + roleType.ObjectType.SingularName);
                        }
                    }

                    strategy.CachedObject.SetValue(roleType, unit);
                }
            }
        }
    }

    internal virtual void SetUnitRole(List<UnitRelation> relations, Class exclusiveRootClass, RoleType roleType)
    {
        this.setUnitRoleByRoleTypeByClass ??= new Dictionary<Class, Dictionary<RoleType, ICommand>>();

        if (!this.setUnitRoleByRoleTypeByClass.TryGetValue(exclusiveRootClass, out var commandByRoleType))
        {
            commandByRoleType = new Dictionary<RoleType, ICommand>();
            this.setUnitRoleByRoleTypeByClass.Add(exclusiveRootClass, commandByRoleType);
        }

        if (!commandByRoleType.TryGetValue(roleType, out var command))
        {
            var sql = this.Database.Mapping.ProcedureNameForSetUnitRoleByRoleTypeByClass[exclusiveRootClass][roleType];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
        }

        command.UnitTableParameter(roleType, relations);

        command.ExecuteNonQuery();
    }

    internal virtual void SetUnitRoles(Strategy strategy, List<RoleType> sortedRoleTypes)
    {
        this.setUnitRolesByRoleTypeByClass ??= new Dictionary<Class, Dictionary<IList<RoleType>, ICommand>>();

        var exclusiveRootClass = strategy.Reference.Class.ExclusiveClass;

        if (!this.setUnitRolesByRoleTypeByClass.TryGetValue(exclusiveRootClass, out var setUnitRoleByRoleType))
        {
            setUnitRoleByRoleType = new Dictionary<IList<RoleType>, ICommand>(new SortedRoleTypeComparer());
            this.setUnitRolesByRoleTypeByClass.Add(exclusiveRootClass, setUnitRoleByRoleType);
        }

        if (!setUnitRoleByRoleType.TryGetValue(sortedRoleTypes, out var command))
        {
            command = this.connection.CreateCommand();
            command.ObjectParameter(strategy.Reference.ObjectId);

            var sql = new StringBuilder();
            sql.Append("UPDATE ").Append(this.Database.Mapping.TableNameForObjectByClass[exclusiveRootClass]).Append(" SET\n");

            var count = 0;
            foreach (var roleType in sortedRoleTypes)
            {
                if (count > 0)
                {
                    sql.Append(" , ");
                }

                ++count;

                var column = this.Database.Mapping.ColumnNameByRoleType[roleType];
                sql.Append(column).Append('=').Append(this.Database.Mapping.ParamInvocationNameByRoleType[roleType]);

                var unit = strategy.EnsureModifiedRoleByRoleType[roleType];
                command.AddUnitRoleParameter(roleType, unit);
            }

            sql.Append("\nWHERE ").Append(Mapping.ColumnNameForObject).Append('=')
                .Append(this.Database.Mapping.ParamInvocationNameForObject).Append('\n');

            command.CommandText = sql.ToString();
            command.ExecuteNonQuery();

            setUnitRoleByRoleType.Add(sortedRoleTypes, command);
        }
        else
        {
            command.ObjectParameter(strategy.Reference.ObjectId);

            foreach (var roleType in sortedRoleTypes)
            {
                var unit = strategy.EnsureModifiedRoleByRoleType[roleType];
                command.AddUnitRoleParameter(roleType, unit);
            }

            command.ExecuteNonQuery();
        }
    }

    internal virtual void GetCompositeRole(Strategy strategy, RoleType roleType)
    {
        this.getCompositeRoleByRoleType ??= new Dictionary<RoleType, ICommand>();

        var reference = strategy.Reference;

        if (!this.getCompositeRoleByRoleType.TryGetValue(roleType, out var command))
        {
            var sql = this.Database.Mapping.ProcedureNameForGetRoleByRoleType[roleType];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.getCompositeRoleByRoleType[roleType] = command;
        }

        command.AddAssociationParameter(reference.ObjectId);

        var result = command.ExecuteScalar();
        if (result == null || result == DBNull.Value)
        {
            strategy.CachedObject.SetValue(roleType, null);
        }
        else
        {
            var objectId = this.Transaction.State.GetObjectIdForExistingObject(result.ToString());
            // TODO: Should add to objectsToRestore
            strategy.CachedObject.SetValue(roleType, objectId);
        }
    }

    internal virtual void SetCompositeRole(List<CompositeRelation> relations, RoleType roleType)
    {
        this.setCompositeRoleByRoleType ??= new Dictionary<RoleType, ICommand>();

        if (!this.setCompositeRoleByRoleType.TryGetValue(roleType, out var command))
        {
            var sql = this.Database.Mapping.ProcedureNameForSetRoleByRoleType[roleType];

            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.setCompositeRoleByRoleType[roleType] = command;
        }

        command.AddCompositeRoleTableParameter(relations);

        command.ExecuteNonQuery();
    }

    internal virtual void GetCompositesRole(Strategy strategy, RoleType roleType)
    {
        this.getCompositesRoleByRoleType ??= new Dictionary<RoleType, ICommand>();

        var reference = strategy.Reference;

        if (!this.getCompositesRoleByRoleType.TryGetValue(roleType, out var command))
        {
            var associationType = roleType.AssociationType;

            string sql;
            if (associationType.IsMany || !roleType.ExistExclusiveClasses)
            {
                sql = this.Database.Mapping.ProcedureNameForGetRoleByRoleType[roleType];
            }
            else
            {
                sql = this.Database.Mapping.ProcedureNameForGetRoleByRoleType[roleType];
            }

            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.getCompositesRoleByRoleType[roleType] = command;
        }

        command.AddAssociationParameter(reference.ObjectId);

        var objectIds = new List<long>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = this.Transaction.State.GetObjectIdForExistingObject(reader[0].ToString());
                objectIds.Add(id);
            }
        }

        strategy.CachedObject.SetValue(roleType, objectIds.ToArray());
    }

    internal virtual void AddCompositeRole(List<CompositeRelation> relations, RoleType roleType)
    {
        this.addCompositeRoleByRoleType ??= new Dictionary<RoleType, ICommand>();

        if (!this.addCompositeRoleByRoleType.TryGetValue(roleType, out var command))
        {
            var sql = this.Database.Mapping.ProcedureNameForAddRoleByRoleType[roleType];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.addCompositeRoleByRoleType[roleType] = command;
        }

        command.AddCompositeRoleTableParameter(relations);

        command.ExecuteNonQuery();
    }

    internal virtual void RemoveCompositeRole(List<CompositeRelation> relations, RoleType roleType)
    {
        this.removeCompositeRoleByRoleType ??= new Dictionary<RoleType, ICommand>();

        if (!this.removeCompositeRoleByRoleType.TryGetValue(roleType, out var command))
        {
            var sql = this.Database.Mapping.ProcedureNameForRemoveRoleByRoleType[roleType];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.removeCompositeRoleByRoleType[roleType] = command;
        }

        command.AddCompositeRoleTableParameter(relations);

        command.ExecuteNonQuery();
    }

    internal virtual void ClearCompositeAndCompositesRole(IList<long> associations, RoleType roleType)
    {
        this.clearCompositeAndCompositesRoleByRoleType ??= new Dictionary<RoleType, ICommand>();

        var sql = this.Database.Mapping.ProcedureNameForClearRoleByRoleType[roleType];

        if (!this.clearCompositeAndCompositesRoleByRoleType.TryGetValue(roleType, out var command))
        {
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.clearCompositeAndCompositesRoleByRoleType[roleType] = command;
        }

        command.ObjectTableParameter(associations);

        command.ExecuteNonQuery();
    }

    internal virtual Reference GetCompositeAssociation(Reference role, AssociationType associationType)
    {
        this.getCompositeAssociationByAssociationType ??= new Dictionary<AssociationType, ICommand>();

        Reference associationObject = null;

        if (!this.getCompositeAssociationByAssociationType.TryGetValue(associationType, out var command))
        {
            var roleType = associationType.RoleType;
            var sql = this.Database.Mapping.ProcedureNameForGetAssociationByRoleType[roleType];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.getCompositeAssociationByAssociationType[associationType] = command;
        }

        command.AddCompositeRoleParameter(role.ObjectId);

        var result = command.ExecuteScalar();

        if (result != null && result != DBNull.Value)
        {
            var id = this.Transaction.State.GetObjectIdForExistingObject(result.ToString());

            associationObject = associationType.Composite.ExclusiveClass != null
                ? this.Transaction.State.GetOrCreateReferenceForExistingObject(associationType.Composite.ExclusiveClass, id,
                    this.Transaction)
                : this.Transaction.State.GetOrCreateReferenceForExistingObject(id, this.Transaction);
        }

        return associationObject;
    }

    internal virtual long[] GetCompositesAssociation(Strategy role, AssociationType associationType)
    {
        this.getCompositesAssociationByAssociationType ??= new Dictionary<AssociationType, ICommand>();

        if (!this.getCompositesAssociationByAssociationType.TryGetValue(associationType, out var command))
        {
            var roleType = associationType.RoleType;
            var sql = this.Database.Mapping.ProcedureNameForGetAssociationByRoleType[roleType];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.getCompositesAssociationByAssociationType[associationType] = command;
        }

        command.AddCompositeRoleParameter(role.ObjectId);

        var objectIds = new List<long>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = this.Transaction.State.GetObjectIdForExistingObject(reader[0].ToString());
                objectIds.Add(id);
            }
        }

        return [.. objectIds];
    }

    internal virtual Reference CreateObject(Class @class)
    {
        this.createObjectByClass ??= new Dictionary<Class, ICommand>();

        if (!this.createObjectByClass.TryGetValue(@class, out var command))
        {
            var sql = this.Database.Mapping.ProcedureNameForCreateObjectByClass[@class];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.createObjectByClass[@class] = command;
        }

        command.AddTypeParameter(@class);

        var result = command.ExecuteScalar();
        var objectId = long.Parse(result.ToString());
        return this.Transaction.State.CreateReferenceForNewObject(@class, objectId, this.Transaction);
    }

    internal virtual IList<Reference> CreateObjects(Class @class, int count)
    {
        this.createObjectsByClass ??= new Dictionary<Class, ICommand>();

        if (!this.createObjectsByClass.TryGetValue(@class, out var command))
        {
            var sql = this.Database.Mapping.ProcedureNameForCreateObjectsByClass[@class];
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandType = CommandType.StoredProcedure;
            this.createObjectsByClass[@class] = command;
        }

        command.AddTypeParameter(@class);
        command.AddCountParameter(count);

        var objectIds = new List<object>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                object id = long.Parse(reader[0].ToString());
                objectIds.Add(id);
            }
        }

        var strategies = new List<Reference>();

        foreach (var id in objectIds)
        {
            var objectId = long.Parse(id.ToString());
            var strategySql = this.Transaction.State.CreateReferenceForNewObject(@class, objectId, this.Transaction);
            strategies.Add(strategySql);
        }

        return strategies;
    }

    internal virtual Reference InstantiateObject(long objectId)
    {
        var command = this.instantiateObject;
        if (command == null)
        {
            var sql =
                @$"SELECT {Mapping.ColumnNameForClass}, {Mapping.ColumnNameForVersion}
FROM {this.Database.Mapping.TableNameForObjects}
WHERE {Mapping.ColumnNameForObject}={this.Database.Mapping.ParamInvocationNameForObject}
";

            command = this.connection.CreateCommand();
            command.CommandText = sql;
            this.instantiateObject = command;
        }

        command.ObjectParameter(objectId);

        using (var reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                var classId = reader.GetGuid(0);
                var version = reader.GetInt64(1);

                var type = (Class)this.Database.MetaPopulation.FindById(classId);
                return this.Transaction.State.GetOrCreateReferenceForExistingObject(type, objectId, version, this.Transaction);
            }

            return null;
        }
    }

    internal virtual IEnumerable<Reference> InstantiateReferences(IEnumerable<long> objectIds)
    {
        var command = this.instantiateObjects;
        if (command == null)
        {
            var sql = this.Database.Mapping.ProcedureNameForInstantiate;
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.instantiateObjects = command;
        }

        command.ObjectTableParameter(objectIds);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var objectIdString = reader.GetValue(0).ToString();
                var classId = reader.GetGuid(1);
                var version = reader.GetInt64(2);

                var objectId = long.Parse(objectIdString);
                var type = (Class)this.Database.ObjectFactory.GetObjectType(classId);

                yield return this.Transaction.State.GetOrCreateReferenceForExistingObject(type, objectId, version, this.Transaction);
            }
        }
    }

    internal virtual Dictionary<long, long> GetVersions(ISet<Reference> references)
    {
        var command = this.getVersion;

        if (command == null)
        {
            var sql = this.Database.Mapping.ProcedureNameForGetVersion;
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.getVersion = command;
        }

        command.AddCompositesRoleTableParameter(references.Select(v => v.ObjectId));

        var versionByObjectId = new Dictionary<long, long>();

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var objectId = long.Parse(reader[0].ToString());
                var version = reader.GetInt64(1);

                versionByObjectId.Add(objectId, version);
            }
        }

        return versionByObjectId;
    }

    internal virtual void UpdateVersion(IEnumerable<long> changed)
    {
        var command = this.updateVersions;
        if (command == null)
        {
            var sql = this.Database.Mapping.ProcedureNameForUpdateVersion;
            command = this.connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.StoredProcedure;
            this.updateVersions = command;
        }

        // TODO: Remove dependency on State
        command.ObjectTableParameter(changed);
        command.ExecuteNonQuery();
    }

    private class SortedRoleTypeComparer : IEqualityComparer<IList<RoleType>>
    {
        public bool Equals(IList<RoleType> x, IList<RoleType> y)
        {
            if (x.Count == y.Count)
            {
                for (var i = 0; i < x.Count; i++)
                {
                    if (!x[i].Equals(y[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public int GetHashCode(IList<RoleType> roleTypes)
        {
            var hashCode = 0;
            foreach (var roleType in roleTypes)
            {
                hashCode ^= roleType.GetHashCode();
            }

            return hashCode;
        }
    }
}
