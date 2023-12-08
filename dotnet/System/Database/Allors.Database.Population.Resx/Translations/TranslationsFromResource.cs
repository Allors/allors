namespace Allors.Population
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Database.Meta;
    using Database.Population;

    public partial class TranslationsFromResource
    {
        private readonly Assembly assembly;
        private readonly IMetaPopulation metaPopulation;

        public TranslationsFromResource(Assembly assembly, IMetaPopulation metaPopulation)
        {
            this.assembly = assembly;
            this.metaPopulation = metaPopulation;
        }

        public IDictionary<IClass, IDictionary<string, Translation[]>> TranslationsByIsoCodeByClass { get; }

        private Stream GetResource(string name)
        {
            var resource = this.assembly.GetManifestResourceStream(name);
            return resource;
        }
    }
}
