namespace Allors.Database.Population
{
    using System.Globalization;
    using Population;

    public class TranslationConfiguration : ITranslationConfiguration
    {
        public CultureInfo DefaultCultureInfo => CultureInfo.GetCultureInfo("en");

        public CultureInfo[] AdditionalCultureInfos => [CultureInfo.GetCultureInfo("nl")];
    }
}
