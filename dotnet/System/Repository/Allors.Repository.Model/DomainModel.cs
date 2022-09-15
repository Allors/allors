namespace Generate.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class DomainModel : RepositoryObjectModel
    {
        public DomainModel(RepositoryModel repositoryModel, Domain domain) : base(repositoryModel) => this.Domain = domain;

        public Domain Domain { get; }

        protected override RepositoryObject RepositoryObject => this.Domain;

        public Guid Id => this.Domain.Id;

        public string Name => this.Domain.Name;

        public DomainModel Base => this.RepositoryModel.Map(this.Domain.Base);

        public ISet<ObjectTypeModel> ObjectTypes => new HashSet<ObjectTypeModel>(this.Domain.ObjectTypes.Select(this.RepositoryModel.Map));

        public ISet<PropertyModel> Properties => new HashSet<PropertyModel>(this.Domain.Properties.Select(this.RepositoryModel.Map));

        public ISet<MethodModel> Methods => new HashSet<MethodModel>(this.Domain.Methods.Select(this.RepositoryModel.Map));
    }
}
