namespace Allors.Population
{
    using System.IO;
    using System.Reflection;
    using Database.Meta;
    using Database.Population;
    using Database.Population.Xml;

    public partial class FixtureResource
    {
        private readonly IMetaPopulation metaPopulation;

        public FixtureResource(IMetaPopulation metaPopulation) => this.metaPopulation = metaPopulation;

        public Fixture Read()
        {
            using Stream stream = this.GetResource("Allors.Population.Fixture.xml");
            var reader = new FixtureReader(this.metaPopulation);
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
