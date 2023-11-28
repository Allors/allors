namespace Allors.Resources
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using Database.Meta;
    using Database.Population;

    public class Population : IPopulation
    {
        public Population(MetaPopulation metaPopulation)
        {
            XmlDocument document = new XmlDocument();
            using Stream stream = this.GetResource("Allors.Resources.Custom.Population.xml");
            using XmlReader reader = XmlReader.Create(stream);
            document.Load(reader);

            this.ObjectsByClass = new Dictionary<IClass, IObject>();

            var documentElement = document.DocumentElement;

            foreach (var @class in metaPopulation.Classes)
            {
            }
        }

        public IDictionary<IClass, IObject> ObjectsByClass { get; }

        private Stream GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            return resource;
        }

    }
}
