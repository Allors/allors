namespace Allors.Meta.Generation.Model;

using Database.Meta;

public class InterfaceModel : CompositeModel
{
    public InterfaceModel(MetaModel metaModel, Interface @interface)
        : base(metaModel) => this.Interface = @interface;

    public Interface Interface { get; }
    public override IMetaIdentifiableObject MetaObject => this.Interface;
    protected override IObjectType ObjectType => this.Interface;
    protected override Composite Composite => this.Interface;
}
