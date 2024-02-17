namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public class MethodTypeModel : MetaIdentifiableObjectModel
{
    public MethodTypeModel(Model model, IMethodType methodType)
        : base(model) => this.MethodType = methodType;

    public IMethodType MethodType { get; }

    public override IMetaIdentifiableObject MetaObject => this.MethodType;

    // IMethodType
    public CompositeModel ObjectType => this.Model.Map(this.MethodType.ObjectType);

    public string Name => this.MethodType.Name;

    public string FullName => $"{this.MethodType.ObjectType.SingularName}{this.MethodType.Name}";
}
