namespace Allors.Embedded.Domain
{
    public interface I1 : IEmbeddedObject
    {
        IEmbeddedCompositesRole<I2> ManyToMany { get; }
    }
}
