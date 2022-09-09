namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;

public interface IFieldObjectType : IMetaIdentifiableObject
{
    IEnumerable<string> WorkspaceNames { get; }

    string Name { get; }

    string SingularName { get; }

    string PluralName { get; }

    Type ClrType { get; }
}
