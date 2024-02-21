namespace Allors.Database.Population.Xml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Database.Meta;
    using Population;

    public class RecordsReader : IRecordsReader
    {
        public const string HandleAttributeName = "handle";

        public RecordsReader(MetaPopulation metaPopulation)
        {
            this.MetaPopulation = metaPopulation;
        }

        public MetaPopulation MetaPopulation { get; }

        public IDictionary<Class, Record[]> Read(Stream stream)
        {
            var recordsByClass = new Dictionary<Class, Record[]>();

            XDocument document = XDocument.Load(stream);
            var documentElement = document.Elements().First();

            foreach (var classElement in documentElement.Elements())
            {
                var @class = this.MetaPopulation.Classes.First(v => string.Equals(v.PluralName, classElement.Name.LocalName, StringComparison.OrdinalIgnoreCase));
                var records = classElement?
                                  .Elements()
                                  .Select(recordElement =>
                                  {
                                      var valueByRoleType = new Dictionary<RoleType, object>(recordElement.Elements()
                                          .Select(roleElement =>
                                          {
                                              var roleType = @class.RoleTypes.First(roleType => roleType.Name.Equals(roleElement.Name.LocalName, StringComparison.OrdinalIgnoreCase));
                                              string roleElementValue = roleElement.Value;
                                              var value = ReadString(roleElementValue, roleType.ObjectType.Tag);
                                              return new KeyValuePair<RoleType, Object>(roleType, value);
                                          }));

                                      var handle = recordElement.Attributes()
                                          .Where(attribute => attribute.Name.LocalName.Equals(HandleAttributeName, StringComparison.OrdinalIgnoreCase))
                                          .Select(attribute =>
                                          {
                                              var roleType = @class.RoleTypes.First(roleType => roleType.RelationType.IsKey);
                                              var name = attribute.Value;
                                              return new Handle(roleType, name, valueByRoleType[roleType]);
                                          })
                                          .FirstOrDefault();

                                      return new Record(@class, handle, valueByRoleType);
                                  })
                                  .ToArray()
                              ?? Array.Empty<Record>();
                recordsByClass[@class] = records;

            }

            return recordsByClass;
        }

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
