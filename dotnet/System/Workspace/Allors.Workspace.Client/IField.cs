namespace Allors.Workspace
{
    using Allors.Workspace.Meta;

    public interface IField
    {
        IFieldType FieldType { get; }

        object Value { get; }
    }
}
