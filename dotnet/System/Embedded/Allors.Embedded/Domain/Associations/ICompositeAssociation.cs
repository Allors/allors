namespace Allors.Embedded
{
    public interface ICompositeAssociation<TAssociation> : IAssociation where TAssociation : IEmbeddedObject
    {
        TAssociation Value
        {
            get;
        }
    }
}
