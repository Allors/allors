namespace Allors.Database.Meta;

using System;

public interface IFieldObjectType : IMetaIdentifiableObject
{
    string Name { get; }

    Type ClrType { get; }
}
