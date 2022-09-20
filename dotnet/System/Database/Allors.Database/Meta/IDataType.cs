namespace Allors.Database.Meta;

using System;

public interface IDataType : IMetaIdentifiableObject
{
    string Name { get; }

    Type ClrType { get; }
}
