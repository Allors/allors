namespace Allors.Embedded
{
    public interface ICompositesRole<TRole> : IRole where TRole : IEmbeddedObject
    {
        TRole[] Value
        {
            get;
            set;
        }

        void Add(TRole value);

        void Remove(TRole value);
    }
}
