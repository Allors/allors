namespace Allors.Embedded
{
    public interface IEmbeddedCompositeRole<T> : IEmbeddedRole where T : IEmbeddedObject
    {
        T EmbeddedValue
        {
            get;
            set;
        }
    }
}
