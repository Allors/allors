namespace Allors.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Database.Meta;
    using Database.Population;

    public class Record : IRecord
    {
        public const string HandleAttributeName = "handle";

        public Record(Population population, IClass @class, XElement recordElement)
        {
            this.Population = population;
            this.Class = @class;

            this.Handle = recordElement.Attributes()
                .Where(v => v.Name.LocalName.Equals(HandleAttributeName, StringComparison.OrdinalIgnoreCase))
                .Select(v => new Handle(this, v))
                .FirstOrDefault();

            this.ValueByRoleType = new Dictionary<IRoleType, object>(recordElement.Elements()
                .Select(roleElement =>
                {
                    var roleType = this.Class.RoleTypes.First(roleType => roleType.Name.Equals(roleElement.Name.LocalName, StringComparison.OrdinalIgnoreCase));
                    string roleElementValue = roleElement.Value;
                    var value = ReadString(roleElementValue, roleType.ObjectType.Tag);
                    return new KeyValuePair<IRoleType, Object>(roleType, value);
                }));
        }

        public IPopulation Population { get; }

        public IClass Class { get; }

        public IHandle Handle { get; }

        public IDictionary<IRoleType, object> ValueByRoleType { get; }

        public static object ReadString(string value, string tag) =>
            tag switch
            {
                UnitTags.String => value,
                UnitTags.Integer => XmlConvert.ToInt32(value),
                UnitTags.Decimal => XmlConvert.ToDecimal(value),
                UnitTags.Float => XmlConvert.ToDouble(value),
                UnitTags.Boolean => XmlConvert.ToBoolean(value),
                UnitTags.DateTime => XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc),
                UnitTags.Unique => Guid.Parse(value),
                UnitTags.Binary => Convert.FromBase64String(value),
                _ => throw new ArgumentException("Unknown Unit tag: " + tag),
            };
    }
}
