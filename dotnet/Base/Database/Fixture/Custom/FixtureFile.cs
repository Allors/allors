namespace Allors.Resources
{
    using System.IO;
    using System.Reflection;
    using Database.Meta;
    using Database.Population;

    public class FixtureFile
    {
        public FixtureFile(IMetaPopulation metaPopulation)
        {
            this.MetaPopulation = metaPopulation;
        }

        public IMetaPopulation MetaPopulation { get; }

        public Fixture Read()
        {
            using Stream stream = this.GetResource("Allors.Resources.Fixture.xml");
            var reader = new PopulationReader(this.MetaPopulation);
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
