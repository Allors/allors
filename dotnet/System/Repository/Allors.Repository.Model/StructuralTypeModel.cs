namespace Generate.Model
{
    using Allors.Repository.Domain;
    using System;

    public abstract class StructuralTypeModel : BehavioralTypeModel
    {
        protected StructuralTypeModel(RepositoryModel repositoryModel) : base(repositoryModel)
        {
        }

        public abstract StructuralType StructuralType { get; }

        public Guid Id => this.StructuralType.Id;

        public string SingularName => this.StructuralType.SingularName;

        public DomainModel Domain => this.RepositoryModel.Map(this.StructuralType.Domain);

        public bool IsInterface => this.StructuralType.IsInterface;

        public bool IsClass => this.StructuralType.IsClass;

        public bool IsComposite => this.StructuralType.IsComposite;

        public bool IsUnit => this.StructuralType.IsUnit;
    }
}
