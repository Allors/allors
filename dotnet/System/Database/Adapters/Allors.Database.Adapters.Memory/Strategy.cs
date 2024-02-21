// <copyright file="Strategy.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Allors.Database.Meta;

public sealed class Strategy : IStrategy
{
    private readonly Dictionary<AssociationType, Strategy> compositeAssociationByAssociationType;
    private readonly Dictionary<RoleType, Strategy> compositeRoleByRoleType;
    private readonly Dictionary<AssociationType, HashSet<Strategy>> compositesAssociationByAssociationType;
    private readonly Dictionary<RoleType, HashSet<Strategy>> compositesRoleByRoleType;
    private readonly Dictionary<RoleType, object> unitRoleByRoleType;
    private WeakReference allorizedObjectWeakReference;
    private bool isDeletedOnRollback;
    private Dictionary<AssociationType, Strategy> rollbackCompositeAssociationByAssociationType;
    private Dictionary<RoleType, Strategy> rollbackCompositeRoleByRoleType;
    private Dictionary<AssociationType, HashSet<Strategy>> rollbackCompositesAssociationByAssociationType;
    private Dictionary<RoleType, HashSet<Strategy>> rollbackCompositesRoleByRoleType;

    private Dictionary<RoleType, object> rollbackUnitRoleByRoleType;

    internal Strategy(Transaction transaction, Class objectType, long objectId, long version)
    {
        this.Transaction = transaction;
        this.UncheckedObjectType = objectType;
        this.ObjectId = objectId;

        this.IsDeleted = false;
        this.isDeletedOnRollback = true;
        this.IsNewInTransaction = true;

        this.ObjectVersion = version;

        this.unitRoleByRoleType = new Dictionary<RoleType, object>();
        this.compositeRoleByRoleType = new Dictionary<RoleType, Strategy>();
        this.compositesRoleByRoleType = new Dictionary<RoleType, HashSet<Strategy>>();
        this.compositeAssociationByAssociationType = new Dictionary<AssociationType, Strategy>();
        this.compositesAssociationByAssociationType = new Dictionary<AssociationType, HashSet<Strategy>>();

        this.rollbackUnitRoleByRoleType = null;
        this.rollbackCompositeRoleByRoleType = null;
        this.rollbackCompositesRoleByRoleType = null;
        this.rollbackCompositeAssociationByAssociationType = null;
        this.rollbackCompositesAssociationByAssociationType = null;
    }

    internal Class UncheckedObjectType { get; }

    internal Transaction Transaction { get; }

    private ChangeLog ChangeLog => this.Transaction.ChangeLog;

    private Dictionary<RoleType, object> RollbackUnitRoleByRoleType =>
        this.rollbackUnitRoleByRoleType ??= new Dictionary<RoleType, object>();

    private Dictionary<RoleType, Strategy> RollbackCompositeRoleByRoleType =>
        this.rollbackCompositeRoleByRoleType ??= new Dictionary<RoleType, Strategy>();

    private Dictionary<RoleType, HashSet<Strategy>> RollbackCompositesRoleByRoleType =>
        this.rollbackCompositesRoleByRoleType ??= new Dictionary<RoleType, HashSet<Strategy>>();

    private Dictionary<AssociationType, Strategy> RollbackCompositeAssociationByAssociationType =>
        this.rollbackCompositeAssociationByAssociationType ??= new Dictionary<AssociationType, Strategy>();

    private Dictionary<AssociationType, HashSet<Strategy>> RollbackCompositesAssociationByAssociationType =>
        this.rollbackCompositesAssociationByAssociationType ??= new Dictionary<AssociationType, HashSet<Strategy>>();

    public bool IsDeleted { get; private set; }

    public bool IsNewInTransaction { get; private set; }

    public long ObjectId { get; }

    public long ObjectVersion { get; private set; }

    public Class Class
    {
        get
        {
            this.AssertNotDeleted();
            return this.UncheckedObjectType;
        }
    }

    ITransaction IStrategy.Transaction => this.Transaction;

    public object GetRole(RoleType roleType) =>
        roleType switch
        {
            { } unitRole when unitRole.ObjectType.IsUnit => this.GetUnitRole(roleType),
            { } compositeRole when compositeRole.IsOne => this.GetCompositeRole(roleType),
            _ => this.GetCompositesRole<IObject>(roleType),
        };

    public void SetRole(RoleType roleType, object value)
    {
        switch (roleType)
        {
            case { } unitRole when unitRole.ObjectType.IsUnit:
                this.SetUnitRole(roleType, value);
                break;
            case { } compositeRole when compositeRole.IsOne:
                this.SetCompositeRole(roleType, (IObject)value);
                break;
            default:
                this.SetCompositesRole(roleType, (IEnumerable<IObject>)value);
                break;
        }
    }

