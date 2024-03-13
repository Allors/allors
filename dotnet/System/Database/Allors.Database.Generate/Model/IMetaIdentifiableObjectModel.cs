namespace Allors.Meta.Generation.Model;

using System;
using System.Collections.Generic;
using Allors.Database.Meta;

public interface IMetaIdentifiableObjectModel : IMetaExtensibleModel
{
    Model Model { get; }

    IMetaIdentifiableObject MetaObject { get; }

    Guid Id { get; }

    string Tag { get; }

    IEnumerable<string> WorkspaceNames { get; }
}
