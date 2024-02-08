namespace Allors.Embedded
{
    public interface IEmbeddedCompositeAssociation<out T> : IEmbeddedAssociation where T : IEmbeddedObject
    {
        T EmbeddedValue
        {
            get;
        }
    }
}