    public void RemoveRole(RoleType roleType)
    {
        switch (roleType)
        {
            case { } unitRole when unitRole.ObjectType.IsUnit:
                this.RemoveUnitRole(roleType);
                break;
            case { } compositeRole when compositeRole.IsOne:
                this.RemoveCompositeRole(roleType);
                break;
            default:
                this.RemoveCompositesRole(roleType);
                break;
        }
    }

    public bool ExistRole(RoleType roleType) =>
        roleType switch
        {
            { } unitRole when unitRole.ObjectType.IsUnit => this.ExistUnitRole(roleType),
            { } compositeRole when compositeRole.IsOne => this.ExistCompositeRole(roleType),
            _ => this.ExistCompositesRole(roleType),
        };

    public object GetUnitRole(RoleType roleType)
    {
        this.AssertNotDeleted();
        return this.GetInternalizedUnitRole(roleType);
    }

    public void SetUnitRole(RoleType roleType, object role)
    {
        this.AssertNotDeleted();
        this.Transaction.Database.UnitRoleChecks(this, roleType);

        var previousRole = this.GetInternalizedUnitRole(roleType);
        role = roleType.Normalize(role);

        if (Equals(role, previousRole))
        {
            return;
        }

        if (!this.RollbackUnitRoleByRoleType.ContainsKey(roleType))
        {
            this.RollbackUnitRoleByRoleType[roleType] = this.GetInternalizedUnitRole(roleType);
        }

        this.ChangeLog.OnChangingUnitRole(this, roleType, previousRole);

        switch (role)
        {
            case null:
                this.unitRoleByRoleType.Remove(roleType);
                break;
            default:
                this.unitRoleByRoleType[roleType] = role;
                break;
        }
    }

    public void RemoveUnitRole(RoleType roleType) => this.SetUnitRole(roleType, null);

    public bool ExistUnitRole(RoleType roleType)
    {
        this.AssertNotDeleted();
        return this.unitRoleByRoleType.ContainsKey(roleType);
    }

    public IObject GetCompositeRole(RoleType roleType)
    {
        this.AssertNotDeleted();
        this.compositeRoleByRoleType.TryGetValue(roleType, out var strategy);
        return strategy?.GetObject();
    }

    public void SetCompositeRole(RoleType roleType, IObject newRole)
    {
        if (newRole == null)
        {
            this.RemoveCompositeRole(roleType);
        }
        else if (roleType.AssociationType.IsOne)
        {
            // 1-1
            this.SetCompositeRoleOne2One(roleType, (Strategy)newRole.Strategy);
        }
        else
        {
            // *-1
            this.SetCompositeRoleMany2One(roleType, (Strategy)newRole.Strategy);
        }
    }

    public void RemoveCompositeRole(RoleType roleType)
    {
        if (roleType.AssociationType.IsOne)
        {
            // 1-1
            this.RemoveCompositeRoleOne2One(roleType);
        }
        else
        {
            // *-1
            this.RemoveCompositeRoleMany2One(roleType);
        }
    }

    public bool ExistCompositeRole(RoleType roleType)
    {
        this.AssertNotDeleted();
        return this.compositeRoleByRoleType.ContainsKey(roleType);
    }

    public IEnumerable<T> GetCompositesRole<T>(RoleType roleType) where T : IObject
    {
        this.AssertNotDeleted();

        this.compositesRoleByRoleType.TryGetValue(roleType, out var strategies);

        if (strategies == null)
        {
            yield break;
        }

        foreach (var strategy in strategies.ToArray())
        {
            yield return (T)strategy.GetObject();
        }
    }

    public void SetCompositesRole(RoleType roleType, IEnumerable<IObject> roles)
    {
        if (roles == null || (roles is ICollection<IObject> collection && collection.Count == 0))
        {
            this.RemoveCompositesRole(roleType);
        }
        else
        {
            var strategies = roles
                .Where(v => v != null)
                .Select(v => this.Transaction.Database.CompositeRolesChecks(this, roleType, (Strategy)v.Strategy))
                .Distinct();

            if (roleType.AssociationType.IsMany)
            {
                this.SetCompositesRolesMany2Many(roleType, strategies);
            }
            else
            {
                this.SetCompositesRolesOne2Many(roleType, strategies);
            }
        }
    }

    public void AddCompositesRole(RoleType roleType, IObject role)
    {
        this.AssertNotDeleted();
        if (role == null)
        {
            return;
        }

        var roleStrategy = this.Transaction.Database.CompositeRolesChecks(this, roleType, (Strategy)role.Strategy);

        if (roleType.AssociationType.IsMany)
        {
            this.AddCompositeRoleMany2Many(roleType, roleStrategy);
        }
        else
        {
            this.AddCompositeRoleOne2Many(roleType, roleStrategy);
        }
    }

