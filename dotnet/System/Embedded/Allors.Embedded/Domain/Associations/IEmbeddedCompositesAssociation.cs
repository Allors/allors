namespace Allors.Embedded
{
    using System.Collections.Generic;

    public interface IEmbeddedCompositesAssociation<out T> : IEmbeddedAssociation where T : IEmbeddedObject
    {
        IReadOnlyCollection<T> Value
        {
            get;
        }
    }
}
