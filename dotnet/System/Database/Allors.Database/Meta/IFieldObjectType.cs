namespace Allors.Database.Meta;

using System;

public interface IFieldObjectType : IMetaObject
{
    string Name { get; }
    
    Type ClrType { get; }
}
