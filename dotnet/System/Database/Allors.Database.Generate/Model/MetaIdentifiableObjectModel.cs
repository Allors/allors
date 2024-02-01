namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

public abstract class MetaIdentifiableObjectModel : IMetaExtensibleModel
{
    protected MetaIdentifiableObjectModel(Model model)
    {
        this.Model = model;
    }

    public Model Model { get; }

    public abstract IMetaIdentifiableObject MetaObject { get; }

    public IMetaExtensible MetaExtensible => this.MetaObject;

    public dynamic Extensions => this.MetaExtensible.Attributes;

    public Guid Id => this.MetaObject.Id;

    public string Tag => this.MetaObject.Tag;

    public IEnumerable<string> WorkspaceNames => this.MetaObject.WorkspaceNames;

    public override string ToString() => this.MetaObject.ToString();
}
