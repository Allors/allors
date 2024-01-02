namespace Allors.Database.Population.Resx
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Database.Meta;
    using Database.Population;

    public partial class TranslationsToFile
    {
        private readonly DirectoryInfo directoryInfo;
        private readonly IDatabase database;
        private readonly M M;

        public TranslationsToFile(DirectoryInfo directoryInfo, IDatabase database, IDictionary<IClass, IDictionary<string, Translations>> translationsByIsoCodeByClass)
        {
            this.directoryInfo = directoryInfo;
            this.database = database;
            this.TranslationsByIsoCodeByClass = translationsByIsoCodeByClass;

            this.M = this.database.Services.Get<M>();
        }

        public IDictionary<IClass, IDictionary<string, Translations>> TranslationsByIsoCodeByClass { get; }

        public void Roundtrip()
        {
        }
    }
}
