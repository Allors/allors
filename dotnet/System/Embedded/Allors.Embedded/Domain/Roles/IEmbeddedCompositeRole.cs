namespace Allors.Embedded
{
    public interface IEmbeddedCompositeRole<T> : IEmbeddedRole where T : IEmbeddedObject
    {
        T Value
        {
            get;
            set;
        }
    }
}
