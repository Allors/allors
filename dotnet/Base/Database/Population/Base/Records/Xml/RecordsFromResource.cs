namespace Allors.Database.Population
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Database.Meta;
    using Database.Population;
    using Database.Population.Xml;

    public partial class RecordsFromResource
    {
        private const string RecordsXmlFileName = "Records.xml";

        public RecordsFromResource(MetaPopulation metaPopulation)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var name = assembly.GetManifestResourceNames()
                .First(v => v.EndsWith(RecordsXmlFileName, StringComparison.OrdinalIgnoreCase));

            using Stream stream = assembly.GetManifestResourceStream(name);
            var reader = new RecordsReader(metaPopulation);
            this.RecordsByClass = reader.Read(stream);
        }

        public IDictionary<Class, Record[]> RecordsByClass { get; }
    }
}
