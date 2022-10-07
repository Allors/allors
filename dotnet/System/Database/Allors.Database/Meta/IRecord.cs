namespace Allors.Database.Meta;

using System.Collections.Generic;

public interface IRecord : IDataType
{
    IReadOnlyList<IFieldType> FieldTypes { get; }
}
