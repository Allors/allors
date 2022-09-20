namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IRecord : IDataType
{
    IEnumerable<IFieldType> FieldTypes { get; }
}
