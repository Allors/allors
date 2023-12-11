namespace Allors.Database.Roundtrip;

using System.Globalization;

public interface ITranslationConfiguration
{
    CultureInfo DefaultCultureInfo { get; }

    CultureInfo[] AdditionalCultureInfos { get; }
}
