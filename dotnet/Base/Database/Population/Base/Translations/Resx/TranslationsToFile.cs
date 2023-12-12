namespace Allors.Database.Population.Resx
{
    using System.Collections.Generic;
    using System.IO;
    using Database.Meta;
    using Database.Population;

    public partial class TranslationsToFile
    {
        private readonly DirectoryInfo directoryInfo;
        private readonly IMetaPopulation metaPopulation;

        public TranslationsToFile(DirectoryInfo directoryInfo, IMetaPopulation metaPopulation)
        {
            this.directoryInfo = directoryInfo;
            this.metaPopulation = metaPopulation;
        }

        public IDictionary<IClass, IDictionary<string, Translation[]>> TranslationsByIsoCodeByClass { get; }

        public void Roundtrip()
        {
            //using var stream = File.Open(directoryInfo.FullName, FileMode.Create);

            //IEnumerable<IObject> objects = this.roundtrip.Objects();

            //var recordsByClass = objects.ToRecordsByClass(this.roundtrip.HandleResolver());

            //var recordsWriter = new RecordsWriter(this.metaPopulation);
            //recordsWriter.Write(stream, recordsByClass);
        }
    }
}
