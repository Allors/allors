namespace Allors.Repository.Domain;

using System.Collections.Generic;

public class Record
{
    public Record(string name)
    {
        this.Name = name;
        this.PartialByDomainName = new Dictionary<string, PartialRecord>();
    }

    public string Name { get; }

    public XmlDoc XmlDoc { get; set; }

    public Dictionary<string, PartialRecord> PartialByDomainName { get; }

    public Dictionary<string, Field> DefinedFieldByName { get; }
}
