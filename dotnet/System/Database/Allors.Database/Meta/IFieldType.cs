namespace Allors.Database.Meta
{
    public interface IFieldType
    {
        string Name { get; }

        IFieldObjectType FieldObjectType { get; }

        bool IsOne { get; }

        bool IsMany { get; }
    }
}
