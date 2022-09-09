namespace Allors.Database.Meta
{
    public interface IFieldType : IMetaIdentifiableObject
    {
        IFieldObjectType FieldObjectType { get; }

        bool IsOne { get; }

        bool IsMany { get; }
    }
}
