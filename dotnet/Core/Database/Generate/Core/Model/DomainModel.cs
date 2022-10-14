namespace Allors.Meta.Generation.Model;

using System;
using Allors.Database.Meta;

public class DomainModel : MetaIdentifiableObjectModel
{
    public DomainModel(MetaModel metaModel, IDomain domain)
        : base(metaModel) => this.Domain = domain;

    public IDomain Domain { get; }

    public override IMetaIdentifiableObject MetaObject => this.Domain;

    // IDomain
    public string Name => this.Domain.Name;

    // IMetaIdentifiableObject
    public Guid Id => this.Domain.Id;

    public string Tag => this.Domain.Tag;
}
