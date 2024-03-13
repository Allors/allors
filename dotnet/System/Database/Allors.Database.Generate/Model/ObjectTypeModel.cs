namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

public abstract class ObjectTypeModel : IMetaIdentifiableObjectModel
{
    protected ObjectTypeModel(Model model)
    {
        this.Model = model;
    }

    public Model Model { get; }

    public abstract IMetaIdentifiableObject MetaObject { get; }

    public IEnumerable<string> WorkspaceNames => this.MetaObject.WorkspaceNames;

    protected abstract ObjectType ObjectType { get; }

    // IMetaIdentifiableObject
    public Guid Id => this.ObjectType.Id;

    public string Tag => this.ObjectType.Tag;

    // IDataType
    public string Name => this.ObjectType.SingularName;

    public Type ClrType => this.ObjectType.BoundType;

    // IObjectType
    public bool IsUnit => this.ObjectType.IsUnit;

    public bool IsComposite => this.ObjectType.IsComposite;

    public bool IsInterface => this.ObjectType.IsInterface;

    public bool IsClass => this.ObjectType.IsClass;

    public string SingularName => this.ObjectType.SingularName;

    public string PluralName => this.ObjectType.PluralName;

    public bool ExistAssignedPluralName => this.PluralName != null;

    public override string ToString() => this.MetaObject.ToString();
}
