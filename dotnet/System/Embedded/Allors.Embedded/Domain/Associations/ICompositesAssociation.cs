namespace Allors.Embedded
{
    public interface ICompositesAssociation<TAssociation> : IAssociation where TAssociation : IEmbeddedObject
    {
        TAssociation[] Value
        {
            get;
        }
    }
}
