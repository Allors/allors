namespace Allors.Embedded
{
    public interface ICompositeRole<TRole> where TRole : IEmbeddedObject
    {
        TRole Value
        {
            get;
            set;
        }
    }
}
