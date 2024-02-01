namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public interface IMetaExtensibleModel
{
    Model Model { get; }

    IMetaExtensible MetaExtensible { get; }

    dynamic Extensions { get; }
}