    public void RemoveCompositesRole(RoleType roleType, IObject role)
    {
        this.AssertNotDeleted();

        if (role == null)
        {
            return;
        }

        var roleStrategy = this.Transaction.Database.CompositeRolesChecks(this, roleType, (Strategy)role.Strategy);

        if (roleType.AssociationType.IsMany)
        {
            this.RemoveCompositeRoleMany2Many(roleType, roleStrategy);
        }
        else
        {
            this.RemoveCompositeRoleOne2Many(roleType, roleStrategy);
        }
    }

    public void RemoveCompositesRole(RoleType roleType)
    {
        this.AssertNotDeleted();

        if (roleType.AssociationType.IsMany)
        {
            this.RemoveCompositeRolesMany2Many(roleType);
        }
        else
        {
            this.RemoveCompositeRolesOne2Many(roleType);
        }
    }

    public bool ExistCompositesRole(RoleType roleType)
    {
        this.AssertNotDeleted();
        this.compositesRoleByRoleType.TryGetValue(roleType, out var roleStrategies);
        return roleStrategies != null;
    }

    public object GetAssociation(AssociationType associationType) => associationType.IsMany
        ? this.GetCompositesAssociation<IObject>(associationType)
        : this.GetCompositeAssociation(associationType);

    public bool ExistAssociation(AssociationType associationType) => associationType.IsMany
        ? this.ExistCompositesAssociation(associationType)
        : this.ExistCompositeAssociation(associationType);

    public IObject GetCompositeAssociation(AssociationType associationType)
    {
        this.AssertNotDeleted();
        this.compositeAssociationByAssociationType.TryGetValue(associationType, out var strategy);
        return strategy?.GetObject();
    }

    public bool ExistCompositeAssociation(AssociationType associationType) => this.GetCompositeAssociation(associationType) != null;

    public IEnumerable<T> GetCompositesAssociation<T>(AssociationType associationType) where T : IObject
    {
        this.AssertNotDeleted();

        this.compositesAssociationByAssociationType.TryGetValue(associationType, out var strategies);

        if (strategies == null)
        {
            yield break;
        }

        foreach (var strategy in strategies.ToArray())
        {
            yield return (T)strategy.GetObject();
        }
    }

    public bool ExistCompositesAssociation(AssociationType associationType)
    {
        this.AssertNotDeleted();
        this.compositesAssociationByAssociationType.TryGetValue(associationType, out var strategies);
        return strategies != null;
    }

    public void Delete()
    {
        this.AssertNotDeleted();

        // Roles
        foreach (var roleType in this.UncheckedObjectType.RoleTypes)
        {
            if (this.ExistRole(roleType))
            {
                if (roleType.ObjectType is Unit)
                {
                    this.RemoveUnitRole(roleType);
                }
                else
                {
                    var associationType = roleType.AssociationType;
                    if (associationType.IsMany)
                    {
                        if (roleType.IsMany)
                        {
                            this.RemoveCompositeRolesMany2Many(roleType);
                        }
                        else
                        {
                            this.RemoveCompositeRoleMany2One(roleType);
                        }
                    }
                    else if (roleType.IsMany)
                    {
                        this.RemoveCompositeRolesOne2Many(roleType);
                    }
                    else
                    {
                        this.RemoveCompositeRoleOne2One(roleType);
                    }
                }
            }
        }

        // Associations
        foreach (var associationType in this.UncheckedObjectType.AssociationTypes)
        {
            var roleType = associationType.RoleType;

            if (this.ExistAssociation(associationType))
            {
                if (associationType.IsMany)
                {
                    this.compositesAssociationByAssociationType.TryGetValue(associationType, out var associationStrategies);

                    // TODO: Optimize
                    if (associationStrategies != null)
                    {
                        foreach (var associationStrategy in new HashSet<Strategy>(associationStrategies))
                        {
                            if (roleType.IsMany)
                            {
                                associationStrategy.RemoveCompositeRoleMany2Many(roleType, this);
                            }
                            else
                            {
                                associationStrategy.RemoveCompositeRoleMany2One(roleType);
                            }
                        }
                    }
                }
                else
                {
                    this.compositeAssociationByAssociationType.TryGetValue(associationType, out var associationStrategy);

                    if (associationStrategy != null)
                    {
                        if (roleType.IsMany)
                        {
                            associationStrategy.RemoveCompositeRoleOne2Many(roleType, this);
                        }
                        else
                        {
                            associationStrategy.RemoveCompositeRoleOne2One(roleType);
                        }
                    }
                }
            }
        }

        this.IsDeleted = true;

        this.ChangeLog.OnDeleted(this);
    }

