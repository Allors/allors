namespace Allors.Database.Meta;

public interface IFieldType : IMetaIdentifiableObject
{
    IRecord Record { get; }

    IFieldObjectType FieldObjectType { get; }

    string Name { get; }

    bool IsOne { get; }

    bool IsMany { get; }
}
