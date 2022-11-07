namespace Allors.Workspace.Response
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request;

    public interface IResponseRecord
    {
        IRecordType RecordType { get; }

        IReadOnlyList<IResponseField> Fields { get; }
    }
}
