// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Adapters;
    using Meta;

    public class CompositeRole<T> : ICompositeRole<T>, IOperandInternal
        where T : class, IObject
    {
        private long databaseVersion;
        private long workspaceVersion;

        public CompositeRole(Strategy strategy, IRoleType roleType)
        {
            this.Object = strategy;
            this.RoleType = roleType;
        }

        IStrategy IRelationEnd.Object => this.Object;

        public Strategy Object { get; }

        public IRelationType RelationType => this.RoleType.RelationType;

        T ICompositeRole<T>.Value
        {
            get => this.Object.Workspace.ObjectFactory.Object<T>(this.Value);
            set => this.Value = value?.Strategy;
        }

        public IRoleType RoleType { get; }

        object IRelationEnd.Value => this.Value;

        object IRole.Value
        {
            get => this.Value;
            set => this.Value = (IStrategy)value;
        }

        public IStrategy Value
        {
            get => this.Object.GetCompositeRole(this.RoleType);
            set => this.Object.SetCompositeRole(this.RoleType, value);
        }

        public bool CanRead => this.Object.CanRead(this.RoleType);

        public bool CanWrite => this.Object.CanWrite(this.RoleType);

        public bool Exist => this.Object.ExistRole(this.RoleType);

        public bool IsModified => this.Object.IsModified(this.RoleType);

        public long WorkspaceVersion
        {
            get
            {
                if (this.databaseVersion != this.Object.Workspace.DatabaseVersion)
                {
                    this.databaseVersion = this.Object.Workspace.DatabaseVersion;
                    ++this.workspaceVersion;
                }

                return workspaceVersion;
            }
        }

        public void BumpWorkspaceVersion()
        {
            ++this.workspaceVersion;
        }

        public void Restore()
        {
            this.Object.RestoreRole(this.RoleType);
        }

        public override string ToString()
        {
            return $"{this.RelationType.AssociationType.ObjectType.SingularName}[{this.Object.Id}].{this.RoleType.Name} = [{Value?.Id}]";
        }
    }
}