    public IObject GetObject()
    {
        IObject allorsObject;
        if (this.allorizedObjectWeakReference == null)
        {
            allorsObject = this.Transaction.Database.ObjectFactory.Create(this);
            this.allorizedObjectWeakReference = new WeakReference(allorsObject);
        }
        else
        {
            allorsObject = (IObject)this.allorizedObjectWeakReference.Target;
            if (allorsObject == null)
            {
                allorsObject = this.Transaction.Database.ObjectFactory.Create(this);
                this.allorizedObjectWeakReference.Target = allorsObject;
            }
        }

        return allorsObject;
    }

    public override string ToString() => this.UncheckedObjectType.SingularName + " " + this.ObjectId;

    internal void Commit()
    {
        if (!this.IsDeleted && !this.Transaction.Database.IsRestoring)
        {
            // TODO: Test
            /*
            if (this.rollbackUnitRoleByRoleType != null ||
                this.rollbackCompositeRoleByRoleType != null ||
                this.rollbackCompositeRoleByRoleType != null ||
                this.rollbackCompositeRoleByRoleType != null ||
                this.rollbackCompositeRoleByRoleType != null ||
                this.rollbackCompositeRoleByRoleType != null)
            {
                ++this.ObjectVersion;
            }
            */

            if (this.rollbackUnitRoleByRoleType != null ||
                this.rollbackCompositeRoleByRoleType != null ||
                this.rollbackCompositesRoleByRoleType != null)
            {
                ++this.ObjectVersion;
            }
        }

        this.rollbackUnitRoleByRoleType = null;
        this.rollbackCompositeRoleByRoleType = null;
        this.rollbackCompositesRoleByRoleType = null;
        this.rollbackCompositeAssociationByAssociationType = null;
        this.rollbackCompositesAssociationByAssociationType = null;

        this.isDeletedOnRollback = this.IsDeleted;
        this.IsNewInTransaction = false;
    }

    internal void Rollback()
    {
        foreach (var dictionaryItem in this.RollbackUnitRoleByRoleType)
        {
            var roleType = dictionaryItem.Key;
            var role = dictionaryItem.Value;

            if (role != null)
            {
                this.unitRoleByRoleType[roleType] = role;
            }
            else
            {
                this.unitRoleByRoleType.Remove(roleType);
            }
        }

        foreach (var dictionaryItem in this.RollbackCompositeRoleByRoleType)
        {
            var roleType = dictionaryItem.Key;
            var role = dictionaryItem.Value;

            if (role != null)
            {
                this.compositeRoleByRoleType[roleType] = role;
            }
            else
            {
                this.compositeRoleByRoleType.Remove(roleType);
            }
        }

        foreach (var dictionaryItem in this.RollbackCompositesRoleByRoleType)
        {
            var roleType = dictionaryItem.Key;
            var role = dictionaryItem.Value;

            if (role != null)
            {
                this.compositesRoleByRoleType[roleType] = role;
            }
            else
            {
                this.compositesRoleByRoleType.Remove(roleType);
            }
        }

        foreach (var dictionaryItem in this.RollbackCompositeAssociationByAssociationType)
        {
            var associationType = dictionaryItem.Key;
            var association = dictionaryItem.Value;

            if (association != null)
            {
                this.compositeAssociationByAssociationType[associationType] = association;
            }
            else
            {
                this.compositeAssociationByAssociationType.Remove(associationType);
            }
        }

        foreach (var dictionaryItem in this.RollbackCompositesAssociationByAssociationType)
        {
            var associationType = dictionaryItem.Key;
            var association = dictionaryItem.Value;

            if (association != null)
            {
                this.compositesAssociationByAssociationType[associationType] = association;
            }
            else
            {
                this.compositesAssociationByAssociationType.Remove(associationType);
            }
        }

        this.rollbackUnitRoleByRoleType = null;
        this.rollbackCompositeRoleByRoleType = null;
        this.rollbackCompositesRoleByRoleType = null;
        this.rollbackCompositeAssociationByAssociationType = null;
        this.rollbackCompositesAssociationByAssociationType = null;

        this.IsDeleted = this.isDeletedOnRollback;
        this.IsNewInTransaction = false;
    }

    internal object GetInternalizedUnitRole(RoleType roleType)
    {
        this.unitRoleByRoleType.TryGetValue(roleType, out var unitRole);
        return unitRole;
    }

