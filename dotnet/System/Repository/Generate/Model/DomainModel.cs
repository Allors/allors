namespace Generate.Model
{
    using System;
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class DomainModel : RepositoryObjectModel
    {
        public DomainModel(RepositoryModel repositoryModel, Domain domain) : base(repositoryModel) => this.Domain = domain;

        public Domain Domain { get; }

        public Guid Id => this.Domain.Id;

        public string Name => this.Domain.Name;

        protected override RepositoryObject RepositoryObject => this.Domain;
    }
}
