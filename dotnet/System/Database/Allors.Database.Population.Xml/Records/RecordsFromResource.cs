namespace Allors.Population
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

        private readonly Assembly assembly;
        private readonly IMetaPopulation metaPopulation;

        public RecordsFromResource(Assembly assembly, IMetaPopulation metaPopulation)
        {
            this.assembly = assembly;
            this.metaPopulation = metaPopulation;

            var name = assembly.GetManifestResourceNames()
                .First(v => v.EndsWith(RecordsXmlFileName, StringComparison.OrdinalIgnoreCase));

            using Stream stream = this.assembly.GetManifestResourceStream(name);
            var reader = new RecordsReader(this.metaPopulation);
            this.RecordsByClass = reader.Read(stream);
        }

        public IDictionary<IClass, Record[]> RecordsByClass { get; }
    }
}
