namespace Allors.Population
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Database;
    using Database.Population;
    using Database.Population.Xml;
    using Database.Meta;
    using Database.Roundtrip;

    public partial class FileRecords
    {
        private readonly IDatabase database;
        private readonly FileInfo fileInfo;
        private readonly M m;

        public FileRecords(IDatabase database, FileInfo fileInfo)
        {
            this.database = database;
            this.fileInfo = fileInfo;
            this.m = database.Services.Get<M>();

            if (!this.fileInfo.Exists)
            {
                this.ExistingRecordsByClass = new Dictionary<IClass, Record[]>();
            }
            else
            {
                using var existingStream = File.Open(this.fileInfo.FullName, FileMode.Open);
                var recordsReader = new RecordsReader(this.m);
                this.ExistingRecordsByClass = recordsReader.Read(existingStream);
            }
        }

        public void Write()
        {
            Func<IStrategy, Handle> handleResolver = this.HandleResolver();

            using var stream = File.Open(fileInfo.FullName, FileMode.Create);

            using var transaction = this.database.CreateTransaction();

            IEnumerable<IObject> objects = this.Objects(transaction);

            var recordsByClass = objects.ToRecordsByClass(handleResolver);

            var recordsWriter = new RecordsWriter(m);
            recordsWriter.Write(stream, recordsByClass);
        }
        
        private IDictionary<IClass, Record[]> ExistingRecordsByClass { get; }
    }
}
