// <copyright file="AccessControlListFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database.Security;
    using Meta;

    public class WorkspaceAccessControl : IAccessControl
    {
        private readonly ISecurity security;
        private readonly IUser user;
        private readonly IDictionary<IClass, IRoleType> masks;
        private readonly string workspaceName;

        private readonly Dictionary<IObject, IAccessControlList> aclByObject;

        public WorkspaceAccessControl(ISecurity security, IUser user, IDictionary<IClass, IRoleType> masks, string workspaceName)
        {
            this.security = security;
            this.user = user;
            this.masks = masks;
            this.workspaceName = workspaceName;

            this.aclByObject = new Dictionary<IObject, IAccessControlList>();
        }

        public IAccessControlList this[IObject @object]
        {
            get
            {
                if (!this.aclByObject.TryGetValue(@object, out var acl))
                {
                    acl = this.GetAccessControlList((Object)@object);
                    this.aclByObject.Add(@object, acl);
                }

                return acl;
            }
        }

        public bool IsMasked(IObject @object)
        {
            if (!this.masks.TryGetValue(@object.Strategy.Class, out var mask))
            {
                return false;
            }

            var acl = this[@object];
            return !acl.CanRead(mask);
        }

        private WorkspaceAccessControlList GetAccessControlList(Object @object)
        {
            var strategy = @object.Strategy;
            var transaction = strategy.Transaction;

            // Grants
            IEnumerable<ISecurityToken> tokens = null;
            var sharedSecurity = @object.SharedSecurity;
            if (sharedSecurity != null)
            {
                tokens = @object.ExistSecurityTokens ? sharedSecurity.SecurityTokens.Concat(@object.SecurityTokens) : sharedSecurity.SecurityTokens;
            }
            else if (@object.ExistSecurityTokens)
            {
                tokens = @object.SecurityTokens;
            }

            if (tokens == null)
            {
                var securityTokens = new SecurityTokens(transaction);
                tokens = strategy.IsNewInTransaction
                    ? new[] { securityTokens.InitialSecurityToken ?? securityTokens.DefaultSecurityToken }
                    : new[] { securityTokens.DefaultSecurityToken };
            }

            var versionedGrants = this.security.GetVersionedGrants(transaction, this.user, tokens.ToArray(), this.workspaceName);

            // Revocations
            var versionedRevocations = @object.ExistRevocations
                ? this.security
                    .GetVersionedRevocations(transaction, this.user, @object.Revocations.Cast<IRevocation>().ToArray(), this.workspaceName)
                    .Where(v => v.PermissionSet.Any())
                    .ToArray()
                : Array.Empty<IVersionedRevocation>();

            // Access Control List
            return new WorkspaceAccessControlList(this, @object, versionedGrants, versionedRevocations);
        }

        private WorkspaceAccessControlList Create(IObject @object, IVersionedGrant[] grants, IVersionedRevocation[] revocations) => new WorkspaceAccessControlList(this, @object, grants, revocations);
    }
}
