namespace Allors.Embedded.Domain
{
    public interface I1 : IEmbeddedObject
    {
        ICompositesRole<I2> ManyToMany { get; }
    }
}
