namespace Generate.Model;

using System;
using System.Collections.Generic;
using Allors.Repository;
using Allors.Repository.Domain;

public abstract class RepositoryObjectModel
{
    protected RepositoryObjectModel(RepositoryModel repositoryModel) => this.RepositoryModel = repositoryModel;

    public RepositoryModel RepositoryModel { get; }

    protected abstract RepositoryObject RepositoryObject { get; }

    public Dictionary<string, Attribute> AttributeByName => this.RepositoryObject.AttributeByName;

    public Dictionary<string, Attribute[]> AttributesByName => this.RepositoryObject.AttributesByName;

    public string Id => (string)((dynamic)this.AttributeByName.Get("Id"))?.Value;

    public override string ToString() => this.RepositoryObject.ToString();
}
