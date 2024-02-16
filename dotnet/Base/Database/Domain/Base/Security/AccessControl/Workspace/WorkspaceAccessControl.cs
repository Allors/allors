// <copyright file="AccessControlListFactory.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Database.Security;
    using Allors.Database.Meta;

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
            var accessDelegation = @object.AccessDelegation;

            // Grants
            IEnumerable<ISecurityToken> tokens = null;
            if (accessDelegation != null)
            {
                tokens = @object.ExistSecurityTokens ? accessDelegation.DelegatedSecurityTokens.Concat(@object.SecurityTokens) : accessDelegation.DelegatedSecurityTokens;
            }
            else if (@object.ExistSecurityTokens)
            {
                tokens = @object.SecurityTokens;
            }

            if (tokens == null)
            {
                var cache = @object.Transaction().Scoped<SecurityTokenByUniqueId>();
                tokens = strategy.IsNewInTransaction
                    ? new[] { cache.InitialSecurityToken ?? cache.DefaultSecurityToken }
                    : [cache.DefaultSecurityToken];
            }

            var versionedGrants = this.security.GetVersionedGrants(transaction, this.user, tokens.ToArray(), this.workspaceName);

            // Revocations
            IEnumerable<IRevocation> revocations;
            if (accessDelegation != null)
            {
                // TODO: Remove Union
                revocations = @object.ExistRevocations ? accessDelegation.DelegatedRevocations.Union(@object.Revocations) : accessDelegation.DelegatedRevocations;
            }
            else
            {
                revocations = @object.Revocations;
            }

            var versionedRevocations = this.security
                .GetVersionedRevocations(transaction, this.user, revocations.ToArray(), this.workspaceName)
                .Where(v => v.PermissionSet.Any())
                .ToArray();

            // Access Control List
            return new WorkspaceAccessControlList(this, @object, versionedGrants, versionedRevocations);
        }
    }
}
