namespace Allors.Meta.Generation.Model;

using System;
using Database.Meta;

public class MethodTypeModel : MetaIdentifiableObjectModel
{
    public MethodTypeModel(MetaModel metaModel, IMethodType methodType)
        : base(metaModel) => this.MethodType = methodType;

    public IMethodType MethodType { get; }

    public override IMetaIdentifiableObject MetaObject => this.MethodType;

    // IMethodType
    public CompositeModel ObjectType => this.MetaModel.Map(this.MethodType.ObjectType);

    public string Name => this.MethodType.Name;

    public string FullName => this.MethodType.FullName;
}
