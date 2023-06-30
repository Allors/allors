namespace Allors.Embedded.Tests.Domain
{
    public interface I1 : IEmbeddedObject
    {
        ICompositesRole<I2> ManyToMany { get; }
    }
}
