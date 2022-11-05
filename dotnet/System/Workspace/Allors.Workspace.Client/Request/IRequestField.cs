namespace Allors.Workspace.Request
{
    using Allors.Workspace.Meta;

    public interface IRequestField
    {
        IFieldType FieldType { get; }

        object Value { get; }
    }
}
