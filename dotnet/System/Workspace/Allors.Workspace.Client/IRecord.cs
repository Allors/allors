namespace Allors.Workspace
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;

    public interface IRecord
    {
        IRecordType RecordType { get; }

        IReadOnlyList<IField> Fields { get; }
    }
}
