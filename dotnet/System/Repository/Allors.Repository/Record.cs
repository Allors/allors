namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Record : FieldObjectType
    {
        public Record(ISet<RepositoryObject> objects, string name)
        {
            this.Name = name;

            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();

            this.FieldByName = new Dictionary<string, Field>();

            objects.Add(this);
        }

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }

        public string Name { get; }

        public XmlDoc XmlDoc { get; set; }

        public Dictionary<string, Field> FieldByName { get; }

        public Field[] Fields => this.FieldByName.Values.ToArray();
    }
}
