namespace Generate.Model
{
    using Allors.Repository.Domain;
    using System;

    public abstract class ObjectTypeModel : FieldObjectTypeModel
    {
        protected ObjectTypeModel(RepositoryModel repositoryModel) : base(repositoryModel)
        {
        }

        public abstract ObjectType ObjectType { get; }

        public Guid Id => this.ObjectType.Id;

        public string SingularName => this.ObjectType.SingularName;

        public DomainModel Domain => this.RepositoryModel.Map(this.ObjectType.Domain);
    }
}
