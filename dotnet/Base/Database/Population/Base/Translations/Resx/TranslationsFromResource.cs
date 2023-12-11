namespace Allors.Population
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using Database.Meta;
    using Database.Roundtrip;

    public partial class TranslationsFromResource
    {
        private const string ResourcesExtension = ".resources";

        public TranslationsFromResource(IMetaPopulation metaPopulation, ITranslationConfiguration configuration)
        {
            CultureInfo[] cultureInfos = [CultureInfo.InvariantCulture, .. configuration.AdditionalCultureInfos];

            var assembly = Assembly.GetExecutingAssembly();

            this.ResourceSetByCultureInfoByClass = assembly.GetManifestResourceNames()
                .Where(v => v.EndsWith(ResourcesExtension, StringComparison.OrdinalIgnoreCase))
                .Select(v => v.Substring(0, v.Length - ResourcesExtension.Length))
                .ToDictionary(v =>
                {
                    var name = v.Substring(v.LastIndexOf('.') + 1);
                    return metaPopulation.Classes.First(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                }, className =>
                {
                    var resourceManager = new ResourceManager(className, assembly);
                    IDictionary<CultureInfo, ResourceSet> resourceSetByCultureInfo = cultureInfos.ToDictionary(cultureInfo => Equals(CultureInfo.InvariantCulture, cultureInfo) ? configuration.DefaultCultureInfo : cultureInfo, cultureInfo => resourceManager.GetResourceSet(cultureInfo, true, false));
                    return resourceSetByCultureInfo;
                });
        }

        public IDictionary<IClass, IDictionary<CultureInfo, ResourceSet>> ResourceSetByCultureInfoByClass { get; }
    }
}
