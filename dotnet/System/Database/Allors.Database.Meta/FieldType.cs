namespace Allors.Database.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FieldType : IFieldType
    {
        private FieldObjectType fieldObjectType;
        private bool isOne;

        private string[] derivedWorkspaceNames;

        public FieldType(Record record, Guid id, string tag = null)
        {
            this.Record = record;
            this.Id = id;
            this.Tag = tag ?? id.Tag();

            this.MetaPopulation.OnFieldTypeCreated(this);
        }

        public IEnumerable<string> WorkspaceNames
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

        IRecord IFieldType.Record => this.Record;
        public Record Record
        {
            get;
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

        IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;
        public MetaPopulation MetaPopulation => this.Record.MetaPopulation;

        internal void DeriveWorkspaceNames() => this.derivedWorkspaceNames = this.FieldObjectType != null ?
            this.Record.WorkspaceNames.Intersect(this.FieldObjectType.WorkspaceNames).ToArray() :
            Array.Empty<string>();
    }
}
