﻿namespace Allors.Population
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

            string assemblyName = assembly.GetName().Name;
            using Stream stream = this.GetResource($"{assemblyName}.Records.xml");
            var reader = new RecordsReader(this.metaPopulation);
            this.RecordsByClass = reader.Read(stream);
        }

        public IDictionary<IClass, Record[]> RecordsByClass { get; }

        private Stream GetResource(string name)
        {
            var resource = this.assembly.GetManifestResourceStream(name);
            return resource;
        }
    }
}
