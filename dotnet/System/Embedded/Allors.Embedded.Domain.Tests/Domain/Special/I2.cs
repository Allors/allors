namespace Allors.Embedded.Domain
{
    public interface I2 : IEmbeddedObject
    {
        IEmbeddedCompositesAssociation<I1> Backs { get; }
    }
}
