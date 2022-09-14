namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Record : BehavioralType
    {
        public Record(string name)
        {
            this.Name = name;

            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();

            this.FieldByName = new Dictionary<string, Field>();
        }

        public string Name { get; }

        public XmlDoc XmlDoc { get; set; }

        public Dictionary<string, Field> FieldByName { get; }

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }

        public Field[] Fields => this.FieldByName.Values.ToArray();
    }
}
