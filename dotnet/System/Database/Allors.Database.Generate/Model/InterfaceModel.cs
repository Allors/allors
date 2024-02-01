namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public class InterfaceModel : CompositeModel
{
    public InterfaceModel(Model model, IInterface @interface)
        : base(model) => this.Interface = @interface;

    public IInterface Interface { get; }
    public override IMetaIdentifiableObject MetaObject => this.Interface;
    protected override IObjectType ObjectType => this.Interface;
    protected override IComposite Composite => this.Interface;
}
