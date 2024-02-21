namespace Allors.Database.Population
{
    using System.Collections.Generic;
    using System.IO;
    using Database.Population;
    using Database.Population.Xml;
    using Database.Meta;

    public partial class RecordsFromFile
    {
        private readonly FileInfo fileInfo;
        private readonly MetaPopulation metaPopulation;

        public RecordsFromFile(FileInfo fileInfo, MetaPopulation metaPopulation)
        {
            this.fileInfo = fileInfo;
            this.metaPopulation = metaPopulation;

            if (!this.fileInfo.Exists)
            {
                this.RecordsByClass = new Dictionary<IClass, Record[]>();
            }
            else
            {
                using var existingStream = File.Open(this.fileInfo.FullName, FileMode.Open);
                var recordsReader = new RecordsReader(this.metaPopulation);
                this.RecordsByClass = recordsReader.Read(existingStream);
            }
        }

        public IDictionary<IClass, Record[]> RecordsByClass { get; }
    }
}
