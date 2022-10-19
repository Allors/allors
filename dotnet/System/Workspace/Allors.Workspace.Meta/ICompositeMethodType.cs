namespace Allors.Workspace.Meta
{
    public interface ICompositeMethodType : IMetaExtensible
    {
        IMethodType MethodType { get; }

        IComposite Composite { get; }

        public dynamic Attributes { get; }
    }
}
