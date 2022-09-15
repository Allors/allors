namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;

    public class Record : FieldObjectType
    {
        public Record(ISet<RepositoryObject> objects, string name)
        {
            this.Name = name;

            this.Fields = new HashSet<Field>();

            objects.Add(this);
        }

        public string Name { get; }

        public XmlDoc XmlDoc { get; set; }

        public ISet<Field> Fields { get; }
    }
}
