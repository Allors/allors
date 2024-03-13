namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

public class MethodTypeModel : IMetaIdentifiableObjectModel
{
    public MethodTypeModel(Model model, MethodType methodType)
    {
        this.Model = model;
        this.MethodType = methodType;
    }

    public Model Model { get; }

    public IMetaExtensible MetaExtensible => this.MetaObject;

    public dynamic Extensions => this.MetaExtensible.Attributes;

    public Guid Id => this.MetaObject.Id;

    public string Tag => this.MetaObject.Tag;

    public IEnumerable<string> WorkspaceNames => this.MetaObject.WorkspaceNames;

    public override string ToString() => this.MetaObject.ToString();

    public MethodType MethodType { get; }

    public IMetaIdentifiableObject MetaObject => this.MethodType;

    // IMethodType
    public CompositeModel ObjectType => this.Model.Map(this.MethodType.ObjectType);

    public string Name => this.MethodType.Name;

    public string FullName => $"{this.MethodType.ObjectType.SingularName}{this.MethodType.Name}";
}
