namespace Allors.Population
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Database.Meta;
    using Database.Population;
    using Database.Population.Xml;

    public partial class ResourceRecords
    {
        private readonly IMetaPopulation metaPopulation;

        public ResourceRecords(IMetaPopulation metaPopulation) => this.metaPopulation = metaPopulation;

        public IDictionary<IClass, Record[]> Read()
        {
            using Stream stream = this.GetResource("Allors.Population.Records.xml");
            var reader = new RecordsReader(this.metaPopulation);
            return reader.Read(stream);
        }

        private Stream GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            return resource;
        }
    }
}
