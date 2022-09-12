namespace Allors.Database.Meta;

using System;

public interface IFieldObjectType : IMetaIdentifiableObject
{
    string Name { get; }

    string SingularName { get; }

    string PluralName { get; }

    Type ClrType { get; }
}
