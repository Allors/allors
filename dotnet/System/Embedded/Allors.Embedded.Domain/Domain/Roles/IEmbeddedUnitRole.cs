namespace Allors.Embedded
{
    public interface IEmbeddedUnitRole<T> : IEmbeddedRole
    {
        T? Value
        {
            get;
            set;
        }
    }
}
