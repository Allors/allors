namespace Allors.Population
{
    using System.Collections.Generic;
    using System.IO;
    using Database;
    using Database.Population.Xml;
    using Database.Meta;
    using Database.Roundtrip;

    public partial class RecordsToFile
    {
        private readonly FileInfo fileInfo;
        private readonly IMetaPopulation metaPopulation;
        private readonly IRoundtripStrategy roundtrip;

        public RecordsToFile(FileInfo fileInfo, IMetaPopulation metaPopulation, IRoundtripStrategy roundtrip)
        {
            this.fileInfo = fileInfo;
            this.metaPopulation = metaPopulation;
            this.roundtrip = roundtrip;
        }

        public void Roundtrip()
        {
            using var stream = File.Open(fileInfo.FullName, FileMode.Create);

            IEnumerable<IObject> objects = this.roundtrip.Objects();

            var recordsByClass = objects.ToRecordsByClass(this.roundtrip.HandleResolver());

            var recordsWriter = new RecordsWriter(this.metaPopulation);
            recordsWriter.Write(stream, recordsByClass);
        }
    }
}
