namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

public class DomainModel : IMetaIdentifiableObjectModel
{
    public DomainModel(Model model, Domain domain)

    {
        this.Model = model;
        this.Domain = domain;
    }

    public Model Model { get; }

    public Domain Domain { get; }

    public IEnumerable<string> WorkspaceNames => this.MetaObject.WorkspaceNames;

    public IMetaIdentifiableObject MetaObject => this.Domain;

    // IDomain
    public string Name => this.Domain.Name;

    // IMetaIdentifiableObject
    public Guid Id => this.Domain.Id;

    public string Tag => this.Domain.Tag;

    public override string ToString() => this.MetaObject.ToString();

}