    internal void SetCompositeRoleOne2One(RoleType roleType, Strategy @new)
    {
        this.AssertNotDeleted();
        this.Transaction.Database.CompositeRoleChecks(this, roleType, @new);

        this.compositeRoleByRoleType.TryGetValue(roleType, out var previousRole);

        if (!@new.Equals(previousRole))
        {
            this.ChangeLog.OnChangingCompositeRole(this, roleType, @new, previousRole);

            var associationType = roleType.AssociationType;

            if (previousRole != null)
            {
                previousRole.compositeAssociationByAssociationType.TryGetValue(associationType, out var previousRoleAssociation);
                this.ChangeLog.OnChangingCompositeAssociation(previousRole, associationType, previousRoleAssociation);

                // previous role
                previousRole.Backup(associationType);
                previousRole.compositeAssociationByAssociationType.Remove(associationType);
            }

            // previous association of newRole
            @new.compositeAssociationByAssociationType.TryGetValue(roleType.AssociationType, out var newPreviousAssociation);

            this.ChangeLog.OnChangingCompositeAssociation(@new, associationType, newPreviousAssociation);

            if (newPreviousAssociation != null && !this.Equals(newPreviousAssociation))
            {
                this.ChangeLog.OnChangingCompositeRole(newPreviousAssociation, roleType, null, previousRole);

                newPreviousAssociation.Backup(roleType);
                newPreviousAssociation.compositeRoleByRoleType.Remove(roleType);
            }

            // Set new role
            this.Backup(roleType);
            this.compositeRoleByRoleType[roleType] = @new;

            // Set new role's association
            @new.Backup(associationType);
            @new.compositeAssociationByAssociationType[associationType] = this;
        }
    }

    internal void SetCompositeRoleMany2One(RoleType roleType, Strategy @new)
    {
        this.AssertNotDeleted();
        this.Transaction.Database.CompositeRoleChecks(this, roleType, @new);

        this.compositeRoleByRoleType.TryGetValue(roleType, out var previousRole);

        if (!@new.Equals(previousRole))
        {
            this.ChangeLog.OnChangingCompositeRole(this, roleType, @new, previousRole);

            var associationType = roleType.AssociationType;

            // Update association of previous role
            if (previousRole != null)
            {
                previousRole.compositesAssociationByAssociationType.TryGetValue(associationType, out var previousRoleAssociation);
                this.ChangeLog.OnChangingCompositesAssociation(previousRole, associationType, previousRoleAssociation);

                previousRole.Backup(associationType);
                previousRoleAssociation.Remove(this);

                if (previousRoleAssociation.Count == 0)
                {
                    previousRole.compositesAssociationByAssociationType.Remove(associationType);
                }
            }

            this.Backup(roleType);
            this.compositeRoleByRoleType[roleType] = @new;

            @new.compositesAssociationByAssociationType.TryGetValue(associationType, out var previousAssociations);

            this.ChangeLog.OnChangingCompositesAssociation(@new, associationType, previousAssociations);

            @new.Backup(associationType);
            if (previousAssociations == null)
            {
                previousAssociations = new HashSet<Strategy>();
                @new.compositesAssociationByAssociationType[associationType] = previousAssociations;
            }

            previousAssociations.Add(this);
        }
    }

    internal void SetCompositesRolesOne2Many(RoleType roleType, IEnumerable<Strategy> roles)
    {
        this.AssertNotDeleted();

        this.compositesRoleByRoleType.TryGetValue(roleType, out var originalRoles);

        if (originalRoles == null || originalRoles.Count == 0)
        {
            foreach (var role in roles)
            {
                this.AddCompositeRoleOne2Many(roleType, role);
            }
        }
        else
        {
            ISet<Strategy> toRemove = new HashSet<Strategy>(originalRoles);

            foreach (var role in roles)
            {
                if (toRemove.Contains(role))
                {
                    toRemove.Remove(role);
                }
                else
                {
                    this.AddCompositeRoleOne2Many(roleType, role);
                }
            }

            foreach (var strategy in toRemove)
            {
                this.RemoveCompositeRoleOne2Many(roleType, strategy);
            }
        }
    }

    internal void SetCompositesRolesMany2Many(RoleType roleType, IEnumerable<Strategy> roles)
    {
        this.AssertNotDeleted();

        this.compositesRoleByRoleType.TryGetValue(roleType, out var originalRoles);

        if (originalRoles == null || originalRoles.Count == 0)
        {
            foreach (var role in roles)
            {
                this.AddCompositeRoleMany2Many(roleType, role);
            }
        }
        else
        {
            ISet<Strategy> toRemove = new HashSet<Strategy>(originalRoles);

            foreach (var role in roles)
            {
                if (toRemove.Contains(role))
                {
                    toRemove.Remove(role);
                }
                else
                {
                    this.AddCompositeRoleMany2Many(roleType, role);
                }
            }

            foreach (var strategy in toRemove)
            {
                this.RemoveCompositeRoleMany2Many(roleType, strategy);
            }
        }
    }

