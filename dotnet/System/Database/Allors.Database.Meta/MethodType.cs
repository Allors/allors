// <copyright file="MethodInterface.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class MethodType : IMethodType, IComparable
    {
        private string[] assignedWorkspaceNames;
        private string[] derivedWorkspaceNames;

        private string name;

        private Record input;
        private Record output;

        public MethodType(Composite objectType, Guid id, string tag = null)
        {
            this.MetaPopulation = objectType.MetaPopulation;
            this.ObjectType = objectType;
            this.Id = id;
            this.Tag = tag ?? id.Tag();

            this.MetaPopulation.OnMethodTypeCreated(this);
        }

        public MetaPopulation MetaPopulation { get; }
        IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;

        IComposite IMethodType.ObjectType => this.ObjectType;

        public Guid Id { get; }

        public string Tag { get; }

        public Composite ObjectType { get; }

        public string[] AssignedWorkspaceNames
        {
            get => this.assignedWorkspaceNames ?? Array.Empty<string>();

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.assignedWorkspaceNames = value;
                this.MetaPopulation.Stale();
            }
        }

        public IEnumerable<string> WorkspaceNames
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedWorkspaceNames;
            }
        }

        public string Name
        {
            get => this.name;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.name = value;
                this.MetaPopulation.Stale();
            }
        }

        public string FullName => $"{this.ObjectType.Name}{this.Name}";

        IRecord IMethodType.Input => this.Input;

        public Record Input
        {
            get => this.input;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.input = value;
                this.MetaPopulation.Stale();
            }
        }

        IRecord IMethodType.Output => this.Output;
        public Record Output
        {
            get => this.output;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.output = value;
                this.MetaPopulation.Stale();
            }
        }

        public string DisplayName => this.Name;

        private string ValidationName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Name))
                {
                    return "method type " + this.Name;
                }

                return "unknown method type";
            }
        }

        public override bool Equals(object other) => this.Id.Equals((other as MethodType)?.Id);

        public override int GetHashCode() => this.Id.GetHashCode();

        public int CompareTo(object other) => this.Id.CompareTo((other as MethodType)?.Id);

        public override string ToString() => this.Name;

        public void Validate(ValidationLog validationLog)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                var message = this.ValidationName + " has no name";
                validationLog.AddError(message, this, ValidationKind.Required, "MethodType.Name");
            }
        }

        internal void DeriveWorkspaceNames() =>
            this.derivedWorkspaceNames = this.assignedWorkspaceNames != null
                ? this.assignedWorkspaceNames
                    .Intersect(this.ObjectType.Classes.SelectMany(v => v.WorkspaceNames))
                    .ToArray()
                : Array.Empty<string>();

        internal void PrepareWorkspaceNames(IDictionary<Record, ISet<string>> workspaceNamesByRecord)
        {
            var visited = new HashSet<Record>();

            if (this.derivedWorkspaceNames.Length > 0)
            {
                this.Input?.PrepareWorkspaceNames(workspaceNamesByRecord, visited, this.derivedWorkspaceNames);
                this.Output?.PrepareWorkspaceNames(workspaceNamesByRecord, visited, this.derivedWorkspaceNames);
            }
        }
    }
}
