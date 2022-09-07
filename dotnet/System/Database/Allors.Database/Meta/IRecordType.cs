namespace Allors.Database.Meta
{
    using System.Collections.Generic;

    public interface IRecordType : IFieldObjectType
    {
        string Name { get; }

        IEnumerable<string> WorkspaceNames { get; }

        IEnumerable<IFieldType> FieldTypes { get; }
    }
}
