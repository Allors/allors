namespace Allors.Database.Population;

using System.Globalization;

public interface ITranslationConfiguration
{
    CultureInfo DefaultCultureInfo { get; }

    CultureInfo[] AdditionalCultureInfos { get; }
}
