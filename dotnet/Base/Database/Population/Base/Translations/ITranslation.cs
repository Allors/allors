namespace Allors.Database.Population;

using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Meta;

public interface ITranslation
{
    ITranslationConfiguration Configuration { get; }

    IDictionary<IClass, IDictionary<IRoleType, IDictionary<CultureInfo, ResourceSet>>> ResourceSetByCultureInfoByRoleTypeByClass { get; }
}
