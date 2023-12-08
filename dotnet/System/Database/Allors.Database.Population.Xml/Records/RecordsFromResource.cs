namespace Allors.Population
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Database.Meta;
    using Database.Population;
    using Database.Population.Xml;

    public partial class RecordsFromResource
    {
        private readonly Assembly assembly;
        private readonly IMetaPopulation metaPopulation;

        public RecordsFromResource(Assembly assembly, IMetaPopulation metaPopulation)
        {
            this.assembly = assembly;
            this.metaPopulation = metaPopulation;
        }

        public IDictionary<IClass, Record[]> Read()
        {
            using Stream stream = this.GetResource("Allors.Population.Records.xml");
            var reader = new RecordsReader(this.metaPopulation);
            return reader.Read(stream);
        }

        private Stream GetResource(string name)
        {
            var resource = this.assembly.GetManifestResourceStream(name);
            return resource;
        }
    }
}
