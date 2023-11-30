namespace Allors.Resources
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Database.Meta;
    using Database.Population;

    public class Population : IPopulation
    {
        public Population(MetaPopulation metaPopulation)
        {
            using Stream stream = this.GetResource("Allors.Resources.Custom.Population.xml");
            XDocument document = XDocument.Load(stream);

            this.ObjectsByClass = new Dictionary<IClass, IRecord[]>();

            var documentElement = document.Elements().First();

            foreach (var @class in metaPopulation.Classes)
            {
                var classElement = documentElement.Elements().FirstOrDefault(v=> @class.PluralName.Equals(v.Name.LocalName, StringComparison.OrdinalIgnoreCase));
                var records = classElement?
                    .Elements()
                    .Select(v => new Record(this, @class, v))
                    .ToArray()
                        ?? Array.Empty<IRecord>();
                this.ObjectsByClass[@class] = records;
            }
        }

        public IDictionary<IClass, IRecord[]> ObjectsByClass { get; }

        private Stream GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            return resource;
        }

    }
}
