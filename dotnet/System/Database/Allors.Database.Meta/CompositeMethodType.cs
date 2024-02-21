namespace Allors.Database.Meta;

public sealed class CompositeMethodType : IMetaExtensible
{
    public CompositeMethodType(IComposite composite, MethodType methodType)
    {
        this.Attributes = new MetaExtension();
        this.Composite = composite;
        this.MethodType = methodType;
    }

    public dynamic Attributes { get; }

    public IComposite Composite { get; }

    public MethodType MethodType { get; }
}
