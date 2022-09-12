namespace Allors.Database.Meta
{
    using System.Collections.Generic;

    public interface IRecord : IFieldObjectType
    {
        IEnumerable<IFieldType> FieldTypes { get; }
    }
}
