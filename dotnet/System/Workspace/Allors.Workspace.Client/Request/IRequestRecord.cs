namespace Allors.Workspace.Request
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;

    public interface IRequestRecord
    {
        IRecordType RecordType { get; }

        IReadOnlyList<IRequestField> Fields { get; }
    }
}
