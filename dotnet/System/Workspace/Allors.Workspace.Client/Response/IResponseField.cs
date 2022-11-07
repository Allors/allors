namespace Allors.Workspace.Response
{
    using Allors.Workspace.Meta;

    public interface IResponseField
    {
        IFieldType FieldType { get; }

        object Value { get; }
    }
}
