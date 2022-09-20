namespace Allors.Repository.Domain;

using System.Collections.Generic;

public class Record : DataType
{
    public Record(ISet<RepositoryObject> objects, string name)
    {
        this.Name = name;

        this.Fields = new HashSet<Field>();

        objects.Add(this);
    }

    public string Name { get; }

    public ISet<Field> Fields { get; }
}
