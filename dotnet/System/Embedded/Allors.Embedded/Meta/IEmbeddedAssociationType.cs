namespace Allors.Embedded.Meta
{
    public interface IEmbeddedAssociationType
    {
        string Name { get; }

        string SingularName { get; }

        string PluralName { get; }

        bool IsOne { get; }

        bool IsMany { get; }
    }
}
