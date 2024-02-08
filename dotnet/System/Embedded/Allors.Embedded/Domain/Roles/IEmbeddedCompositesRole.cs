namespace Allors.Embedded
{
    using System.Collections.Generic;

    public interface IEmbeddedCompositesRole<T> : IEmbeddedRole where T : IEmbeddedObject
    {
        IReadOnlyCollection<T> EmbeddedValue
        {
            get;
            set;
        }

        void EmbeddedAdd(T value);

        void EmbeddedRemove(T value);
    }
}
