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

    public class FixtureWriter : IFixtureWriter
    {
        public FixtureWriter(IMetaPopulation metaPopulation)
        {
            this.MetaPopulation = metaPopulation;
        }

        public IMetaPopulation MetaPopulation { get; }

        public void Write(Stream stream, Fixture fixture)
        {
            var document = this.Write(fixture);
            document.Save(stream);
        }

        private XDocument Write(Fixture fixture)
        {
            return new XDocument(new XElement("population", fixture.RecordsByClass
                .OrderBy(v => v.Key.Name, StringComparer.OrdinalIgnoreCase)
                .Select(Class)));

            XElement Class(KeyValuePair<IClass, Record[]> grouping) =>
                new(grouping.Key.PluralName,
                    (grouping.Key.KeyRoleType.ObjectType.Tag == UnitTags.String ?
                        grouping.Value.OrderBy(v => (string)v.ValueByRoleType[grouping.Key.KeyRoleType], StringComparer.OrdinalIgnoreCase) :
                        grouping.Value.OrderBy(v => v.ValueByRoleType[grouping.Key.KeyRoleType]))
                    .Select(Object(grouping.Key)));

            Func<Record, XElement> Object(IClass @class) => record =>
                new XElement(@class.SingularName,
                    record.ValueByRoleType.Keys.OrderBy(v => v.Name, StringComparer.OrdinalIgnoreCase)
                        .Select(Role(record)));

            Func<IRoleType, XElement> Role(Record strategy) => roleType =>
                new XElement(roleType.Name, this.WriteString(roleType, strategy.ValueByRoleType[roleType]));

        }

        public string WriteString(IRoleType roleType, object unit) =>
            roleType.ObjectType.Tag switch
            {
                UnitTags.String => (string)unit,
                UnitTags.Integer => XmlConvert.ToString((int)unit),
                UnitTags.Decimal => XmlConvert.ToString((decimal)unit),
                UnitTags.Float => XmlConvert.ToString((double)unit),
                UnitTags.Boolean => XmlConvert.ToString((bool)unit),
                UnitTags.DateTime => XmlConvert.ToString((DateTime)unit, XmlDateTimeSerializationMode.Utc),
                UnitTags.Unique => XmlConvert.ToString((Guid)unit),
                UnitTags.Binary => Convert.ToBase64String((byte[])unit),
                _ => throw new ArgumentException("Unknown Unit Role Type: " + roleType),
            };

    }
}
