namespace Allors.Embedded
{
    public interface IUnitRole<T> : IRole
    {
        T? Value
        {
            get;
            set;
        }
    }
}
