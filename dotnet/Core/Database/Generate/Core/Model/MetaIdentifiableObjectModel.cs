namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

public abstract class MetaIdentifiableObjectModel
{
    protected MetaIdentifiableObjectModel(MetaModel metaModel)
    {
        this.MetaModel = metaModel;
    }

    public MetaModel MetaModel { get; }

    public abstract IMetaIdentifiableObject MetaObject { get; }

    public Guid Id => this.MetaObject.Id;

    public string Tag => this.MetaObject.Tag;

    public IEnumerable<string> WorkspaceNames => this.MetaObject.WorkspaceNames;

    public override string ToString() => this.MetaObject.ToString();
}
