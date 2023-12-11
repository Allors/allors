namespace Allors.Population
{
    using System.Globalization;
    using Database.Roundtrip;

    public class TranslationConfiguration : ITranslationConfiguration
    {
        public CultureInfo DefaultCultureInfo => CultureInfo.GetCultureInfo("en");

        public CultureInfo[] AdditionalCultureInfos => [CultureInfo.GetCultureInfo("nl")];
    }
}
