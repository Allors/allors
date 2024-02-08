namespace Allors.Embedded
{
    using System.Collections.Generic;

    public interface IEmbeddedCompositesRole<T> : IEmbeddedRole where T : IEmbeddedObject
    {
        IReadOnlyCollection<T> Value
        {
            get;
            set;
        }

        void Add(T value);

        void Remove(T value);
    }
}
