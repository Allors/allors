namespace Allors.Population
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using Database.Meta;
    using Database.Population;

    public partial class TranslationsFromResource
    {
        private const string ResourcesExtension = ".resources";

        private readonly Assembly assembly;
        private readonly IMetaPopulation metaPopulation;

        public TranslationsFromResource(Assembly assembly, IMetaPopulation metaPopulation)
        {
            this.assembly = assembly;
            this.metaPopulation = metaPopulation;

            var resourceManagersByClass = assembly.GetManifestResourceNames()
                .Where(v => v.EndsWith(ResourcesExtension, StringComparison.OrdinalIgnoreCase))
                .Select(v => v.Substring(0, v.Length - ResourcesExtension.Length))
                .ToDictionary(v =>
                {
                    var name = v.Substring(v.LastIndexOf('.') + 1);
                    return metaPopulation.Classes.First(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                }, v => new ResourceManager(v, assembly));


            var resourceManager = resourceManagersByClass.Values.FirstOrDefault();

            var neutral = resourceManager?.GetResourceSet(CultureInfo.InvariantCulture, true, false);
            var english = resourceManager?.GetResourceSet(CultureInfo.GetCultureInfo("en"), true, false);
            var dutch = resourceManager?.GetResourceSet(CultureInfo.GetCultureInfo("nl"), true, false);
        }

        public IDictionary<IClass, IDictionary<string, Translation[]>> TranslationsByIsoCodeByClass { get; }
    }
}
