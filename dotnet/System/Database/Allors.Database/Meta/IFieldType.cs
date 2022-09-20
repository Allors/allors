namespace Allors.Database.Meta;

public interface IFieldType : IMetaIdentifiableObject
{
    IRecord Record { get; }

    IDataType DataType { get; }

    string Name { get; }

    bool IsOne { get; }

    bool IsMany { get; }
}
