namespace Allors.Embedded
{
    using Meta;

    public interface ICompositeAssociation<TAssociation> where TAssociation : IEmbeddedObject
    {
        TAssociation Value
        {
            get;
        }
    }
}
