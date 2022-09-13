namespace Allors.Repository.Domain;

using System.Collections.Generic;

public class Record
{
    public Record(Domain domain, string name)
    {
        this.Name = name;

        domain.Records.Add(this);
    }

    public string Name { get; }

    public XmlDoc XmlDoc { get; set; }

    public Dictionary<string, Field> FieldByName { get; }
}
