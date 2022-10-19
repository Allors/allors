namespace Allors.Database.Meta;

public interface ICompositeMethodType : IMetaExtensible
{
    public IComposite Composite { get; }

    public IMethodType MethodType{ get; }
}
