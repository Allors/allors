namespace Generate.Model
{
    using Allors.Repository;
    using Allors.Repository.Domain;
    using System.Collections.Generic;
    using System;

    public class FieldModel : RepositoryObjectModel
    {
        public FieldModel(RepositoryModel repositoryModel, Field field) : base(repositoryModel) => this.Field = field;

        public Field Field { get; }

        protected override RepositoryObject RepositoryObject => this.Field;

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }

        public string Name => this.Field.Name;

        public RecordModel Record => this.RepositoryModel.Map(this.Field.Record);

        public XmlDoc XmlDoc => this.Field.XmlDoc;

        public BehavioralTypeModel Type => this.RepositoryModel.Map(this.Field.Type);

        public bool IsOne => this.Field.IsOne;

        public bool IsMany => this.Field.IsMany;
    }
}
