namespace Allors.Database.Population
{
    using System.Collections.Generic;
    using System.IO;
    using Database;
    using Database.Population.Xml;
    using Database.Meta;
    using Population;

    public partial class RecordsToFile
    {
        private readonly FileInfo fileInfo;
        private readonly MetaPopulation metaPopulation;
        private readonly IRecordRoundtripStrategy recordRoundtripStrategy;

        public RecordsToFile(FileInfo fileInfo, MetaPopulation metaPopulation, IRecordRoundtripStrategy recordRoundtripStrategy)
        {
            this.fileInfo = fileInfo;
            this.metaPopulation = metaPopulation;
            this.recordRoundtripStrategy = recordRoundtripStrategy;
        }

        public void Roundtrip()
        {
            using var stream = File.Open(fileInfo.FullName, FileMode.Create);

            IEnumerable<IObject> objects = this.recordRoundtripStrategy.Objects();

            var recordsByClass = objects.ToRecordsByClass(this.recordRoundtripStrategy);

            var recordsWriter = new RecordsWriter(this.metaPopulation);
            recordsWriter.Write(stream, recordsByClass);
        }
    }
}
