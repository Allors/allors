namespace Allors.Database.Meta
{
    using System.Collections.Generic;

    public interface IRecordType : IFieldObjectType
    {

        IEnumerable<IFieldType> FieldTypes { get; }
    }
}
