namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IRecordType : IDataType
{
    IReadOnlyList<IFieldType> FieldTypes { get; }
}
