namespace Allors.Embedded.Tests.Domain
{
    public interface I2 : IEmbeddedObject
    {
        ICompositesAssociation<I1> Backs { get; }
    }
}
