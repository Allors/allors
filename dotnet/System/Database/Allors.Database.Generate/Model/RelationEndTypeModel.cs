namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

public abstract class RelationEndTypeModel : IMetaExtensibleModel
{
    protected RelationEndTypeModel(Model model)
    {
        this.Model = model;
    }

    public Model Model { get; }

    // IMetaIdentifiable
    public Guid Id => this.MetaObject.Id;

    public string Tag => this.MetaObject.Tag;

    public IEnumerable<string> WorkspaceNames => this.MetaObject.WorkspaceNames;

    // IRelationEndType
    public IMetaIdentifiableObject MetaObject => this.RelationEndType;

    public ObjectTypeModel ObjectType => this.Model.Map(this.RelationEndType.ObjectType);

    public string Name => this.RelationEndType.Name;

    public string SingularName => this.RelationEndType.SingularName;

    public string SingularFullName => this.RelationEndType.SingularFullName;

    public string PluralName => this.RelationEndType.PluralName;

    public string PluralFullName => this.RelationEndType.PluralFullName;

    public bool IsOne => this.RelationEndType.IsOne;

    public bool IsMany => this.RelationEndType.IsMany;

    protected abstract RelationEndType RelationEndType { get; }

}
