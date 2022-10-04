namespace Allors.Repository.Attributes;

public interface IExtensionAttribute
{
    bool ForRelationType { get; }

    bool ForAssociationType { get; }

    bool ForRoleType { get; }

    bool ForCompositeRoleType { get; }

    string Name { get; }

    string Value { get; }
}
