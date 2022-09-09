namespace Allors.Database.Meta
{
    public interface IFieldType
    {
        IFieldObjectType FieldObjectType { get; }

        bool IsOne { get; }

        bool IsMany { get; }
    }
}