    internal void FillRoleForBackup(Dictionary<RoleType, List<Strategy>> strategiesByRoleType)
    {
        if (this.IsDeleted)
        {
            return;
        }

        if (this.unitRoleByRoleType != null)
        {
            foreach (var dictionaryEntry in this.unitRoleByRoleType)
            {
                var roleType = dictionaryEntry.Key;

                if (!strategiesByRoleType.TryGetValue(roleType, out var strategies))
                {
                    strategies = new List<Strategy>();
                    strategiesByRoleType.Add(roleType, strategies);
                }

                strategies.Add(this);
            }
        }

        if (this.compositeRoleByRoleType != null)
        {
            foreach (var dictionaryEntry in this.compositeRoleByRoleType)
            {
                var roleType = dictionaryEntry.Key;

                if (!strategiesByRoleType.TryGetValue(roleType, out var strategies))
                {
                    strategies = new List<Strategy>();
                    strategiesByRoleType.Add(roleType, strategies);
                }

                strategies.Add(this);
            }
        }

        if (this.compositesRoleByRoleType != null)
        {
            foreach (var dictionaryEntry in this.compositesRoleByRoleType)
            {
                var roleType = dictionaryEntry.Key;

                if (!strategiesByRoleType.TryGetValue(roleType, out var strategies))
                {
                    strategies = new List<Strategy>();
                    strategiesByRoleType.Add(roleType, strategies);
                }

                strategies.Add(this);
            }
        }
    }

    internal void BackupUnit(XmlWriter writer, RoleType roleType)
    {
        var unitType = (Unit)roleType.ObjectType;
        var value = XmlBackup.WriteString(unitType.Tag, this.unitRoleByRoleType[roleType]);

        writer.WriteStartElement(XmlBackup.Relation);
        writer.WriteAttributeString(XmlBackup.Association, this.ObjectId.ToString());
        writer.WriteString(value);
        writer.WriteEndElement();
    }

    internal void BackupComposites(XmlWriter writer, RoleType roleType)
    {
        writer.WriteStartElement(XmlBackup.Relation);
        writer.WriteAttributeString(XmlBackup.Association, this.ObjectId.ToString());

        var roleStragies = this.compositesRoleByRoleType[roleType];
        var i = 0;
        foreach (var roleStrategy in roleStragies)
        {
            if (i > 0)
            {
                writer.WriteString(XmlBackup.ObjectsSplitter);
            }

            writer.WriteString(roleStrategy.ObjectId.ToString());
            ++i;
        }

        writer.WriteEndElement();
    }

    internal void BackupComposite(XmlWriter writer, RoleType roleType)
    {
        writer.WriteStartElement(XmlBackup.Relation);
        writer.WriteAttributeString(XmlBackup.Association, this.ObjectId.ToString());

        var roleStragy = this.compositeRoleByRoleType[roleType];
        writer.WriteString(roleStragy.ObjectId.ToString());

        writer.WriteEndElement();
    }

    internal bool ShouldTrim(RoleType roleType, object originalRole)
    {
        var role = this.GetInternalizedUnitRole(roleType);
        return Equals(role, originalRole);
    }

    internal bool ShouldTrim(RoleType roleType, Strategy originalRole)
    {
        this.compositeRoleByRoleType.TryGetValue(roleType, out var role);
        return Equals(role, originalRole);
    }

    internal bool ShouldTrim(RoleType roleType, Strategy[] originalRole)
    {
        this.compositesRoleByRoleType.TryGetValue(roleType, out var role);

        if (role == null)
        {
            return originalRole == null || originalRole.Length == 0;
        }

        if (originalRole == null)
        {
            return role.Count == 0;
        }

        return role.SetEquals(originalRole);
    }

    internal bool ShouldTrim(AssociationType associationType, Strategy originalAssociation)
    {
        this.compositeAssociationByAssociationType.TryGetValue(associationType, out var association);
        return Equals(association, originalAssociation);
    }

    internal bool ShouldTrim(AssociationType associationType, Strategy[] originalAssociation)
    {
        this.compositesAssociationByAssociationType.TryGetValue(associationType, out var association);

        if (association == null)
        {
            return originalAssociation == null || originalAssociation.Length == 0;
        }

        if (originalAssociation == null)
        {
            return association.Count == 0;
        }

        return association.SetEquals(originalAssociation);
    }

    private void Backup(RoleType roleType)
    {
        if (roleType.IsMany)
        {
            if (!this.RollbackCompositesRoleByRoleType.ContainsKey(roleType))
            {
                this.compositesRoleByRoleType.TryGetValue(roleType, out var strategies);

                if (strategies == null)
                {
                    this.RollbackCompositesRoleByRoleType[roleType] = null;
                }
                else
                {
                    this.RollbackCompositesRoleByRoleType[roleType] = new HashSet<Strategy>(strategies);
                }
            }
        }
        else if (!this.RollbackCompositeRoleByRoleType.ContainsKey(roleType))
        {
            this.compositeRoleByRoleType.TryGetValue(roleType, out var strategy);

            if (strategy == null)
            {
                this.RollbackCompositeRoleByRoleType[roleType] = null;
            }
            else
            {
                this.RollbackCompositeRoleByRoleType[roleType] = strategy;
            }
        }
    }

