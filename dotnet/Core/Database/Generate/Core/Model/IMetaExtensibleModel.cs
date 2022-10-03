namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public interface IMetaExtensibleModel
{
    MetaModel MetaModel { get; }

    IMetaExtensible MetaExtensible { get; }

    dynamic Extensions { get; }
}
