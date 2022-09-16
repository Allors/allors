namespace Allors.Repository;

using System;
using System.Collections.Generic;

public abstract class RepositoryObject
{
    protected RepositoryObject()
    {
        this.AttributeByName = new Dictionary<string, Attribute>();
        this.AttributesByName = new Dictionary<string, Attribute[]>();
    }

    public Dictionary<string, Attribute> AttributeByName { get; }

    public Dictionary<string, Attribute[]> AttributesByName { get; }
}
