namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;

    public class RecordType : FieldObjectType, IRecordType
    {
        private Type clrType;

        private string[] assignedWorkspaceNames;
        private string[] derivedWorkspaceNames;

        protected RecordType(MetaPopulation metaPopulation, Guid id, string tag = null) : base(metaPopulation, id, tag)
        {
        }

        public override Type ClrType => this.clrType;

        public string[] AssignedWorkspaceNames
        {
            get => this.assignedWorkspaceNames;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.assignedWorkspaceNames = value;
                this.MetaPopulation.Stale();
            }
        }

        public override IEnumerable<string> WorkspaceNames
        {
            get
            {
                this.MetaPopulation.Derive();
                return this.derivedWorkspaceNames;
            }
        }

        public IEnumerable<IFieldType> FieldTypes { get; }

        internal void Bind(Dictionary<string, Type> typeByTypeName) => this.clrType = typeByTypeName[this.Name];

        public void DeriveWorkspaceNames(HashSet<string> workspaceNames)
        {
            this.derivedWorkspaceNames = this.assignedWorkspaceNames ?? Array.Empty<string>();
            workspaceNames.UnionWith(this.derivedWorkspaceNames);
        }
    }
}
