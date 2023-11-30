namespace Allors.Resources
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Database.Meta;
    using Database.Population;

    public class Handle : IHandle
    {
        public Handle(Record record, XAttribute attribute)
        {
            this.Record = record;
            this.RoleType = record.Class.RoleTypes.First(v => v.Name.Equals("UniqueId", StringComparison.OrdinalIgnoreCase));
            this.Name = attribute.Value;
        }

        public IRecord Record { get; }

        public IRoleType RoleType { get; }
        
        public string Name { get; }

        public object Value => this.Record.ValueByRoleType[this.RoleType];
    }
}
