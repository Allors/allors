namespace Allors.Database.Population.Resx
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Database.Meta;
    using Database.Population;
    using KGySoft.Resources;

    public partial class TranslationsToFile
    {
        private readonly DirectoryInfo directoryInfo;
        private readonly RoleType roleType;

        public TranslationsToFile(
            DirectoryInfo directoryInfo,
            IDictionary<Class, IDictionary<string, Translations>> translationsByIsoCodeByClass,
            RoleType roleType)
        {
            this.directoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, "Translations"));
            this.roleType = roleType;
            this.TranslationsByIsoCodeByClass = translationsByIsoCodeByClass;
        }

        public IDictionary<Class, IDictionary<string, Translations>> TranslationsByIsoCodeByClass { get; }

        public void Roundtrip()
        {
            foreach (var (@class, translationsByIsoCode) in this.TranslationsByIsoCodeByClass)
            {
                foreach (var (isoCode, translations) in translationsByIsoCode)
                {
                    var fileName = isoCode.Equals("en", System.StringComparison.OrdinalIgnoreCase)
                        ? $"{@class.SingularName}.{this.roleType.SingularName}.resx"
                        : $"{@class.SingularName}.{this.roleType.SingularName}.{isoCode}.resx";

                    var fileInfo = new FileInfo(Path.Combine(this.directoryInfo.FullName, fileName));

                    using var writer = new ResXResourceWriter(fileInfo.FullName);

                    var valueByKey = translations.ValueByKey;
                    foreach (var (key, value) in valueByKey.OrderBy(v => v.Key))
                    {
                        writer.AddResource(key, value);
                    }
                }
            }
        }
    }
}
