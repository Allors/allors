namespace Allors.Workspace.Request
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;

    public interface IInclude
    {
        IFieldType OutputFieldType { get; }

        IReadOnlyList<IInclude> Children { get; }
    }
}
