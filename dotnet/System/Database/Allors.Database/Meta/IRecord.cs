namespace Allors.Database.Meta;

public interface IRecord : IDataType
{
    IFieldType[] FieldTypes { get; }
}
