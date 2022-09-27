namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;

public abstract class MetaIdentifiableObject : IMetaIdentifiableObject
{
    protected MetaIdentifiableObject(MetaPopulation metaPopulation, Guid id)
    {
        this.Extensions = new MetaExtension();
        this.MetaPopulation = metaPopulation;
        this.Id = id;
        this.Tag = id.Tag();
    }

    public dynamic Extensions { get; }

    IMetaPopulation IMetaIdentifiableObject.MetaPopulation => this.MetaPopulation;

    public MetaPopulation MetaPopulation { get; }

    public abstract IEnumerable<string> WorkspaceNames { get; }

    public Guid Id { get; }

    public string Tag { get; set; }
}