    private void Backup(AssociationType associationType)
    {
        if (associationType.IsMany)
        {
            if (!this.RollbackCompositesAssociationByAssociationType.ContainsKey(associationType))
            {
                this.compositesAssociationByAssociationType.TryGetValue(associationType, out var strategies);

                if (strategies == null)
                {
                    this.RollbackCompositesAssociationByAssociationType[associationType] = null;
                }
                else
                {
                    this.RollbackCompositesAssociationByAssociationType[associationType] = new HashSet<Strategy>(strategies);
                }
            }
        }
        else if (!this.RollbackCompositeAssociationByAssociationType.ContainsKey(associationType))
        {
            this.compositeAssociationByAssociationType.TryGetValue(associationType, out var strategy);
            this.RollbackCompositeAssociationByAssociationType[associationType] = strategy;
        }
    }

    private void RemoveCompositeRoleOne2One(RoleType roleType)
    {
        this.AssertNotDeleted();
        this.Transaction.Database.CompositeRoleChecks(this, roleType);

        var previousRole = (Strategy)this.GetCompositeRole(roleType)?.Strategy;
        if (previousRole != null)
        {
            var associationType = roleType.AssociationType;

            this.ChangeLog.OnChangingCompositeRole(this, roleType, null, previousRole);

            previousRole.compositeAssociationByAssociationType.TryGetValue(associationType, out var previousRoleAssociation);
            this.ChangeLog.OnChangingCompositeAssociation(previousRole, associationType, previousRoleAssociation);

            previousRole.Backup(associationType);
            previousRole.compositeAssociationByAssociationType.Remove(associationType);

            // remove role
            this.Backup(roleType);
            this.compositeRoleByRoleType.Remove(roleType);
        }
    }

    private void RemoveCompositeRoleMany2One(RoleType roleType)
    {
        this.AssertNotDeleted();
        this.Transaction.Database.CompositeRoleChecks(this, roleType);

        var previousRole = (Strategy)this.GetCompositeRole(roleType)?.Strategy;

        if (previousRole != null)
        {
            this.ChangeLog.OnChangingCompositeRole(this, roleType, null, previousRole);

            var associationType = roleType.AssociationType;

            previousRole.compositesAssociationByAssociationType.TryGetValue(associationType, out var previousRoleAssociation);
            this.ChangeLog.OnChangingCompositesAssociation(previousRole, associationType, previousRoleAssociation);

            previousRole.Backup(associationType);
            previousRoleAssociation.Remove(this);

            if (previousRoleAssociation.Count == 0)
            {
                previousRole.compositesAssociationByAssociationType.Remove(associationType);
            }

            // remove role
            this.Backup(roleType);
            this.compositeRoleByRoleType.Remove(roleType);
        }
    }

    private void AddCompositeRoleMany2Many(RoleType roleType, Strategy add)
    {
        this.compositesRoleByRoleType.TryGetValue(roleType, out var previousRole);
        if (previousRole?.Contains(add) == true)
        {
            return;
        }

        this.ChangeLog.OnChangingCompositesRole(this, roleType, add, previousRole);

        // Add the new role
        this.Backup(roleType);
        this.compositesRoleByRoleType.TryGetValue(roleType, out var role);
        if (role == null)
        {
            role = new HashSet<Strategy>();
            this.compositesRoleByRoleType[roleType] = role;
        }

        role.Add(add);

        // Add the new association
        var associationType = roleType.AssociationType;

        add.compositesAssociationByAssociationType.TryGetValue(associationType, out var addAssociation);
        this.ChangeLog.OnChangingCompositesAssociation(add, associationType, addAssociation);

        add.Backup(associationType);
        if (addAssociation == null)
        {
            addAssociation = new HashSet<Strategy>();
            add.compositesAssociationByAssociationType[associationType] = addAssociation;
        }

        addAssociation.Add(this);
    }

