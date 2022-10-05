namespace Allors.Database.Meta;

public interface ICompositeMethodType
{
    public IComposite Composite { get; }

    public IMethodType MethodType{ get; }
}
