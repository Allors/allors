namespace Allors.Embedded
{
    public interface IEmbeddedUnitRole<T> : IEmbeddedRole
    {
        T? EmbeddedValue
        {
            get;
            set;
        }
    }
}
