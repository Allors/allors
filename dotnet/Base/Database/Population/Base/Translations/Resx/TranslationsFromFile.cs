namespace Allors.Database.Population.Resx
{
    using System.Collections.Generic;
    using System.IO;
    using Database.Population;
    using Database.Meta;

    public partial class TranslationsFromFile
    {
        private readonly DirectoryInfo directoryInfo;
        private readonly IMetaPopulation metaPopulation;

        public TranslationsFromFile(DirectoryInfo directoryInfo, IMetaPopulation metaPopulation)
        {
            this.directoryInfo = directoryInfo;
            this.metaPopulation = metaPopulation;

            if (!this.directoryInfo.Exists)
            {
                this.TranslationsByIsoCodeByClass = new Dictionary<IClass, IDictionary<string, Translations[]>>();
            }
            else
            {
                //using var existingStream = File.Open(this.fileInfo.FullName, FileMode.Open);
                //var recordsReader = new RecordsReader(this.metaPopulation);
                //this.RecordsByClass = recordsReader.Read(existingStream);
            }
        }

        public IDictionary<IClass, IDictionary<string, Translations[]>> TranslationsByIsoCodeByClass { get; }
    }
}
