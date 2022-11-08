namespace Allors.Workspace.Request
{
    using System.Collections.Generic;
    using Allors.Workspace.Meta;

    public interface IPull
    {
        IFieldType OutputFieldType { get; }

        IReadOnlyList<IPull> Children { get; }
    }
}
