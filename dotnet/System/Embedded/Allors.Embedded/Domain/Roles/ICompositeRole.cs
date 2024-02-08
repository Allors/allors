namespace Allors.Embedded
{
    public interface ICompositeRole<TRole> : IRole where TRole : IEmbeddedObject
    {
        TRole Value
        {
            get;
            set;
        }
    }
}
