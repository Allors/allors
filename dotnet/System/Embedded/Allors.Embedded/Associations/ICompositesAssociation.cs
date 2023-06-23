namespace Allors.Embedded
{
    public interface ICompositesAssociation<TAssociation> where TAssociation : IEmbeddedObject
    {
        TAssociation[] Value
        {
            get;
        }
    }
}
