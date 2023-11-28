namespace Allors.Resources
{
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using Database.Meta;
    using Database.Population;

    public class Population : IPopulation
    {
        public Population(MetaPopulation metaPopulation)
        {
            this.MetaPopulation = metaPopulation;
            XmlDocument document = new XmlDocument();
            using Stream stream = this.GetResource("Allors.Resources.Custom.Population.xml");
            using XmlReader reader = XmlReader.Create(stream);
            document.Load(reader);
            this.XmlDocument = document;
        }

        public MetaPopulation MetaPopulation { get; }

        public XmlDocument XmlDocument { get; set; }

        protected Stream GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            return resource;
        }

        protected byte[] GetResourceBytes(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            using var ms = new MemoryStream();
            resource?.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
