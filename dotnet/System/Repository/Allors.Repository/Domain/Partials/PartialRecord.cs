namespace Allors.Repository.Domain;

using System.Collections.Generic;

public class PartialRecord
{
    public PartialRecord(string name)
    {
        this.Name = name;
        this.FieldByName = new Dictionary<string, Field>();
    }

    public string Name { get; }

    public Dictionary<string, Field> FieldByName { get; }

    public override string ToString() => this.Name;
}
