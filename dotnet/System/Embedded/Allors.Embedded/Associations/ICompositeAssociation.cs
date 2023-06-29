namespace Allors.Embedded
{
    public interface ICompositeAssociation<TAssociation> where TAssociation : IEmbeddedObject
    {
        TAssociation Value
        {
            get;
        }
    }
}
