namespace Allors.Embedded
{
    using Meta;

    public interface ICompositeRole<TRole> where TRole : IEmbeddedObject
    {
        TRole Value
        {
            get;
            set;
        }
    }
}