    private void AddCompositeRoleOne2Many(RoleType roleType, Strategy add)
    {
        this.compositesRoleByRoleType.TryGetValue(roleType, out var previousRole);
        if (previousRole?.Contains(add) == true)
        {
            return;
        }

        this.ChangeLog.OnChangingCompositesRole(this, roleType, add, previousRole);

        var associationType = roleType.AssociationType;

        // 1-...
        add.compositeAssociationByAssociationType.TryGetValue(roleType.AssociationType, out var addPreviousAssociation);

        this.ChangeLog.OnChangingCompositeAssociation(add, associationType, addPreviousAssociation);

        if (addPreviousAssociation != null)
        {
            addPreviousAssociation.compositesRoleByRoleType.TryGetValue(roleType, out var newRolePreviousAssociationRole);
            this.ChangeLog.OnChangingCompositesRole(addPreviousAssociation, roleType, null, newRolePreviousAssociationRole);

            // Remove obsolete role
            addPreviousAssociation.Backup(roleType);
            if (newRolePreviousAssociationRole == null)
            {
                newRolePreviousAssociationRole = new HashSet<Strategy>();
                addPreviousAssociation.compositesRoleByRoleType[roleType] = newRolePreviousAssociationRole;
            }

            newRolePreviousAssociationRole.Remove(add);
            if (newRolePreviousAssociationRole.Count == 0)
            {
                addPreviousAssociation.compositesRoleByRoleType.Remove(roleType);
            }
        }

        // Add the new role
        this.Backup(roleType);
        var role = previousRole;
        if (role == null)
        {
            role = new HashSet<Strategy>();
            this.compositesRoleByRoleType[roleType] = role;
        }

        role.Add(add);

        // Set new association
        this.compositeAssociationByAssociationType.TryGetValue(associationType, out var previousAssociation);
        this.ChangeLog.OnChangingCompositeAssociation(add, associationType, previousAssociation);

        add.Backup(associationType);
        add.compositeAssociationByAssociationType[associationType] = this;
    }

    private void RemoveCompositeRoleMany2Many(RoleType roleType, Strategy remove)
    {
        this.compositesRoleByRoleType.TryGetValue(roleType, out var roleStrategies);
        if (roleStrategies?.Contains(remove) != true)
        {
            return;
        }

        this.ChangeLog.OnChangingCompositesRole(this, roleType, remove, roleStrategies);

        // Remove role
        this.Backup(roleType);
        roleStrategies.Remove(remove);
        if (roleStrategies.Count == 0)
        {
            this.compositesRoleByRoleType.Remove(roleType);
        }

        // Remove association
        var associationType = roleType.AssociationType;

        remove.compositesAssociationByAssociationType.TryGetValue(associationType, out var association);
        this.ChangeLog.OnChangingCompositesAssociation(remove, associationType, association);

        remove.Backup(associationType);
        association.Remove(this);

        if (association.Count == 0)
        {
            remove.compositesAssociationByAssociationType.Remove(associationType);
        }
    }

    private void RemoveCompositeRoleOne2Many(RoleType roleType, Strategy roleToRemove)
    {
        this.compositesRoleByRoleType.TryGetValue(roleType, out var role);
        if (role?.Contains(roleToRemove) != true)
        {
            return;
        }

        this.ChangeLog.OnChangingCompositesRole(this, roleType, roleToRemove, role);

        this.Backup(roleType);

        // Remove role
        role.Remove(roleToRemove);
        if (role.Count == 0)
        {
            this.compositesRoleByRoleType.Remove(roleType);
        }

        // Remove association
        var associationType = roleType.AssociationType;

        roleToRemove.compositeAssociationByAssociationType.TryGetValue(associationType, out var previousAssociation);
        this.ChangeLog.OnChangingCompositeAssociation(roleToRemove, associationType, previousAssociation);

        roleToRemove.Backup(associationType);
        roleToRemove.compositeAssociationByAssociationType.Remove(associationType);
    }

    private void RemoveCompositeRolesMany2Many(RoleType roleType)
    {
        this.compositesRoleByRoleType.TryGetValue(roleType, out var previousRoleStrategies);
        if (previousRoleStrategies != null)
        {
            foreach (var previousRoleStrategy in previousRoleStrategies)
            {
                this.ChangeLog.OnChangingCompositesRole(this, roleType, previousRoleStrategy, previousRoleStrategies);
            }

            foreach (var strategy in new List<Strategy>(previousRoleStrategies))
            {
                this.RemoveCompositeRoleMany2Many(roleType, strategy);
            }
        }
    }

    private void RemoveCompositeRolesOne2Many(RoleType roleType)
    {
        // TODO: Optimize
        this.compositesRoleByRoleType.TryGetValue(roleType, out var previousRoleStrategies);
        if (previousRoleStrategies != null)
        {
            foreach (var strategy in new List<Strategy>(previousRoleStrategies))
            {
                this.RemoveCompositeRoleOne2Many(roleType, strategy);
            }
        }
    }

    private void AssertNotDeleted()
    {
        if (this.IsDeleted)
        {
            throw new Exception($"Object of class {this.UncheckedObjectType.SingularName} with id {this.ObjectId} has been deleted");
        }
    }

    public class ObjectIdComparer : IComparer<Strategy>
    {
        public int Compare(Strategy x, Strategy y) => x.ObjectId.CompareTo(y.ObjectId);
    }
}
