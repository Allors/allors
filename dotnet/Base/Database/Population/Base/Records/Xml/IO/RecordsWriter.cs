namespace Allors.Database.Population.Xml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using CaseExtensions;
    using Database.Meta;
    using Population;

    public class RecordsWriter : IRecordsWriter
    {
        public RecordsWriter(MetaPopulation metaPopulation)
        {
            this.MetaPopulation = metaPopulation;
        }

        public MetaPopulation MetaPopulation { get; }

        public void Write(Stream stream, IDictionary<Class, Record[]> recordsByClass)
        {
            var document = this.Write(recordsByClass);
            document.Save(stream);
        }

        private XDocument Write(IDictionary<Class, Record[]> recordsByClass)
        {
            return new XDocument(new XElement("population",
                recordsByClass
                    .OrderBy(v => v.Key.SingularName, StringComparer.OrdinalIgnoreCase)
                    .Select(Class)));

            XElement Class(KeyValuePair<Class, Record[]> grouping) => new(grouping.Key.PluralName.ToCamelCase(),
                (grouping
                    .Key.KeyRoleType.ObjectType.Tag == UnitTags.String ?
                        grouping.Value.OrderBy(v => (string)v.ValueByRoleType[grouping.Key.KeyRoleType], StringComparer.OrdinalIgnoreCase) :
                        grouping.Value.OrderBy(v => v.ValueByRoleType[grouping.Key.KeyRoleType]))
                    .Select(Object(grouping.Key)));

            Func<Record, XElement> Object(Class @class) => record => new XElement(@class.SingularName.ToCamelCase(),
                Handle(record),
                record.ValueByRoleType
                    .Keys
                    .OrderBy(v => v.SingularName, StringComparer.OrdinalIgnoreCase)
                    .Select(Role(record)));

            XAttribute Handle(Record record) => record.Handle != null ? new XAttribute(RecordsReader.HandleAttributeName, record.Handle.Name) : null;

            Func<RoleType, XElement> Role(Record strategy) => roleType => new XElement(roleType.Name.ToCamelCase(),
                this.WriteString(roleType, strategy.ValueByRoleType[roleType]));
        }

        public string WriteString(RoleType roleType, object unit) =>
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
