namespace Allors.Resources
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Database.Meta;
    using Database.Population;

    public class FixtureReader : IFixtureReader
    {
        public const string HandleAttributeName = "handle";

        public FixtureReader(IMetaPopulation metaPopulation)
        {
            this.MetaPopulation = metaPopulation;
        }

        public IMetaPopulation MetaPopulation { get; }

        public Fixture Read(Stream stream)
        {
            XDocument document = XDocument.Load(stream);

            var fixture = new Fixture(new Dictionary<IClass, Database.Population.Record[]>());

            var documentElement = document.Elements().First();

            foreach (var @class in this.MetaPopulation.Classes)
            {
                var classElement = documentElement.Elements().FirstOrDefault(v => @class.PluralName.Equals(v.Name.LocalName, StringComparison.OrdinalIgnoreCase));
                var records = classElement?
                                  .Elements()
                                  .Select(recordElement =>
                                  {
                                      var valueByRoleType = new Dictionary<IRoleType, object>(recordElement.Elements()
                                          .Select(roleElement =>
                                          {
                                              var roleType = @class.RoleTypes.First(roleType => roleType.Name.Equals(roleElement.Name.LocalName, StringComparison.OrdinalIgnoreCase));
                                              string roleElementValue = roleElement.Value;
                                              var value = ReadString(roleElementValue, roleType.ObjectType.Tag);
                                              return new KeyValuePair<IRoleType, Object>(roleType, value);
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
                              ?? Array.Empty<Database.Population.Record>();
                fixture.RecordsByClass[@class] = records;
            }

            return fixture;
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
