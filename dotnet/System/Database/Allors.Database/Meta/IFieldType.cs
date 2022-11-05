namespace Allors.Database.Meta;

public interface IFieldType : IMetaIdentifiableObject
{
    IRecordType Record { get; }

    IDataType DataType { get; }

    string Name { get; }

    bool IsOne { get; }

    bool IsMany { get; }
}
