namespace Allors.Meta.Generation.Model;

using Database.Meta;

public class MethodTypeModel : MetaIdentifiableObjectModel
{
    public MethodTypeModel(MetaModel metaModel, MethodType methodType)
        : base(metaModel) => this.MethodType = methodType;

    public MethodType MethodType { get; }

    public override IMetaIdentifiableObject MetaObject => this.MethodType;

    // IMethodType
    public CompositeModel ObjectType => this.MetaModel.Map(this.MethodType.ObjectType);

    public string Name => this.MethodType.Name;

    public string FullName => $"{this.MethodType.ObjectType.Name}{this.MethodType.Name}";

    public RecordModel Input => this.MetaModel.Map(this.MethodType.Input);

    public RecordModel Output => this.MetaModel.Map(this.MethodType.Output);
}
