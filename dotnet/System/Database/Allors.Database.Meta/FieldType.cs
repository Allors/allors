namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;

    public class FieldType : IFieldType
    {
        private string[] assignedWorkspaceNames;
        private string[] derivedWorkspaceNames;

        private bool isOne;
        private FieldObjectType fieldObjectType;

        public FieldType(MetaPopulation metaPopulation, Guid id, string tag = null)
        {
            this.MetaPopulation = metaPopulation;
            this.Id = id;
            this.Tag = tag ?? id.Tag();

            this.MetaPopulation.OnFieldTypeCreated(this);
        }

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

        public string[] WorkspaceNames
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedWorkspaceNames;
            }
        }

        public Guid Id { get; }

        public string Tag { get; }

        public bool IsOne
        {
            get => this.isOne;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.isOne = value;
                this.MetaPopulation.Stale();
            }
        }

        public bool IsMany
        {
            get => !this.isOne;

            set => this.IsOne = !value;
        }

        IFieldObjectType IFieldType.FieldObjectType => this.FieldObjectType;
        public FieldObjectType FieldObjectType
        {
            get => this.fieldObjectType;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.fieldObjectType = value;
                this.MetaPopulation.Stale();
            }
        }

        public MetaPopulation MetaPopulation { get; }
        IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;

        public void DeriveWorkspaceNames(HashSet<string> workspaceNames)
        {
            this.derivedWorkspaceNames = this.assignedWorkspaceNames ?? Array.Empty<string>();
            workspaceNames.UnionWith(this.derivedWorkspaceNames);
        }
    }
}
